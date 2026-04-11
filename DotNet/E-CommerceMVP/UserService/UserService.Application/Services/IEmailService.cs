using System;

namespace UserService.Application.Services
{
  public interface IEmailService
  {
    Task<bool> SendEmailConfirmationAsync(string email, string token, Guid userId);
    Task<bool> SendPasswordResetAsync(string email, string token, Guid userId);
    Task<bool> SendWelcomeEmailAsync(string email, string userName);
    Task<bool> SendPasswordChangedNotificationAsync(string email, string userName);
  }
}
