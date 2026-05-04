namespace FinanceTracker.API.DTOs.Dashboard
{
    public class MonthlySummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal ActualTotalBalance { get; set; }

        // Thêm cái này để FE có cái mà vẽ
        public List<ChartItemDto> ChartData { get; set; } = new();
    }

    public class ChartItemDto
    {
        public required string Date { get; set; }=string.Empty; //Định dạng dd/MM
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
