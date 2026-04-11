using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
  public class UpdateProfileDTOValidator : AbstractValidator<UpdateProfileDTO>
  {
    public UpdateProfileDTOValidator()
    {
      RuleFor(x => x.UserId)
          .NotEmpty().WithMessage("User ID is required.");

      RuleFor(x => x.FullName)
          .NotEmpty().WithMessage("Full name is required.")
          .Length(2, 50).WithMessage("Full name must be between 2 and 50 characters.");

      RuleFor(x => x.PhoneNumber)
          .NotEmpty().WithMessage("Phone number is required.")
          .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in valid international format.");

      RuleFor(x => x.ProfilePhotoUrl)
          .Must(BeValidUrl).WithMessage("Profile photo URL must be a valid URL.")
          .When(x => !string.IsNullOrEmpty(x.ProfilePhotoUrl));
    }

    private static bool BeValidUrl(string? url)
    {
      if (string.IsNullOrEmpty(url))
        return true;
      return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
  }
}
