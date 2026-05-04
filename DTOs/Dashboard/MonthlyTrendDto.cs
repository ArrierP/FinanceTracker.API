namespace FinanceTracker.API.DTOs.Dashboard
{
    public class MonthlyTrendDto
    {
        public string Month { get; set; } = string.Empty; // Ví dụ: "Tháng 1", "Tháng 2", ...
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
// Lưu ý: Đây là DTO đơn giản để trả về dữ liệu xu hướng thu chi theo tháng trong năm hiện tại.