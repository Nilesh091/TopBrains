using System;

namespace jwtAuth.AuthService
{
    public interface IAuthService
    {
        string GenerateToken(string username);
    }
}
