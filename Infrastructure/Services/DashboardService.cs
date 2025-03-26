using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Infrastructure.Repositories;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<object> GetMonthlySalesChartData()
        {
            var monthlySales = await _repo.GetMonthlySales();
            return new
            {
                labels = monthlySales.Select(ms => ms.Month).ToList(),
                datasets = new[]
                {
                    new
                    {
                        label = "Monthly Sales",
                        data = monthlySales.Select(ms => ms.TotalQuantity).ToList(),
                        backgroundColor = "rgba(54, 162, 235, 0.2)",
                        borderColor = "rgba(54, 162, 235, 1)",
                        borderWidth = 1
                    }
                }
            };
        }

        public async Task<object> GetMonthlyRevenueChartData()
        {
            var monthlySales = await _repo.GetMonthlySales();
            return new
            {
                labels = monthlySales.Select(ms => ms.Month).ToList(),
                datasets = new[]
                {
                    new
                    {
                        label = "Monthly Revenue",
                        data = monthlySales.Select(ms => ms.TotalRevenue).ToList(),
                        backgroundColor = "rgba(255, 99, 132, 0.2)",
                        borderColor = "rgba(255, 99, 132, 1)",
                        borderWidth = 1
                    }
                }
            };
        }

        public async Task<object> GetTopCategorySalesChartData()
        {
            var topCategories = await _repo.GetTopCategorySales();
            return new
            {
                labels = topCategories.Select(tc => tc.CategoryName).ToList(),
                datasets = new[]
                {
                    new
                    {
                        label = "Top Categories Sales",
                        data = topCategories.Select(tc => tc.SalesAmount).ToList(),
                        backgroundColor = new string[] { "rgba(255, 99, 132, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(75, 192, 192, 0.2)" },
                        borderColor = new string[] { "rgba(255, 99, 132, 1)", "rgba(54, 162, 235, 1)", "rgba(75, 192, 192, 1)" },
                        borderWidth = 1
                    }
                }
            };
        }

        public async Task<int> GetTotalNoOfSales()
        {
            return await _repo.GetTotalNoOfSales();
        }

        public async Task<int> GetTotalNoOfOrders()
        {
            return await _repo.GetTotalNoOfOrders();
        }
    }
}
