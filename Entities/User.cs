using FinanceTracker.API.Enums;

namespace FinanceTracker.API.Entities
{
    public class User : BaseAuditableEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = new UserRole();
        public bool IsLocked { get; set; }
        public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
}
