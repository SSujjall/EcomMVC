using EcomSiteMVC.Extensions.KhaltiPaymentService.Model;

namespace EcomSiteMVC.Extensions.KhaltiPaymentService.Service
{
    public interface IKhaltiService
    {
        Task<PaymentInitiateResponse> InitiatePayment(KhaltiRequestModel requestModel);
    }
}
