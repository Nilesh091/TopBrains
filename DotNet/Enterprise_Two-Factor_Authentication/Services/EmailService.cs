using System;
using System.Net;
using System.Net.Mail;

namespace Enterprise_Two_Factor_Authentication.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("pevgroupprojectnilesh@gmail.com", "kkwd qcuw gtnm zygd"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("pevgroupprojectnilesh@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
