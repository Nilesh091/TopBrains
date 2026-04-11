using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
  public class AddressDTOValidator : AbstractValidator<AddressDTO>
  {
    public AddressDTOValidator()
    {
      RuleFor(x => x.userId)
          .NotEmpty().WithMessage("User ID is required.");

      RuleFor(x => x.AddressLine1)
          .NotEmpty().WithMessage("Address Line 1 is required.")
          .Length(5, 100).WithMessage("Address Line 1 must be between 5 and 100 characters.");

      RuleFor(x => x.AddressLine2)
          .MaximumLength(100).WithMessage("Address Line 2 cannot exceed 100 characters.")
          .When(x => !string.IsNullOrEmpty(x.AddressLine2));

      RuleFor(x => x.City)
          .NotEmpty().WithMessage("City is required.")
          .Length(2, 50).WithMessage("City must be between 2 and 50 characters.");

      RuleFor(x => x.State)
          .NotEmpty().WithMessage("State is required.")
          .Length(2, 50).WithMessage("State must be between 2 and 50 characters.");

      RuleFor(x => x.PostalCode)
          .NotEmpty().WithMessage("Postal code is required.")
          .Matches(@"^[a-zA-Z0-9\s\-]{3,20}$").WithMessage("Postal code format is invalid.");

      RuleFor(x => x.Country)
          .NotEmpty().WithMessage("Country is required.")
          .Length(2, 50).WithMessage("Country must be between 2 and 50 characters.");
    }
  }
}
