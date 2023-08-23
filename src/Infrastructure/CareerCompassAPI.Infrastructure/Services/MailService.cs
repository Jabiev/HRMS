using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Domain.Concretes;
using CareerCompassAPI.Domain.Identity;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;

namespace CareerCompassAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly UserManager<AppUser> _userManager;

        public MailService(EmailConfiguration emailConfig,
                           UserManager<AppUser> userManager)
        {
            _emailConfig = emailConfig;
            _userManager = userManager;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message.Content; 

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }
        private async Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

    }
}
