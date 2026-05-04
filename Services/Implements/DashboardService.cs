using FinanceTracker.Api.Data;
using FinanceTracker.Api.Enums;
using FinanceTracker.API.DTOs.Dashboard;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FinanceTracker.API.Services.Implements
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public DashboardService(ApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        // 1. Lấy tổng quan (Thẻ thông tin chính) - Giải quyết lỗi "Không biết hiện tại có bao nhiêu tiền"
        public async Task<DashboardOverviewDto> GetOverviewAsync()
        {
            var userId = _currentUser.UserId;

            // Tính tổng số dư thực tế từ tất cả các ví của User
            var actualBalance = await _context.Wallets
                .Where(x => x.UserId == userId)
                .SumAsync(x => x.Balance);

            // Tổng thu/chi trong tháng hiện tại để hiển thị so sánh nhanh
            var now = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            var monthlyStats = await _context.Transactions
                .Where(x => x.UserId == userId && x.Date >= firstDayOfMonth)
                .ToListAsync();

            return new DashboardOverviewDto
            {
                ActualTotalBalance = actualBalance, // Số dư thực thực tế
                TotalIncome = monthlyStats.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                TotalExpense = monthlyStats.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
            };
        }

        // 2. Thống kê biểu đồ cột (7 ngày hoặc 30 ngày)
        public async Task<MonthlySummaryDto> GetMonthlySummaryAsync(int days = 7)
        {
            var userId = _currentUser.UserId;
            var startDate = DateTime.UtcNow.AddDays(-days);

            var transactions = await _context.Transactions
                .Where(x => x.UserId == userId && x.Date >= startDate)
                .ToListAsync();

            var chartItems = transactions
                .GroupBy(x => x.Date.Date)
                .Select(g => new ChartItemDto
                {
                    Date = g.Key.ToString("dd/MM"), // Fix lỗi ép kiểu DateTime sang string
                    Income = g.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount),
                    Expense = g.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new MonthlySummaryDto
            {
                TotalIncome = chartItems.Sum(x => x.Income),
                TotalExpense = chartItems.Sum(x => x.Expense),
                ActualTotalBalance = await _context.Wallets.Where(w => w.UserId == userId).SumAsync(w => w.Balance),
                ChartData = chartItems
            };
        }

        // 3. Thống kê theo danh mục (Biểu đồ tròn) - Để biết tiền chi vào đâu nhiều nhất
        public async Task<List<CategorySummaryDto>> GetCategorySummaryAsync()
        {
            var userId = _currentUser.UserId;

            return await _context.Transactions
                .Where(x => x.UserId == userId && x.Type == TransactionType.Expense)
                .GroupBy(x => x.Category.Name)
                .Select(g => new CategorySummaryDto
                {
                    CategoryName = g.Key,
                    Amount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.Amount)
                .ToListAsync();
        }

        // 4. Tóm tắt danh sách ví - Xem chi tiết từng nguồn tiền
        public async Task<List<WalletSummaryDto>> GetWalletSummaryAsync()
        {
            return await _context.Wallets
                .Where(x => x.UserId == _currentUser.UserId)
                .Select(w => new WalletSummaryDto
                {
                    WalletName = w.Name,
                    Balance = w.Balance,
                    Currency = "VND" // Hoặc lấy từ thuộc tính của ví nếu có
                })
                .ToListAsync();
        }

        // 5. Xu hướng chi tiêu theo tháng (Nếu FE cần vẽ biểu đồ đường dài hạn)
        public async Task<List<MonthlyTrendDto>> GetMonthlyTrendAsync(int months = 6)
        {
            var userId = _currentUser.UserId;
            var startDate = DateTime.UtcNow.AddMonths(-months);

            var data = await _context.Transactions
                .Where(x => x.UserId == userId && x.Date >= startDate)
                .ToListAsync();

            return data
                .GroupBy(x => new { x.Date.Year, x.Date.Month })
                .Select(g => new MonthlyTrendDto
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Income = g.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount),
                    Expense = g.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount)
                })
                .ToList();
        }
    }
}