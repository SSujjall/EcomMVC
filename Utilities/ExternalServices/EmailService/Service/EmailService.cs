using EcomSiteMVC.Extensions.EmailService.Config;
using EcomSiteMVC.Extensions.EmailService.Model;
using MimeKit;

namespace EcomSiteMVC.Extensions.EmailService.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        public EmailService(EmailConfig emailConfig) => _emailConfig = emailConfig;

        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email,", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.Username, _emailConfig.Password);
                client.Send(mailMessage);
            }
            catch
            {
                throw new InvalidOperationException();
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
