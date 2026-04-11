using FluentValidation;
using System.Text.RegularExpressions;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
  public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
  {
    public RegisterDTOValidator()
    {
      RuleFor(x => x.UserName)
          .NotEmpty().WithMessage("Username is required.")
          .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
          .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Username can only contain letters, numbers, underscores, and hyphens.");

      RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Email is required.")
          .EmailAddress().WithMessage("Invalid email address format.");

      RuleFor(x => x.Password)
          .NotEmpty().WithMessage("Password is required.")
          .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
          .Matches(@"^(?=.*[a-z])").WithMessage("Password must contain at least one lowercase letter.")
          .Matches(@"^(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.")
          .Matches(@"^(?=.*\d)").WithMessage("Password must contain at least one digit.")
          .Must(BeValidPassword).WithMessage("Password must not be commonly used.");

      RuleFor(x => x.PhoneNumber)
          .Matches(@"^\+?[1-9]\d{1,14}$", RegexOptions.IgnoreCase)
          .WithMessage("Phone number must be in valid international format.")
          .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

      RuleFor(x => x.FullName)
          .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.")
          .When(x => !string.IsNullOrEmpty(x.FullName));
    }

    private static bool BeValidPassword(string password)
    {
      // List of commonly used passwords (simplified)
      var commonPasswords = new[] { "password", "123456", "qwerty", "abc123", "password123" };
      return !commonPasswords.Contains(password.ToLower());
    }
  }
}
