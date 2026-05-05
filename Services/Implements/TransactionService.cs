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

    public async Task<IEnumerable<TransactionDto>> GetAllAsync(int? walletId = null, DateTime? fromDate = null, DateTime? toDate = null, TransactionType? type = null)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var query = _context.Transactions
            .Include(t => t.Category)
            .Include(t => t.Wallet)
            .Where(t => t.UserId == userId)
            .AsQueryable();

        // Logic Lọc (Filter) - Giữ nguyên
        if (walletId.HasValue)
            query = query.Where(t => t.WalletId == walletId.Value);

        if (fromDate.HasValue)
            query = query.Where(t => t.TransactionDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(t => t.TransactionDate <= toDate.Value);

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        
        return await query
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                TransactionDate = t.TransactionDate,
                Type = t.Type.ToString(),
                CategoryName = t.Category != null ? t.Category.Name : "N/A",
                WalletName = t.Wallet != null ? t.Wallet.Name : "N/A",
                CreatedAt = t.CreatedAt,
                ToWalletId = t.ToWalletId
            })
            .ToListAsync();
    }

    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        // 1. Truy vấn dữ liệu từ Database
        var transaction = await _context.Transactions
            .Include(t => t.Category)
            .Include(t => t.Wallet)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        // 2. Nếu không tìm thấy, trả về null
        if (transaction == null) return null;

        // 3. Ánh xạ (Mapping) dữ liệu sang TransactionDto để trả về
        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            Type = transaction.Type.ToString(),
            CategoryName = transaction.Category?.Name ?? "N/A",
            WalletName = transaction.Wallet?.Name ?? "N/A",
            CreatedAt = transaction.CreatedAt,
            ToWalletId = transaction.ToWalletId
        };
    }

    // SỬA Ở ĐÂY: Đổi Task<Transaction?> thành Task<TransactionDto?>
    public async Task<TransactionDto?> CreateAsync(CreateTransactionDto dto)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        using var dbTransaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == dto.WalletId && w.UserId == userId);
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.UserId == userId);

            if (wallet == null) return null;

            if (dto.Type != TransactionType.Transfer && category == null)
                return null;

            var transaction = new Transaction
            {
                Amount = dto.Amount,
                Description = dto.Description,
                TransactionDate = dto.TransactionDate,
                Date = dto.TransactionDate, // Có thể bỏ nếu Database chỉ dùng TransactionDate
                Type = dto.Type,
                WalletId = dto.WalletId,
                CategoryId = dto.CategoryId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            // --- XỬ LÝ CẬP NHẬT SỐ DƯ (Đã thêm kiểm tra số âm) ---
            if (dto.Type == TransactionType.Income)
            {
                wallet.Balance += dto.Amount;
            }
            else if (dto.Type == TransactionType.Expense)
            {
                // Kiểm tra ví có đủ tiền để chi tiêu không
                if (wallet.Balance < dto.Amount)
                    throw new Exception("Số dư không đủ");

                wallet.Balance -= dto.Amount;
            }
            else if (dto.Type == TransactionType.Transfer)
            {
                if (dto.ToWalletId == null) throw new Exception("Cần ví đích để chuyển khoản.");

                var toWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == dto.ToWalletId && w.UserId == userId);
                if (toWallet == null) throw new Exception("Ví đích không tồn tại.");

                // Kiểm tra ví gửi (Ví A) có đủ tiền để chuyển không
                if (wallet.Balance < dto.Amount)
                    throw new Exception("Số dư không đủ");

                // Trừ tiền ví gửi, cộng tiền ví nhận
                wallet.Balance -= dto.Amount;
                toWallet.Balance += dto.Amount;

                transaction.Description = $"[Chuyển tiền đến {toWallet.Name}] " + dto.Description;
                transaction.ToWalletId = dto.ToWalletId;
            }

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            await dbTransaction.CommitAsync();

            return new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                Type = transaction.Type.ToString(),
                WalletName = wallet.Name,
                CategoryName = dto.Type == TransactionType.Transfer ? null : category!.Name,
                CreatedAt = transaction.CreatedAt
            };
        }
        catch (Exception)
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateTransactionDto dto)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        // 1. Lấy thực thể GỐC từ Database (không dùng GetByIdAsync vì hàm đó trả về DTO)
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (transaction == null) return false;

        // 2. Kiểm tra quyền sở hữu nếu đổi Ví
        if (transaction.WalletId != dto.WalletId)
        {
            var isWalletValid = await _context.Wallets.AnyAsync(w => w.Id == dto.WalletId && w.UserId == userId);
            if (!isWalletValid) return false;

            // Lưu ý: Nếu bạn muốn cập nhật luôn số dư khi đổi ví, bạn cần thêm logic cộng/trừ Balance ở đây
        }

        // 3. Kiểm tra quyền sở hữu nếu đổi Danh mục
        if (transaction.CategoryId != dto.CategoryId)
        {
            var isCategoryValid = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId);
            if (!isCategoryValid) return false;
        }

        // 4. Cập nhật các giá trị
        transaction.Amount = dto.Amount;
        transaction.Description = dto.Description;
        transaction.TransactionDate = dto.TransactionDate;
        transaction.Type = dto.Type;
        transaction.WalletId = dto.WalletId;
        transaction.CategoryId = dto.CategoryId;
        transaction.LastModifiedAt = DateTime.UtcNow;

        // 5. Lưu xuống DB
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Wallet)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == _currentUserService.UserId);

        if (transaction == null) return false;

        // Phân loại logic hoàn tác số dư cho từng loại giao dịch
        if (transaction.Type == TransactionType.Income)
        {
            if (transaction.Wallet != null)
            {
                // Kiểm tra tránh số dư âm khi xóa khoản thu
                if (transaction.Wallet.Balance < transaction.Amount)
                    throw new Exception("Not enough balance in the destination wallet");

                transaction.Wallet.Balance -= transaction.Amount;
            }
        }
        else if (transaction.Type == TransactionType.Expense)
        {
            if (transaction.Wallet != null)
            {
                transaction.Wallet.Balance += transaction.Amount;
            }
        }
        else if (transaction.Type == TransactionType.Transfer)
        {
            // 1. Hoàn lại số tiền đã trừ cho Ví gửi (Ví A)
            if (transaction.Wallet != null)
            {
                transaction.Wallet.Balance += transaction.Amount;
            }

            // 2. Trừ lại số tiền đã nhận của Ví đích (Ví B)
            if (transaction.ToWalletId.HasValue)
            {
                var toWallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.Id == transaction.ToWalletId.Value && w.UserId == _currentUserService.UserId);

                if (toWallet != null)
                {
                    // Kiểm tra tránh số dư âm cho ví B khi hoàn tác chuyển tiền
                    if (toWallet.Balance < transaction.Amount)
                        throw new Exception("Not enough balance in the destination wallet");

                    toWallet.Balance -= transaction.Amount;
                }
            }
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }
}