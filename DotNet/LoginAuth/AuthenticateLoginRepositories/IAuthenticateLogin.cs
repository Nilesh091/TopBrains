using System;
using LoginAuth.Models;

namespace LoginAuth.AuthenticateLoginRepositories
{
    public interface IAuthenticateLogin
    {
        Task<IEnumerable<UserLogin>> GetAll();
        Task<UserLogin> AuthenticateUser(string username, string password);
    }
}
