using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public AuthController(IUserRepository userRepository, IConfiguration config)
    {
      _userRepository = userRepository;
      _configuration = config;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
      var user = new User
      {
        Name = model.Name,
        Email = model.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
      };

      await _userRepository.CreateUser(user);
      return Ok("User registered successfully");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
      var user = await _userRepository.GetUserByEmail(model.Email);
      if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
      {
        return Unauthorized("Invalid credentials");
      }

      var token = GenerateJwtToken(user);
      return Ok(new { Token = token });
    }
    private string GenerateJwtToken(User user)
    {
      var jwtSettings = _configuration.GetSection("JwtSettings");
      var secretKey = jwtSettings["Secret"];
      var issuer = jwtSettings["Issuer"];
      var audience = jwtSettings["Audience"];

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };

      var token = new JwtSecurityToken(
          issuer,
          audience,
          claims,
          expires: DateTime.UtcNow.AddHours(10),
          signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
