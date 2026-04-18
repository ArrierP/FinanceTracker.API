using FinanceTracker.API.Enums;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        UserRole? UserRole { get; }
        bool IsAdmin { get; }
    }
}
