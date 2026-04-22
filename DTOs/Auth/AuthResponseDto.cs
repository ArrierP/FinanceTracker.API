using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace FinanceTracker.API.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
