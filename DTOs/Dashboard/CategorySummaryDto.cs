namespace FinanceTracker.API.DTOs.Dashboard
{
    public class CategorySummaryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}

// Lưu ý: Đây là DTO đơn giản để trả về dữ liệu thống kê theo danh mục.
// FE sẽ sử dụng dữ liệu này để vẽ biểu đồ tròn..