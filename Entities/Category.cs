using FinanceTracker.Api.Enums;

namespace FinanceTracker.API.Entities;

// Kế thừa BaseAuditableEntity để tự động có Id, CreatedAt, CreatedBy...
public class Category : BaseAuditableEntity
{
    // Tên danh mục (Ví dụ: Lương, Ăn uống, Tiền điện)
    public string Name { get; set; } = string.Empty;

    // Phân loại: Thu nhập hay Chi tiêu
    public CategoryType Type { get; set; }

    // ==========================================
    // KHÓA NGOẠI VÀ ĐIỀU HƯỚNG (RELATIONSHIPS)
    // ==========================================

    // 1. Liên kết với User: Mỗi danh mục do 1 User tạo ra (để không xem chéo của nhau)
    public int UserId { get; set; }
    public User? User { get; set; }

    // 2. Liên kết với Transaction: Một danh mục có thể chứa nhiều giao dịch (Quan hệ 1-Nhiều)
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}