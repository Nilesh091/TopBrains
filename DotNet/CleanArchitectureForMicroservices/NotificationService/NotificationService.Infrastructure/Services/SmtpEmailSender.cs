using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using NotificationService.Domain.Entity;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public SmtpEmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(EmailMessage message)
    {
        var smtp = _config.GetSection("SmtpSettings");

        var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"]!))
        {
            Credentials = new NetworkCredential(
                smtp["Username"],
                smtp["Password"]),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(smtp["From"]),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = true
        };

        mail.To.Add(message.To);

        await client.SendMailAsync(mail);
    }
}