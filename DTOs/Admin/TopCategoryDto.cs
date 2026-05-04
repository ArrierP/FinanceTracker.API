namespace FinanceTracker.API.DTOs.Admin
{
    public class TopCategoryDto
    {
        // Tên của danh mục (ví dụ: Ăn uống, Di chuyển)
        public string CategoryName { get; set; } = string.Empty;

        // Số lần danh mục này được người dùng sử dụng trong các giao dịch
        public int UsageCount { get; set; }

        // (Tùy chọn) Bạn có thể thêm Icon để Frontend hiển thị đẹp hơn
        public string Icon { get; set; } = string.Empty;

        // (Tùy chọn) Thêm loại để phân biệt Top Thu hay Top Chi
        public string Type { get; set; } = string.Empty;
    }
}
