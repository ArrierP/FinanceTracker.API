using FinanceTracker.API.DTOs.Auth;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}
