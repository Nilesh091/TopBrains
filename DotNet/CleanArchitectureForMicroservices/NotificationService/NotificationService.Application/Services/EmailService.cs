using System;
using NotificationService.Application.DTOs;
using NotificationService.Domain.Entity;
namespace NotificationService.Application.Services
{
    public class EmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendEmailAsync(SendEmailRequest request)
        {
            var email = new EmailMessage
            {
                To = request.To,
                Subject = request.Subject,
                Body = request.Body,
                TemplateId = request.TemplateId
            };

            await _emailSender.SendAsync(email);
        }
    }
}
