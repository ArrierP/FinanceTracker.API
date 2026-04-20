using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTracker.Api.Data;
using FinanceTracker.API.DTOs.Dashboard;
using FinanceTracker.API.Services.Interfaces;

public class GetCategoryChartDataQuery : IRequest<List<CategoryChartItemDto>> { }

public class GetCategoryChartDataHandler : IRequestHandler<GetCategoryChartDataQuery, List<CategoryChartItemDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetCategoryChartDataHandler(ApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<CategoryChartItemDto>> Handle(GetCategoryChartDataQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .Include(t => t.Wallet)
            .AsQueryable();

        if (!_currentUser.IsAdmin)
        {
            query = query.Where(t => t.Wallet!.UserId == _currentUser.UserId);
        }

        return await query
            .GroupBy(t => t.Category!.Name)
            .Select(g => new CategoryChartItemDto
            {
                CategoryName = g.Key,
                Total = g.Sum(x => x.Amount)
            })
            .ToListAsync();
    }
}