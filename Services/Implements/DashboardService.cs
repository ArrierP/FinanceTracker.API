using FinanceTracker.Api.Data;
using FinanceTracker.Api.Enums;
using FinanceTracker.API.DTOs.Dashboard;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DashboardService(ApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<MonthlySummaryDto> GetMonthlySummaryAsync()
    {
        var userId = _currentUser.UserId;
        var isAdmin = _currentUser.IsAdmin;

        var now = DateTime.UtcNow;

        var query = _context.Transactions.AsQueryable();

        // 🔥 Quan trọng: phân quyền
        if (!isAdmin)
        {
            query = query.Where(x => x.UserId == userId);
        }

        var startDate = DateTime.UtcNow.AddDays(-7);
        var data = await query
            .Where(x => x.Date >= startDate)
            .ToListAsync();

        var chartItems = data
            .GroupBy(x => x.Date.ToString("dd/MM")) // Nhóm theo ngày/tháng
            .Select(g => new ChartItemDto
            {
                Date = g.Key,
                Income = g.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount),
                Expense = g.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount)
            })
            .OrderBy(x => x.Date)
            .ToList();

        return new MonthlySummaryDto
        {
            TotalIncome = chartItems.Sum(x => x.Income),
            TotalExpense = chartItems.Sum(x => x.Expense),
            ChartData = chartItems // Gửi mảng này về
        };
    }
}