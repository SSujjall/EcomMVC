using EcomSiteMVC.Core.DTOs;

namespace EcomSiteMVC.Core.IRepositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalNoOfSales();
        Task<int> GetTotalNoOfOrders();
        Task<IEnumerable<MonthlySalesDTO>> GetMonthlySales();
        Task<IEnumerable<TopCategorySalesDTO>> GetTopCategorySales();
    }
}
