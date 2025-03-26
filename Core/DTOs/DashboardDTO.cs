namespace EcomSiteMVC.Core.DTOs
{
    public class DashboardViewModel
    {
        public dynamic MonthlySalesChartData { get; set; }
        public dynamic MonthlyRevenueChartData { get; set; }
        public dynamic TopCategorySalesChartData { get; set; }

        // For Cards
        public int TotalNoOfSales { get; set; }
        public int TotalNoOfOrders { get; set; }
    }

    public class MonthlySalesDTO
    {
        public string Month { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class MonthlyRevenueDTO
    {
    }
    
    public class TopCategorySalesDTO
    {
        public string CategoryName { get; set; }
        public decimal SalesAmount { get; set; }
    }
}
