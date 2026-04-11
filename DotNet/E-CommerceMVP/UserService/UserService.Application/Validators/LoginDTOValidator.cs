using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
  public class LoginDTOValidator : AbstractValidator<LoginDTO>
  {
    public LoginDTOValidator()
    {
      RuleFor(x => x.EmailOrUserName)
          .NotEmpty().WithMessage("Email or username is required.");

      RuleFor(x => x.Password)
          .NotEmpty().WithMessage("Password is required.");

      RuleFor(x => x.ClientId)
          .NotEmpty().WithMessage("Client ID is required.");
    }
  }
}
