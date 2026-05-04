namespace FinanceTracker.API.DTOs.Dashboard
{
    public class DashboardOverviewDto
    {
        public decimal ActualTotalBalance { get; set; } // Số dư thực tế trong ví
        public decimal TotalIncome { get; set; }        // Tổng thu tháng này
        public decimal TotalExpense { get; set; }       // Tổng chi tháng này
    }
}
