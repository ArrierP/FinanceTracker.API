using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTracker.Api.Data;
using FinanceTracker.API.DTOs.Dashboard;
using FinanceTracker.API.Services.Interfaces;
using FinanceTracker.Api.Enums;

public class GetMonthlySummaryQuery : IRequest<MonthlySummaryDto>
{
    public int Month { get; set; }
    public int Year { get; set; }
}

public class GetMonthlySummaryHandler : IRequestHandler<GetMonthlySummaryQuery, MonthlySummaryDto>
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetMonthlySummaryHandler(ApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<MonthlySummaryDto> Handle(GetMonthlySummaryQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Transactions
            .Include(t => t.Wallet)
            .AsQueryable();

        if (!_currentUser.IsAdmin)
        {
            query = query.Where(t => t.Wallet!.UserId == _currentUser.UserId);
        }

        var data = await query
            .Where(t => t.TransactionDate.Month == request.Month &&
                        t.TransactionDate.Year == request.Year)
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