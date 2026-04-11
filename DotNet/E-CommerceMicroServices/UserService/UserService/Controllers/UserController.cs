using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    // ✅ GET All Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
      return Ok(await _userRepository.GetAllUsers());
    }

    // ✅ GET User by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
      var user = await _userRepository.GetUserById(id);
      if (user == null) return NotFound();
      return Ok(user);
    }

    // ✅ CREATE a New User
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user)
    {
      if (user == null) return BadRequest();
      User newUser = await _userRepository.CreateUser(user);
      return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
    }

    // ✅ UPDATE User
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
      if (id != user.Id) return BadRequest("User ID mismatch");

      var existingUser = await _userRepository.GetUserById(id);
      if (existingUser == null) return NotFound();
      existingUser.Name = user.Name;
      existingUser.Email = user.Email;
      existingUser.PasswordHash = user.PasswordHash;

      await _userRepository.UpdateUser(existingUser);
      return NoContent();
    }

    // ✅ DELETE User
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      await _userRepository.DeleteUser(id);
      if (id == null) return NotFound();
      return NoContent();
    }
  }
}
