using FinanceTracker.API.Entities;

public interface IAdminService
{
    Task<List<User>> GetAllUsersAsync();
    Task LockUserAsync(int userId);
    Task UnlockUserAsync(int userId);
}