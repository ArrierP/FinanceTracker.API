using FinanceTracker.API.Entities;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FinanceTracker.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Bỏ .ToString() để truyền thẳng int? vào int?
                        entry.Entity.CreatedBy = _currentUserService.UserId ?? 0;
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        // Bỏ .ToString()
                        entry.Entity.LastModifiedBy = _currentUserService.UserId ?? 0 ;
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        // ĐÃ GỘP TẤT CẢ VÀO 1 HÀM DUY NHẤT Ở ĐÂY
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. Cấu hình quan hệ User -> Wallet
            // ==========================================
            modelBuilder.Entity<User>()
                .HasMany(u => u.Wallets)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Ví sẽ bị xóa nếu người dùng bị xóa

            // ==========================================
            // 2. Cấu hình quan hệ để tránh lỗi Cascade
            // ==========================================
            // Tắt tự động xóa Transaction khi xóa Wallet
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Wallet)
                .WithMany()
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tắt tự động xóa Transaction khi xóa Category
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}