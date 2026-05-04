namespace FinanceTracker.API.DTOs.Dashboard
{
    public class WalletSummaryDto
    {
        public string WalletName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "VND";
    }
}
// Lưu ý: Đây là DTO đơn giản để trả về dữ liệu tổng quan về từng ví của người dùng.