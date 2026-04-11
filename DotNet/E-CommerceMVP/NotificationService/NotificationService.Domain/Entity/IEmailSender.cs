using System;

namespace NotificationService.Domain.Entity
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}
