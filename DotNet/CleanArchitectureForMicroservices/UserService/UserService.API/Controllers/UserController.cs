using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UAParser;
using UserService.API.DTOs;
using UserService.Application.DTOs;
using UserService.Application.Services;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            try
            {
                var result = await _userService.RegisterAsync(dto);
                if (!result)
                    return BadRequest(ApiResponse<string>.FailResponse("Registration failed. Email or username might already exist."));

                // Send welcome email via Notification Service
                await _emailService.SendWelcomeEmailAsync(dto.Email, dto.UserName);

                return Ok(ApiResponse<string>.SuccessResponse("User registered successfully. Please check your email for confirmation."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error during registration.", new List<string> { ex.Message }));
            }
        }
        [HttpPost("send-confirmation-email")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> SendConfirmationEmail([FromBody] EmailDTO dto)
        {
            try
            {
                var emailTokenResponse = await _userService.SendConfirmationEmailAsync(dto.Email);
                if (emailTokenResponse == null)
                    return NotFound(ApiResponse<string>.FailResponse("User with this email not found"));

                // Send confirmation email via Notification Service instead of returning token
                var emailSent = await _emailService.SendEmailConfirmationAsync(dto.Email, emailTokenResponse.Token, emailTokenResponse.UserId);
                if (!emailSent)
                    return StatusCode(500, ApiResponse<string>.FailResponse("Failed to send confirmation email."));

                return Ok(ApiResponse<string>.SuccessResponse("Confirmation email sent successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error sending confirmation email.", new List<string> { ex.Message }));
            }
        }
        [HttpPost("verify-email")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<IActionResult> VerifyConfirmationEmailAsync([FromBody] ConfirmEmailDTO dto)
        {
            try
            {
                var success = await _userService.VerifyConfirmationEmailAsync(dto);
                if (!success)
                    return BadRequest(ApiResponse<string>.FailResponse("Invalid confirmation token or user."));
                return Ok(ApiResponse<string>.SuccessResponse("Email confirmed successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error confirming email.", new List<string> { ex.Message }));
            }
        }
        [HttpPost("login")]
        [EnableRateLimiting("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            var UserAgent = GetNormalizedUserAgent();
            var loginResponse = await _userService.LoginAsync(dto, IPAddress, UserAgent);

            if (!string.IsNullOrEmpty(loginResponse.ErrorMessage))
            {
                loginResponse.Succeeded = false;
                return Unauthorized(ApiResponse<LoginResponseDTO>.FailResponse(loginResponse.ErrorMessage, errors: null, data: loginResponse));
            }

            loginResponse.Succeeded = true;
            return Ok(ApiResponse<LoginResponseDTO>.SuccessResponse(loginResponse,
                loginResponse.RequiresTwoFactor ? "Two-factor authentication required. OTP sent to your email." : "Login successful."));
        }

        [HttpPost("verify-otp")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDTO dto)
        {
            try
            {
                var IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
                var UserAgent = GetNormalizedUserAgent();

                var otpResponse = await _userService.VerifyOtpAsync(dto, IPAddress, UserAgent);

                if (!string.IsNullOrEmpty(otpResponse.ErrorMessage))
                {
                    otpResponse.Succeeded = false;
                    return Unauthorized(ApiResponse<LoginResponseDTO>.FailResponse(otpResponse.ErrorMessage, errors: null, data: otpResponse));
                }

                otpResponse.Succeeded = true;
                return Ok(ApiResponse<LoginResponseDTO>.SuccessResponse(otpResponse, "OTP verified successfully. Login completed."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LoginResponseDTO>.FailResponse("Error verifying OTP.", new List<string> { ex.Message }));
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO dto)
        {
            var IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            var UserAgent = GetNormalizedUserAgent();
            var refreshTokenResponse = await _userService.RefreshTokenAsync(dto, IPAddress, UserAgent);
            if (!string.IsNullOrEmpty(refreshTokenResponse.ErrorMessage))
                return Unauthorized(ApiResponse<string>.FailResponse(refreshTokenResponse.ErrorMessage));
            return Ok(ApiResponse<RefreshTokenResponseDTO>.SuccessResponse(refreshTokenResponse, "Token refreshed successfully."));
        }
        [HttpPost("revoke-token")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDTO dto)
        {
            try
            {
                var success = await _userService.RevokeRefreshTokenAsync(dto.RefreshToken, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "");
                if (!success)
                    return BadRequest(ApiResponse<string>.FailResponse("Invalid token or token already revoked."));
                return Ok(ApiResponse<string>.SuccessResponse("Token revoked successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error revoking token.", new List<string> { ex.Message }));
            }
        }
        [HttpGet("profile/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<ProfileDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            try
            {
                var profile = await _userService.GetProfileAsync(userId);
                if (profile == null)
                    return NotFound(ApiResponse<string>.FailResponse("User profile not found."));
                return Ok(ApiResponse<ProfileDTO>.SuccessResponse(profile));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error fetching profile.", new List<string> { ex.Message }));
            }
        }
        [Authorize]
        [HttpPut("profile")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized(ApiResponse<string>.FailResponse("Invalid user token."));

                // Authorization check: users can only update their own profile
                if (dto.UserId != userId)
                    return Forbid();

                var success = await _userService.UpdateProfileAsync(dto);
                if (!success)
                    return BadRequest(ApiResponse<string>.FailResponse("Failed to update profile."));

                return Ok(ApiResponse<string>.SuccessResponse("Profile updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error updating profile.", new List<string> { ex.Message }));
            }
        }
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailDTO dto)
        {
            try
            {
                var forgotPassword = await _userService.ForgotPasswordAsync(dto.Email);
                if (forgotPassword == null)
                    return NotFound(ApiResponse<string>.FailResponse("Email not found."));

                // Send password reset email via Notification Service instead of returning token
                var emailSent = await _emailService.SendPasswordResetAsync(dto.Email, forgotPassword.Token, forgotPassword.UserId);
                if (!emailSent)
                    return StatusCode(500, ApiResponse<string>.FailResponse("Failed to send password reset email."));

                return Ok(ApiResponse<string>.SuccessResponse("Password reset email sent successfully. Check your inbox."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error in forgot password process.", new List<string> { ex.Message }));
            }
        }
        // Reset Password (Forgot Password Flow)
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            try
            {
                var success = await _userService.ResetPasswordAsync(dto.UserId, dto.Token, dto.NewPassword);
                if (!success)
                    return BadRequest(ApiResponse<string>.FailResponse("Invalid token or user."));
                return Ok(ApiResponse<string>.SuccessResponse("Password reset successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error resetting password.", new List<string> { ex.Message }));
            }
        }
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        [ProducesResponseType(typeof(ApiResponse<string>), 401)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized(ApiResponse<string>.FailResponse("Invalid user token."));
                var success = await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
                if (!success)
                    return BadRequest(ApiResponse<string>.FailResponse("Password change failed."));
                return Ok(ApiResponse<string>.SuccessResponse("Password changed successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error changing password.", new List<string> { ex.Message }));
            }
        }
        [Authorize]
        [HttpPost("addresses")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> AddOrUpdateAddress([FromBody] AddressDTO dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized(ApiResponse<string>.FailResponse("Invalid user token."));

                // Authorization check: users can only manage their own addresses
                if (dto.userId != userId)
                    return Forbid();

                var addressId = await _userService.AddOrUpdateAddressAsync(dto);
                if (addressId == Guid.Empty)
                    return BadRequest(ApiResponse<string>.FailResponse("Failed to add or update address."));

                return Ok(ApiResponse<Guid>.SuccessResponse(addressId, "Address saved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error managing address.", new List<string> { ex.Message }));
            }
        }
        [HttpGet("{userId}/addresses")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AddressDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAddresses(Guid userId)
        {
            try
            {
                var addresses = await _userService.GetAddressesAsync(userId);
                return Ok(ApiResponse<IEnumerable<AddressDTO>>.SuccessResponse(addresses));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error fetching addresses.", new List<string> { ex.Message }));
            }
        }
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.NotFound)]
        [HttpPost("delete-address")]
        public async Task<IActionResult> DeleteAddress([FromBody] DeleteAddressDTO dto)
        {
            try
            {
                var deleted = await _userService.DeleteAddressAsync(dto.UserId, dto.AddressId);
                if (!deleted)
                    return BadRequest(ApiResponse<string>.FailResponse("Address not found or deletion failed."));
                return Ok(ApiResponse<string>.SuccessResponse("Address deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error deleting address.", new List<string> { ex.Message }));
            }
        }
        [HttpGet("{userId}/exists")]
        public async Task<IActionResult> UserExists(Guid userId)
        {
            bool exists = await _userService.IsUserExistsAsync(userId);
            var response = new ApiResponse<bool>
            {
                Success = true,
                Data = exists,
                Message = exists ? "User exists." : "User does not exist."
            };
            return Ok(response);
        }
        [HttpGet("{userId}/address/{addressId}")]
        [ProducesResponseType(typeof(ApiResponse<AddressDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserAddress(Guid userId, Guid addressId)
        {
            try
            {
                var address = await _userService.GetAddressByUserIdAndAddressIdAsync(userId, addressId);
                if (address != null)
                    return Ok(ApiResponse<AddressDTO>.SuccessResponse(address));
                return NotFound(ApiResponse<string>.FailResponse("Address not found."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error fetching addresses.", new List<string> { ex.Message }));
            }
        }
        private string GetNormalizedUserAgent()
        {
            var userAgentRaw = HttpContext.Request.Headers["User-Agent"].ToString();
            if (string.IsNullOrWhiteSpace(userAgentRaw))
                return "Unknown";
            try
            {
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgentRaw);
                var browser = clientInfo.UA.Family ?? "UnknownBrowser";
                var browserVersion = clientInfo.UA.Major ?? "0";
                var os = clientInfo.OS.Family ?? "UnknownOS";
                return $"{browser}-{browserVersion}_{os}";
            }
            catch
            {
                // In case parsing fails, fallback to raw user agent or unknown
                return "Unknown";
            }
        }

    }
}
