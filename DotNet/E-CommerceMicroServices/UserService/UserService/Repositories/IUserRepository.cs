using UserService.Models;

namespace UserService.Repositories
{
  public interface IUserRepository
  {
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> GetUserByEmail(string email);
    Task AddUser(User user);
    Task<User> CreateUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(int id);
  }
}
