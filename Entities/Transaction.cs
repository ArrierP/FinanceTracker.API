using FinanceTracker.Api.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.API.Entities;

public class Transaction : BaseAuditableEntity
{
    [Column(TypeName = "decimal(18,2)")]
    // Số tiền giao dịch (dùng decimal để tính toán tiền tệ chính xác nhất)
    public decimal Amount { get; set; }

    // Ghi chú chi tiết cho giao dịch
    public string? Description { get; set; }

    // Ngày giờ phát sinh giao dịch
    public DateTime TransactionDate { get; set; }

    // Loại giao dịch
    public TransactionType Type { get; set; }

    // ==========================================
    // KHÓA NGOẠI VÀ ĐIỀU HƯỚNG (RELATIONSHIPS)
    // ==========================================

    // 1. Liên kết với Wallet (Ví): Giao dịch này trừ/cộng tiền vào Ví nào?
    public int WalletId { get; set; }
    public Wallet? Wallet { get; set; }
    public int? ToWalletId { get; set; }
    [ForeignKey("ToWalletId")]
    public Wallet? ToWallet { get; set; }
    // 2. Liên kết với Category (Danh mục): Giao dịch này thuộc nhóm nào?
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
}