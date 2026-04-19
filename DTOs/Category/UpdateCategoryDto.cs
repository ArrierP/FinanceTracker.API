using FinanceTracker.Api.Enums;

namespace FinanceTracker.API.DTOs.Category;

public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
}