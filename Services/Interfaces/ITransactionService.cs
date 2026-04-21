using FinanceTracker.Api.Enums;
using FinanceTracker.API.DTOs.Transaction;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllAsync(int? walletId = null, DateTime? fromDate = null, DateTime? toDate = null, TransactionType? type = null);
    Task<TransactionDto?> GetByIdAsync(int id);
    Task<TransactionDto?> CreateAsync(CreateTransactionDto dto); // Chỗ này quan trọng
    Task<bool> UpdateAsync(int id, UpdateTransactionDto dto);
    Task<bool> DeleteAsync(int id);
}