using EcomSiteMVC.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Core.IServices
{
    public interface IPaymentService
    {
        Task<IActionResult> ProcessCodPayment(Order order);
        Task<IActionResult> ProcessKhaltiPayment(Order order);
        Task<IActionResult> VerifyKhaltiPayment(int orderId, string pidx);

        #region helpers
        (string orderId, string pidx) ExtractKhaltiPaymentDetails(string queryString);
        #endregion
    }
}
