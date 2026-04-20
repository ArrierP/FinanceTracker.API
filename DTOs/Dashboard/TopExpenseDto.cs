namespace FinanceTracker.API.DTOs.Dashboard;

public class TopExpenseDto
{
    public decimal Amount { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}