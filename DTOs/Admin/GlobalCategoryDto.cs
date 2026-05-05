using FinanceTracker.Api.Enums;

namespace FinanceTracker.API.DTOs.Admin
{
    public class GlobalCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //public string Icon { get; set; } = string.Empty;
        public CategoryType Type { get; set; } // Income hoặc Expense
    }
}


