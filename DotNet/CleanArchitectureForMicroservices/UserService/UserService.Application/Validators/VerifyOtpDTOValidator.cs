using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
  public class VerifyOtpDTOValidator : AbstractValidator<VerifyOtpDTO>
  {
    public VerifyOtpDTOValidator()
    {
      RuleFor(x => x.UserId)
          .NotEmpty()
          .WithMessage("User ID is required.");

      RuleFor(x => x.OtpCode)
          .NotEmpty()
          .WithMessage("OTP code is required.")
          .Length(6)
          .WithMessage("OTP code must be exactly 6 digits.")
          .Matches(@"^\d{6}$")
          .WithMessage("OTP code must contain only numeric digits.");

      RuleFor(x => x.ClientId)
          .NotEmpty()
          .WithMessage("Client ID is required.");
    }
  }
}
