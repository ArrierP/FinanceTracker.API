using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.API.Entities
{
    public class Wallet : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public double Balance { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; } = new User();
    }
}
