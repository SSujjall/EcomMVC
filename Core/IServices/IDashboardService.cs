using EcomSiteMVC.Core.DTOs;

namespace EcomSiteMVC.Core.IServices
{
    public interface IDashboardService
    {
        Task<object> GetMonthlySalesChartData();
        Task<object> GetMonthlyRevenueChartData();
        Task<object> GetTopCategorySalesChartData();
        Task<int> GetTotalNoOfSales();
        Task<int> GetTotalNoOfOrders();
    }
}
