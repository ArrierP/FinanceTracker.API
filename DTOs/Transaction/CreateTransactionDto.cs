using FinanceTracker.Api.Enums;

namespace FinanceTracker.API.DTOs.Transaction;

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionType Type { get; set; }
    public int WalletId { get; set; }
    public int CategoryId { get; set; }
}