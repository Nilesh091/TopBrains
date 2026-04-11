using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
  public class RefreshTokenRequestDTOValidator : AbstractValidator<RefreshTokenRequestDTO>
  {
    public RefreshTokenRequestDTOValidator()
    {
      RuleFor(x => x.RefreshToken)
          .NotEmpty().WithMessage("Refresh token is required.");

      RuleFor(x => x.ClientId)
          .NotEmpty().WithMessage("Client ID is required.");
    }
  }
}
