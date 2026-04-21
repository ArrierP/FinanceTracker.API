using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FinanceTracker.API.Entities
{
    public class Wallet : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]

        [JsonIgnore]
        public User? User { get; set; }
    }
}
