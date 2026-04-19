using FinanceTracker.Api.Enums;
using FinanceTracker.API.DTOs.Transaction;
using FinanceTracker.API.Entities;

namespace FinanceTracker.API.Services.Interfaces;

public interface ITransactionService
{
    // Lấy danh sách có hỗ trợ lọc (theo ví, ngày tháng, loại giao dịch)
    Task<IEnumerable<Transaction>> GetAllAsync(int? walletId = null, DateTime? fromDate = null, DateTime? toDate = null, TransactionType? type = null);

    Task<Transaction?> GetByIdAsync(int id);

    Task<Transaction?> CreateAsync(CreateTransactionDto dto);

    Task<bool> UpdateAsync(int id, UpdateTransactionDto dto);

    Task<bool> DeleteAsync(int id);
}