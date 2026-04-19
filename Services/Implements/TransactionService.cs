using FinanceTracker.Api.Data;
using FinanceTracker.Api.Enums;
using FinanceTracker.API.DTOs.Transaction;
using FinanceTracker.API.Entities;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Implements;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public TransactionService(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync(int? walletId = null, DateTime? fromDate = null, DateTime? toDate = null, TransactionType? type = null)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        // Chỉ lấy các giao dịch thuộc về các Ví (Wallet) do user hiện tại sở hữu
        var query = _context.Transactions
            .Include(t => t.Category) // Kéo theo dữ liệu Category để hiển thị tên
            .Include(t => t.Wallet)   // Kéo theo dữ liệu Wallet để hiển thị tên
            .Where(t => t.Wallet!.UserId == userId)
            .AsQueryable();

        // Logic Lọc (Filter)
        if (walletId.HasValue)
            query = query.Where(t => t.WalletId == walletId.Value);

        if (fromDate.HasValue)
            query = query.Where(t => t.TransactionDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(t => t.TransactionDate <= toDate.Value);

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        // Sắp xếp mới nhất lên đầu
        return await query.OrderByDescending(t => t.TransactionDate).ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        return await _context.Transactions
            .Include(t => t.Category)
            .Include(t => t.Wallet)
            .FirstOrDefaultAsync(t => t.Id == id && t.Wallet!.UserId == userId);
    }

    public async Task<Transaction?> CreateAsync(CreateTransactionDto dto)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        // 1. Kiểm tra Ví (Wallet) có tồn tại và có thuộc về User này không?
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == dto.WalletId && w.UserId == userId);
        if (wallet == null) return null; // Không hợp lệ

        // 2. Kiểm tra Danh mục (Category) có tồn tại và có thuộc về User này không?
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.UserId == userId);
        if (category == null) return null; // Không hợp lệ

        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Description = dto.Description,
            TransactionDate = dto.TransactionDate,
            Type = dto.Type,
            WalletId = dto.WalletId,
            CategoryId = dto.CategoryId
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<bool> UpdateAsync(int id, UpdateTransactionDto dto)
    {
        var transaction = await GetByIdAsync(id);
        if (transaction == null) return false;

        // Nếu người dùng muốn đổi giao dịch sang Ví khác hoặc Danh mục khác, phải kiểm tra lại quyền sở hữu
        if (transaction.WalletId != dto.WalletId)
        {
            var isWalletValid = await _context.Wallets.AnyAsync(w => w.Id == dto.WalletId && w.UserId == _currentUserService.UserId);
            if (!isWalletValid) return false;
        }

        if (transaction.CategoryId != dto.CategoryId)
        {
            var isCategoryValid = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == _currentUserService.UserId);
            if (!isCategoryValid) return false;
        }

        transaction.Amount = dto.Amount;
        transaction.Description = dto.Description;
        transaction.TransactionDate = dto.TransactionDate;
        transaction.Type = dto.Type;
        transaction.WalletId = dto.WalletId;
        transaction.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var transaction = await GetByIdAsync(id);
        if (transaction == null) return false;

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }
}