using jwtAuth.AuthService;
using jwtAuth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jwtAuth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            if (model.Username == "admin" && model.Password == "password")
            {
                var token = _authService.GenerateToken(model.Username);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
