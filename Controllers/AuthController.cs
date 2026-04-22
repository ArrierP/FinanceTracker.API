using FinanceTracker.API.DTOs.Auth;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                await _authService.RegisterAsync(registerDto);
                return Ok(new { message = "Registration successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var authResponse = await _authService.LoginAsync(loginDto);

                return Ok(new
                {
                    token = authResponse.Token,
                    user = new
                    {
                        email = authResponse.Email,
                        role = authResponse.Role // Trả về "Admin" hoặc "User"
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
