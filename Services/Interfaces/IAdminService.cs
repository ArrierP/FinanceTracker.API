using FinanceTracker.API.DTOs.Admin;
using FinanceTracker.API.Entities;

public interface IAdminService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task LockUserAsync(int userId);
    Task UnlockUserAsync(int userId);
}