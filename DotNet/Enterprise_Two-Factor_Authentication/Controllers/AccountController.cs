using Enterprise_Two_Factor_Authentication.Models;
using Enterprise_Two_Factor_Authentication.Services;
using Enterprise_Two_Factor_Authentication.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Enterprise_Two_Factor_Authentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly QRCodeService _qrCodeService;
        private readonly EmailService _emailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            QRCodeService qrService,
            EmailService emailService
        )
        {
            this._qrCodeService = qrService;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._emailService = emailService;
        }

        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                TempData["RegisterSuccess"] = $"Account created successfully for {user.Email}. Please setup Two-Factor Authentication.";

                return RedirectToAction("Enable2FAuth");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        public IActionResult Enable2FAuth()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            true);
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("VerifyAuthenticatorLogin");
            }
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (!user.TwoFactorEnabled)
                {
                    TempData["Enable2FA"] = "You must enable Two-Factor Authentication before continuing.";

                    return RedirectToAction("Enable2FAuth");
                }


                TempData["LoginSuccess"] = $"Welcome {user.Email}, you logged in successfully!";


                return RedirectToAction("Dashboard", "Home");
            }
            ModelState.AddModelError("", "Invalid Login");
            return View(model);
        }
        // SETUP AUTHENTICATOR
        public async Task<IActionResult> SetupAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);

            var key = await _userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                key = await _userManager.GetAuthenticatorKeyAsync(user);
            }
            string issuer = "Enterprise_Two_Factor_Authentication";
            string authenticatorUri =
                $"otpauth://totp/{issuer}:{user.Email}?secret={key}&issuer={issuer}";

            var qrCode = _qrCodeService.GenerateQRCode(authenticatorUri);

            var model = new AuthenticatorSetupViewModel
            {
                QRCodeImage = qrCode,
                ManualKey = key
            };

            return View(model);
        }
        public IActionResult VerifyAuthenticator()
        {
            return View();
        }
        // VERIFY AUTHENTICATOR
        [HttpPost]
        public async Task<IActionResult> VerifyAuthenticator(VerifyAuthenticatorViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            var valid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                _userManager.Options.Tokens.AuthenticatorTokenProvider,
                model.Code);

            if (!valid)
            {
                ModelState.AddModelError("", "Invalid Code");
                return View(model);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            return RedirectToAction("Dashboard", "Home");
        }

        // LOGIN OTP
        public IActionResult VerifyAuthenticatorLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAuthenticatorLogin(VerifyAuthenticatorViewModel model)
        {
            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                model.Code,
                model.RememberMe,
                model.RememberMachine);

            if (result.Succeeded)
            {
                TempData["LoginSuccess"] = $"Welcome {User.Identity.Name}, you logged in successfully!";
                return RedirectToAction("Dashboard", "Home");
            }

            ModelState.AddModelError("", "Invalid Code");

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["LogoutMessage"] = "You have logged out successfully.";

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult EmailLogin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendOTP(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View("EmailLogin");
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider
            );

            await _emailService.SendEmailAsync(
                email,
                "Your Login OTP",
                $"Your OTP code is: <b>{token}</b>"
            );

            // store time
            TempData["OTPTime"] = DateTime.UtcNow;
            TempData["Email"] = email;

            return RedirectToAction("VerifyEmailOTP", new { email = email });
        }


        [HttpGet]
        public IActionResult VerifyEmailOTP(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmailOTP(string email, string code)
        {
            if (TempData["OTPTime"] == null)
            {
                ModelState.AddModelError("", "OTP expired");
                return RedirectToAction("EmailLogin");
            }

            DateTime otpTime = (DateTime)TempData["OTPTime"];

            if (DateTime.UtcNow > otpTime.AddMinutes(5))
            {
                ModelState.AddModelError("", "OTP expired");
                return RedirectToAction("EmailLogin");
            }

            var user = await _userManager.FindByEmailAsync(email);

            var valid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider,
                code
            );

            if (valid)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid OTP");
            ViewBag.Email = email;
            return View();
        }

    }
}
