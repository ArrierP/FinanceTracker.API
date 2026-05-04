using FinanceTracker.API.DTOs.Dashboard;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardOverviewDto> GetOverviewAsync();

        Task<MonthlySummaryDto> GetMonthlySummaryAsync(int days=7);

        // Xu hướng nhiều tháng (Biểu đồ đường)
        Task<List<MonthlyTrendDto>> GetMonthlyTrendAsync(int months = 6);

        // Thống kê theo danh mục (Biểu đồ tròn: Ăn uống, Di chuyển...)
        Task<List<CategorySummaryDto>> GetCategorySummaryAsync();

        // Tóm tắt ví: Mỗi ví còn bao nhiêu tiền
        Task<List<WalletSummaryDto>> GetWalletSummaryAsync();
    }
}
