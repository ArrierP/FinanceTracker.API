using FinanceTracker.Api.Data;
using FinanceTracker.API.Entities;
using FinanceTracker.API.Enums;
using FinanceTracker.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            // Kiểm tra xem đã có dữ liệu chưa
            if (context.Users.Any())
            {
                return; // Đã có dữ liệu, không seed nữa
            }

            // 1. Tạo Admin mặc định
            var admin = new User
            {
                Email = "admin@finance.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                IsLocked = false
            };

            // 2. Tạo User mẫu
            var user = new User
            {
                Email = "user@finance.com",
                Password = BCrypt.Net.BCrypt.HashPassword("User@123"),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                IsLocked = false
            };

            context.Users.AddRange(admin, user);
            await context.SaveChangesAsync();

            // 3. Tạo các Category mặc định cho User mẫu
            var categories = new List<Category>
            {
                new Category { Name = "Lương", Type = CategoryType.Income, UserId = user.Id, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Thưởng", Type = CategoryType.Income, UserId = user.Id, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Ăn uống", Type = CategoryType.Expense, UserId = user.Id, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Di chuyển", Type = CategoryType.Expense, UserId = user.Id, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Mua sắm", Type = CategoryType.Expense, UserId = user.Id, CreatedAt = DateTime.UtcNow },
                new Category { Name = "Tiền điện/nước", Type = CategoryType.Expense, UserId = user.Id, CreatedAt = DateTime.UtcNow }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // 4. Tạo Ví mẫu cho User
            var wallet = new Wallet
            {
                Name = "Ví Tiền Mặt",
                Balance = 1000000,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            context.Wallets.Add(wallet);
            await context.SaveChangesAsync();

            // 5. Tạo một vài giao dịch mẫu
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 500000,
                    Description = "Lương tháng 4",
                    TransactionDate = DateTime.UtcNow.AddDays(-1),
                    Date = DateTime.UtcNow.AddDays(-1),
                    Type = TransactionType.Income,
                    WalletId = wallet.Id,
                    CategoryId = categories.First(c => c.Name == "Lương").Id,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Transaction
                {
                    Amount = 50000,
                    Description = "Ăn trưa",
                    TransactionDate = DateTime.UtcNow,
                    Date = DateTime.UtcNow,
                    Type = TransactionType.Expense,
                    WalletId = wallet.Id,
                    CategoryId = categories.First(c => c.Name == "Ăn uống").Id,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Transactions.AddRange(transactions);
            await context.SaveChangesAsync();
        }
    }
}