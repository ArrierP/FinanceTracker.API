namespace FinanceTracker.API.DTOs.Admin
{
    public class SystemStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers24h { get; set; }
        public int TotalTransactions24h { get; set; }
        public List<UserGrowthDto> UserGrowth { get; set; } = [];
        public List<TopCategoryDto> TopCategories { get; set; } = [];
    }
    public class UserGrowthDto
    {
        public string Date { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
