using Microsoft.AspNetCore.Mvc;
using LoginAuth.AuthenticateLoginRepositories;
namespace LoginAuth.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticateLogin _authenticateLogin;
        public LoginController(IAuthenticateLogin authenticateLogin)
        {
            _authenticateLogin = authenticateLogin;
        }
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var user = _authenticateLogin.AuthenticateUser(username, password);
            if (user.Result != null)
            {
                ViewBag.username = string.Format("Welcome {0}", username);
                TempData["username"] = "Ravi";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.username = "Invalid username or password for" + username;
                return View();
            }
        }

    }
}
