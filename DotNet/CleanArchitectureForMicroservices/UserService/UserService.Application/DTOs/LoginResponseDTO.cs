using System;

namespace UserService.Application.DTOs
{
    public class LoginResponseDTO
    {
        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public string? ErrorMessage { get; set; }
        public int? RemainingAttempts { get; set; }
        public List<string>? Roles { get; set; } // Add roles for frontend authorization
        public Guid? UserId { get; set; } // Return UserId when 2FA is required or on successful login

    }
}
