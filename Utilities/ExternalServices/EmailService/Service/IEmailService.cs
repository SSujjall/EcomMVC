using EcomSiteMVC.Extensions.EmailService.Model;

namespace EcomSiteMVC.Extensions.EmailService.Service
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage message);
    }
}