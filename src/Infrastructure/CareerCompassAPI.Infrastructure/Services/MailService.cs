using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Domain.Concretes;
using MimeKit;
using MailKit.Net.Smtp;


namespace CareerCompassAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly EmailConfiguration _emailConfig;

        public MailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var htmlContent = "Hello";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlContent;

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

    }
}
