using FinanceTracker.API.Enums;
using FinanceTracker.API.Services.Interfaces;
using System.Security.Claims;

namespace FinanceTracker.API.Services.Implements
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
                return null;
            }
        }

        public UserRole? UserRole 
        {
            get
            {
                var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);
                return Enum.TryParse<UserRole>(roleClaim?.Value, true, out UserRole userRole) ? userRole : (UserRole?)null;
            }
        }

        public bool IsAdmin => UserRole == Enums.UserRole.Admin;

    }
}
