using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.API.DTOs.Wallet
{
    public class CreateWalletDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public double InitialBalance { get; set; } = 0;
    }
}
