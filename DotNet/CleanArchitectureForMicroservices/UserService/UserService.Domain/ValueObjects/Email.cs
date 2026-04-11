using System;

namespace UserService.Domain.ValueObjects
{
  public class Email : IEquatable<Email>
  {
    public string Value { get; }

    private Email(string value)
    {
      Value = value;
    }

    public static Email Create(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentException("Email cannot be empty", nameof(email));

      var trimmedEmail = email.Trim();

      if (!IsValidEmail(trimmedEmail))
        throw new ArgumentException("Email format is invalid", nameof(email));

      return new Email(trimmedEmail);
    }

    private static bool IsValidEmail(string email)
    {
      try
      {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
      }
      catch
      {
        return false;
      }
    }

    public override bool Equals(object? obj)
    {
      return Equals(obj as Email);
    }

    public bool Equals(Email? other)
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

    public static bool operator ==(Email? left, Email? right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Email? left, Email? right)
    {
      return !Equals(left, right);
    }
  }
}
