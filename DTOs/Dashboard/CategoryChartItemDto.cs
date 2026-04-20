namespace FinanceTracker.API.DTOs.Dashboard;

public class CategoryChartItemDto
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal Total { get; set; }
}