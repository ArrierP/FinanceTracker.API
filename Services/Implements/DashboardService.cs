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

        var data = await query
            .Where(x => x.Date.Month == now.Month && x.Date.Year == now.Year)
            .ToListAsync();

        return new MonthlySummaryDto
        {
            TotalIncome = data
                .Where(x => x.Type == TransactionType.Income)
                .Sum(x => x.Amount),

            TotalExpense = data
                .Where(x => x.Type == TransactionType.Expense)
                .Sum(x => x.Amount)
        };
    }
}