namespace FinanceTracker.API.DTOs.Dashboard
{
    public class MonthlySummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalBalance => TotalIncome - TotalExpense;
        // Thêm cái này để FE có cái mà vẽ
        public List<ChartItemDto> ChartData { get; set; } = new();
    }

    public class ChartItemDto
    {
        public string Date { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
