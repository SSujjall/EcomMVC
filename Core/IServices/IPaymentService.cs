using EcomSiteMVC.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Core.IServices
{
    public interface IPaymentService
    {
        Task<IActionResult> ProcessKhaltiPayment(Order order);
        Task<IActionResult> ProcessCodPayment(Order order);
        Task<IActionResult> VerifyKhaltiPayment(int orderId, string pidx);
        (string orderId, string pidx) ExtractPaymentDetails(string queryString);
    }
}
