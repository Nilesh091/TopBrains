using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
  public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordDTO>
  {
    public ChangePasswordDTOValidator()
    {
      RuleFor(x => x.CurrentPassword)
          .NotEmpty().WithMessage("Current password is required.");

      RuleFor(x => x.NewPassword)
          .NotEmpty().WithMessage("New password is required.")
          .MinimumLength(8).WithMessage("New password must be at least 8 characters.")
          .Matches(@"^(?=.*[a-z])").WithMessage("Password must contain at least one lowercase letter.")
          .Matches(@"^(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.")
          .Matches(@"^(?=.*\d)").WithMessage("Password must contain at least one digit.")
          .NotEqual(x => x.CurrentPassword).WithMessage("New password must be different from current password.");
    }
  }
}
