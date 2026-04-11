using System;
using System.Text.RegularExpressions;

namespace UserService.Domain.ValueObjects
{
  public class PhoneNumber : IEquatable<PhoneNumber>
  {
    public string Value { get; }

    private PhoneNumber(string value)
    {
      Value = value;
    }

    public static PhoneNumber? Create(string? phoneNumber)
    {
      if (string.IsNullOrWhiteSpace(phoneNumber))
        return null;

      var trimmed = phoneNumber.Trim();

      if (!IsValidPhoneNumber(trimmed))
        throw new ArgumentException("Phone number format is invalid", nameof(phoneNumber));

      return new PhoneNumber(trimmed);
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
      // E.164 international format validation
      return Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$");
    }

    public override bool Equals(object? obj)
    {
      return Equals(obj as PhoneNumber);
    }

    public bool Equals(PhoneNumber? other)
    {
      return other is not null && Value == other.Value;
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public override string ToString()
    {
      return Value;
    }

    public static bool operator ==(PhoneNumber? left, PhoneNumber? right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(PhoneNumber? left, PhoneNumber? right)
    {
      return !Equals(left, right);
    }
  }
}
