using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsers() => await _context.Users.ToListAsync();

    public async Task<User> GetUserById(int id) => await _context.Users.FindAsync(id);

    public async Task<User> GetUserByEmail(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddUser(User user)
    {
      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
      _context.Users.Update(user);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user != null)
      {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
      }
    }
    public async Task<User> CreateUser(User user)
    {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      return user;
    }
  }
}
