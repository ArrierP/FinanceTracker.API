namespace FinanceTracker.API.DTOs.Transaction
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public DateTime TransactionDate { get; set; }

        // Trả về dạng chuỗi (Ví dụ: "Income", "Expense") để dễ hiển thị
        public string Type { get; set; } = string.Empty;

        // Trả về tên hiển thị thay vì chỉ ID
        public string CategoryName { get; set; } = string.Empty;

        public string WalletName { get; set; } = string.Empty;

        // Dành cho trường hợp chuyển khoản (nếu có)
        public int? ToWalletId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}