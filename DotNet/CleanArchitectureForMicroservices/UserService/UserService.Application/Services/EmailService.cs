using System;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UserService.Application.Services
{
  public class EmailService : IEmailService
  {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(HttpClient httpClient, IConfiguration configuration, ILogger<EmailService> logger)
    {
      _httpClient = httpClient;
      _configuration = configuration;
      _logger = logger;
    }

    public async Task<bool> SendEmailConfirmationAsync(string email, string token, Guid userId)
    {
      try
      {
        var confirmationLink = $"{_configuration["AppSettings:AppUrl"]}/confirm-email?userId={userId}&token={Uri.EscapeDataString(token)}";

        var emailRequest = new
        {
          to = email,
          subject = "Confirm Your Email Address",
          body = $"Please confirm your email by clicking the link: {confirmationLink}",
          templateId = "email_confirmation"
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_configuration["Services:NotificationService"]}/api/v1/email/send",
            emailRequest);

        if (!response.IsSuccessStatusCode)
        {
          _logger.LogError($"Failed to send confirmation email to {email}: {response.StatusCode}");
          return false;
        }

        _logger.LogInformation($"Confirmation email sent successfully to {email}");
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error sending confirmation email to {email}");
        return false;
      }
    }

    public async Task<bool> SendPasswordResetAsync(string email, string token, Guid userId)
    {
      try
      {
        var resetLink = $"{_configuration["AppSettings:AppUrl"]}/reset-password?userId={userId}&token={Uri.EscapeDataString(token)}";

        var emailRequest = new
        {
          to = email,
          subject = "Reset Your Password",
          body = $"Click the link to reset your password: {resetLink}. This link expires in 24 hours.",
          templateId = "password_reset"
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_configuration["Services:NotificationService"]}/api/v1/email/send",
            emailRequest);

        if (!response.IsSuccessStatusCode)
        {
          _logger.LogError($"Failed to send password reset email to {email}: {response.StatusCode}");
          return false;
        }

        _logger.LogInformation($"Password reset email sent successfully to {email}");
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error sending password reset email to {email}");
        return false;
      }
    }

    public async Task<bool> SendWelcomeEmailAsync(string email, string userName)
    {
      try
      {
        var emailRequest = new
        {
          to = email,
          subject = "Welcome to Our Platform",
          body = $"Welcome {userName}! Your account has been created successfully.",
          templateId = "welcome"
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_configuration["Services:NotificationService"]}/api/v1/email/send",
            emailRequest);

        if (!response.IsSuccessStatusCode)
        {
          _logger.LogError($"Failed to send welcome email to {email}: {response.StatusCode}");
          return false;
        }

        _logger.LogInformation($"Welcome email sent successfully to {email}");
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error sending welcome email to {email}");
        return false;
      }
    }

    public async Task<bool> SendPasswordChangedNotificationAsync(string email, string userName)
    {
      try
      {
        var emailRequest = new
        {
          to = email,
          subject = "Password Changed Successfully",
          body = $"Hi {userName}, your password has been changed successfully. If you didn't make this change, please reset your password immediately.",
          templateId = "password_changed_notification"
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_configuration["Services:NotificationService"]}/api/v1/email/send",
            emailRequest);

        if (!response.IsSuccessStatusCode)
        {
          _logger.LogError($"Failed to send password changed notification to {email}: {response.StatusCode}");
          return false;
        }

        _logger.LogInformation($"Password changed notification sent successfully to {email}");
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error sending password changed notification to {email}");
        return false;
      }
    }

    public async Task<bool> SendOtpAsync(string email, string otpCode)
    {
      try
      {
        var emailRequest = new
        {
          to = email,
          subject = "Your 2FA One-Time Password (OTP)",
          body = $"Your OTP for Two-Factor Authentication is: {otpCode}\n\nThis code will expire in 10 minutes.\n\nDo not share this code with anyone.",
          templateId = "otp_2fa"
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_configuration["Services:NotificationService"]}/api/v1/email/send",
            emailRequest);

        if (!response.IsSuccessStatusCode)
        {
          _logger.LogError($"Failed to send OTP email to {email}: {response.StatusCode}");
          return false;
        }

        _logger.LogInformation($"OTP email sent successfully to {email}");
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error sending OTP email to {email}");
        return false;
      }
    }
  }
}
