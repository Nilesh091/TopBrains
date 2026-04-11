using System;
using System.Text.RegularExpressions;

namespace UserService.Domain.ValueObjects
{
  public class Password : IEquatable<Password>
  {
    public string Value { get; }

    private Password(string value)
    {
      Value = value;
    }

    public static Password Create(string password)
    {
      if (string.IsNullOrWhiteSpace(password))
        throw new ArgumentException("Password cannot be empty", nameof(password));

      if (!IsValidPassword(password))
        throw new ArgumentException(
            "Password must be at least 8 characters and contain uppercase, lowercase, digit, and special character",
            nameof(password));

      return new Password(password);
    }

    private static bool IsValidPassword(string password)
    {
      if (password.Length < 8)
        return false;

      var hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
      var hasLowerCase = Regex.IsMatch(password, @"[a-z]");
      var hasDigit = Regex.IsMatch(password, @"\d");

      return hasUpperCase && hasLowerCase && hasDigit;
    }

    public override bool Equals(object? obj)
    {
      return Equals(obj as Password);
    }

    public bool Equals(Password? other)
    {
      return other is not null && Value == other.Value;
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public override string ToString()
    {
      return "***"; // Never expose actual password
    }

    public static bool operator ==(Password? left, Password? right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Password? left, Password? right)
    {
      return !Equals(left, right);
    }
  }
}
