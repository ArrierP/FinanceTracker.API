using FinanceTracker.Api.Enums;

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionType Type { get; set; }
    public int WalletId { get; set; } // Ví nguồn (Gửi tiền đi)
    public int? ToWalletId { get; set; } // Ví đích (Nhận tiền - chỉ dùng cho Transfer)
    public int CategoryId { get; set; }
}