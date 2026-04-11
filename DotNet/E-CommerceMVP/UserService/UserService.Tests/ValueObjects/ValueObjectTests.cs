using FluentAssertions;
using UserService.Domain.ValueObjects;
using Xunit;

namespace UserService.Tests.ValueObjects
{
  public class EmailValueObjectTests
  {
    [Fact]
    public void Create_WithValidEmail_ShouldSucceed()
    {
      // Arrange
      var emailString = "test@example.com";

      // Act
      var email = Email.Create(emailString);

      // Assert
      email.Should().NotBeNull();
      email.Value.Should().Be(emailString);
    }

    [Fact]
    public void Create_WithInvalidEmailFormat_ShouldThrow()
    {
      // Arrange
      var invalidEmail = "invalid-email";

      // Act & Assert
      var ex = Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
      ex.Message.Should().Contain("Email format is invalid");
    }

    [Fact]
    public void Create_WithEmptyEmail_ShouldThrow()
    {
      // Act & Assert
      var ex = Assert.Throws<ArgumentException>(() => Email.Create(""));
      ex.Message.Should().Contain("Email cannot be empty");
    }

    [Fact]
    public void EmailEquality_WithSameValue_ShouldBeEqual()
    {
      // Arrange
      var email1 = Email.Create("test@example.com");
      var email2 = Email.Create("test@example.com");

      // Act & Assert
      email1.Should().Be(email2);
    }

    [Fact]
    public void EmailEquality_WithDifferentValues_ShouldNotBeEqual()
    {
      // Arrange
      var email1 = Email.Create("test1@example.com");
      var email2 = Email.Create("test2@example.com");

      // Act & Assert
      email1.Should().NotBe(email2);
    }
  }

  public class PasswordValueObjectTests
  {
    [Fact]
    public void Create_WithValidPassword_ShouldSucceed()
    {
      // Arrange
      var passwordString = "SecurePass123";

      // Act
      var password = Password.Create(passwordString);

      // Assert
      password.Should().NotBeNull();
      password.Value.Should().Be(passwordString);
    }

    [Fact]
    public void Create_WithWeakPassword_ShouldThrow()
    {
      // Arrange
      var weakPassword = "weak";

      // Act & Assert
      var ex = Assert.Throws<ArgumentException>(() => Password.Create(weakPassword));
      ex.Message.Should().Contain("Password must be at least 8 characters");
    }

    [Fact]
    public void Create_WithoutUppercase_ShouldThrow()
    {
      // Arrange
      var invalidPassword = "onlylowercase123";

      // Act & Assert
      var ex = Assert.Throws<ArgumentException>(() => Password.Create(invalidPassword));
      ex.Message.Should().NotBeEmpty();
    }

    [Fact]
    public void ToString_ShouldNotExposePassword()
    {
      // Arrange
      var password = Password.Create("SecurePass123");

      // Act
      var result = password.ToString();

      // Assert
      result.Should().Be("***");
    }
  }

  public class PhoneNumberValueObjectTests
  {
    [Fact]
    public void Create_WithValidPhoneNumber_ShouldSucceed()
    {
      // Arrange
      var phoneString = "+1234567890";

      // Act
      var phone = PhoneNumber.Create(phoneString);

      // Assert
      phone.Should().NotBeNull();
      phone!.Value.Should().Be(phoneString);
    }

    [Fact]
    public void Create_WithInvalidPhoneFormat_ShouldThrow()
    {
      // Arrange
      var invalidPhone = "invalid";

      // Act & Assert
      var ex = Assert.Throws<ArgumentException>(() => PhoneNumber.Create(invalidPhone));
      ex.Message.Should().Contain("Phone number format is invalid");
    }

    [Fact]
    public void Create_WithNull_ShouldReturnNull()
    {
      // Act
      var result = PhoneNumber.Create(null);

      // Assert
      result.Should().BeNull();
    }

    [Fact]
    public void Create_WithEmptyString_ShouldReturnNull()
    {
      // Act
      var result = PhoneNumber.Create("");

      // Assert
      result.Should().BeNull();
    }
  }
}
