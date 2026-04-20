using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTracker.Api.Data;
using FinanceTracker.API.DTOs.Dashboard;
using FinanceTracker.API.Services.Interfaces;
using FinanceTracker.Api.Enums;

public class GetTopExpensesQuery : IRequest<List<TopExpenseDto>> { }

public class GetTopExpensesHandler : IRequestHandler<GetTopExpensesQuery, List<TopExpenseDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetTopExpensesHandler(ApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<TopExpenseDto>> Handle(GetTopExpensesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .Include(t => t.Wallet)
            .Where(t => t.Type == TransactionType.Expense);

        if (!_currentUser.IsAdmin)
        {
            query = query.Where(t => t.Wallet!.UserId == _currentUser.UserId);
        }

        return await query
            .OrderByDescending(t => t.Amount)
            .Take(5)
            .Select(t => new TopExpenseDto
            {
                Amount = t.Amount,
                CategoryName = t.Category!.Name,
                Date = t.TransactionDate
            })
            .ToListAsync();
    }
}