using System.Threading.Tasks;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Web.Controllers
{
    [Authorize(Roles = "Superadmin,Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var monthlySalesChartData = await _dashboardService.GetMonthlySalesChartData();
            var monthlyRevenueChartData = await _dashboardService.GetMonthlyRevenueChartData();
            var topCategorySalesChartData = await _dashboardService.GetTopCategorySalesChartData();
            var totalNoOfSales = await _dashboardService.GetTotalNoOfSales();
            var totalNoOfOrders = await _dashboardService.GetTotalNoOfOrders();

            var viewModel = new DashboardViewModel
            {
                MonthlySalesChartData = monthlySalesChartData,
                MonthlyRevenueChartData = monthlyRevenueChartData,
                TopCategorySalesChartData = topCategorySalesChartData,
                TotalNoOfSales = totalNoOfSales,
                TotalNoOfOrders = totalNoOfOrders,
            };

            return View(viewModel);
        }
    }
}
