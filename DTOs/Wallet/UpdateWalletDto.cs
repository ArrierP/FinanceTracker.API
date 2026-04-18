using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.API.DTOs.Wallet
{
    public class UpdateWalletDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
