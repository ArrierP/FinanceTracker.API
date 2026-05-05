using FinanceTracker.API.DTOs.Admin;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task LockUserAsync(int userId);
        Task UnlockUserAsync(int userId);
        // --- Thống kê hệ thống ---
        Task<SystemStatsDto> GetSystemStatsAsync();

        // --- Quản lý danh mục mặc định ---
        Task<List<GlobalCategoryDto>> GetDefaultCategoriesAsync();
        Task CreateDefaultCategoryAsync(GlobalCategoryDto dto);

        Task UpdateDefaultCategoryAsync(int id, GlobalCategoryDto dto);
        Task DeleteDefaultCategoryAsync(int id);
    }
}

