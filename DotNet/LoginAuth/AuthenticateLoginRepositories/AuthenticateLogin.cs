using System;
using LoginAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAuth.AuthenticateLoginRepositories
{
    public class AuthenticateLogin : IAuthenticateLogin
    {
        private readonly LoginDbContext _context;
        public AuthenticateLogin(LoginDbContext context)
        {
            _context = context;
        }

        public async Task<UserLogin> AuthenticateUser(String username, string pasword)
        {
            var successed = await _context.UserLogins.FirstOrDefaultAsync(u => u.Username == username && u.Password == pasword);
            return successed;
        }
        public async Task<IEnumerable<UserLogin>> GetAll()
        {
            return await _context.UserLogins.ToListAsync();
        }

    }
}
