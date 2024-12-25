using EcomSiteMVC.EmailService.Model;

namespace EcomSiteMVC.EmailService.Service
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage message);
    }
}