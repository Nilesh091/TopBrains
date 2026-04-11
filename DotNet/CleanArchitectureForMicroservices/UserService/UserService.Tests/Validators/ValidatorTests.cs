using FluentAssertions;
using FluentValidation.TestHelper;
using UserService.Application.DTOs;
using UserService.Application.Validators;
using Xunit;

namespace UserService.Tests.Validators
{
  public class RegisterDTOValidatorTests
  {
    private readonly RegisterDTOValidator _validator;

    public RegisterDTOValidatorTests()
    {
      _validator = new RegisterDTOValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldHaveNoErrors()
    {
      // Arrange
      var dto = new RegisterDTO
      {
        UserName = "validuser",
        Email = "valid@example.com",
        Password = "SecurePass123!",
        FullName = "Valid User",
        PhoneNumber = "+1234567890"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithMissingUserName_ShouldHaveError()
    {
      // Arrange
      var dto = new RegisterDTO
      {
        UserName = "",
        Email = "test@example.com",
        Password = "SecurePass123!"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldHaveError()
    {
      // Arrange
      var dto = new RegisterDTO
      {
        UserName = "testuser",
        Email = "invalid-email",
        Password = "SecurePass123!"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithWeakPassword_ShouldHaveError()
    {
      // Arrange
      var dto = new RegisterDTO
      {
        UserName = "testuser",
        Email = "test@example.com",
        Password = "weak"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WithInvalidPhoneFormat_ShouldHaveError()
    {
      // Arrange
      var dto = new RegisterDTO
      {
        UserName = "testuser",
        Email = "test@example.com",
        Password = "SecurePass123!",
        PhoneNumber = "invalid-phone"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }
  }

  public class LoginDTOValidatorTests
  {
    private readonly LoginDTOValidator _validator;

    public LoginDTOValidatorTests()
    {
      _validator = new LoginDTOValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldHaveNoErrors()
    {
      // Arrange
      var dto = new LoginDTO
      {
        EmailOrUserName = "test@example.com",
        Password = "SecurePass123!",
        ClientId = "web"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithMissingClientId_ShouldHaveError()
    {
      // Arrange
      var dto = new LoginDTO
      {
        EmailOrUserName = "test@example.com",
        Password = "SecurePass123!",
        ClientId = ""
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldHaveValidationErrorFor(x => x.ClientId);
    }
  }

  public class UpdateProfileDTOValidatorTests
  {
    private readonly UpdateProfileDTOValidator _validator;

    public UpdateProfileDTOValidatorTests()
    {
      _validator = new UpdateProfileDTOValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldHaveNoErrors()
    {
      // Arrange
      var dto = new UpdateProfileDTO
      {
        UserId = Guid.NewGuid(),
        FullName = "Updated Name",
        PhoneNumber = "+1234567890",
        ProfilePhotoUrl = "https://example.com/photo.jpg"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithInvalidPhoneFormat_ShouldHaveError()
    {
      // Arrange
      var dto = new UpdateProfileDTO
      {
        UserId = Guid.NewGuid(),
        FullName = "Valid Name",
        PhoneNumber = "invalid-phone",
        ProfilePhotoUrl = "https://example.com/photo.jpg"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void Validate_WithInvalidUrl_ShouldHaveError()
    {
      // Arrange
      var dto = new UpdateProfileDTO
      {
        UserId = Guid.NewGuid(),
        FullName = "Valid Name",
        PhoneNumber = "+1234567890",
        ProfilePhotoUrl = "not-a-valid-url"
      };

      // Act
      var result = _validator.TestValidate(dto);

      // Assert
      result.ShouldHaveValidationErrorFor(x => x.ProfilePhotoUrl);
    }
  }
}
