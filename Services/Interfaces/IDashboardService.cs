using FinanceTracker.API.DTOs.Dashboard;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<MonthlySummaryDto> GetMonthlySummaryAsync();
    }
}
