using FinanceTracker.Api.Data;
using FinanceTracker.API.DTOs.Admin;
using FinanceTracker.API.Entities;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Implements
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role.ToString(), // Chuyển Enum sang String
                    IsLocked = user.IsLocked
                })
                .ToListAsync();
        }
        public async Task LockUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.IsLocked = true;
            await _context.SaveChangesAsync();
        }

        public async Task UnlockUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.IsLocked = false;
            await _context.SaveChangesAsync();
        }

        // --- BỔ SUNG MỤC 1: THỐNG KÊ HỆ THỐNG ---
        public async Task<SystemStatsDto> GetSystemStatsAsync()
        {
            var now = DateTime.UtcNow;
            var last24h = now.AddDays(-1);

            // 1. Tổng số người dùng trên hệ thống
            var totalUsers = await _context.Users.CountAsync();

            // 2. System Traffic: Số lượng giao dịch trong 24h qua
            var transactions24h = await _context.Transactions
                .Where(t => t.CreatedAt >= last24h)
                .CountAsync();

            // 3. Active Users: Người dùng có đăng nhập trong 24h qua (Dựa trên LastLogin)
            var activeUsers = await _context.Users
                .Where(u => u.LastLogin >= last24h)
                .CountAsync();

            // 4. User Growth: Lượng người dùng mới đăng ký trong 7 ngày gần nhất
            var growth = await _context.Users
                        .Where(u => u.CreatedAt >= now.AddDays(-7))
                        .GroupBy(u => u.CreatedAt.Date)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Count()
                        })
                        .ToListAsync();
            var result = growth
                .Select(g => new UserGrowthDto
                {
                    Date = g.Date.ToString("dd/MM"),
                    Count = g.Count
                })
                .OrderBy(x => x.Date)
                .ToList();
            // Thống kê Top 5 danh mục được sử dụng nhiều nhất trên hệ thống
            var topCategories = await _context.Transactions
                .GroupBy(t => t.CategoryId)
                .Select(g => new TopCategoryDto
                {
                    CategoryName = _context.Categories.Where(c => c.Id == g.Key).Select(c => c.Name).FirstOrDefault()??"Không xác định",
                    UsageCount = g.Count()
                })
                .OrderByDescending(x => x.UsageCount)
                .Take(5)
                .ToListAsync();

            // Trả về thêm topCategories trong SystemStatsDto

            return new SystemStatsDto
            {
                TotalUsers = totalUsers,
                ActiveUsers24h = activeUsers,
                TotalTransactions24h = transactions24h,
                UserGrowth = result,
                TopCategories = topCategories
            };


        }

        // --- BỔ SUNG MỤC 3: QUẢN LÝ DANH MỤC MẶC ĐỊNH ---
        public async Task CreateDefaultCategoryAsync(GlobalCategoryDto dto)
        {
            // Tạo danh mục mới với đánh dấu IsDefault = true
            // Lưu ý: UserId sẽ để null vì đây là danh mục dùng chung cho toàn hệ thống
            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type,
                IsDefault = true,
                UserId = null // Danh mục gốc không thuộc về riêng user nào
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GlobalCategoryDto>> GetDefaultCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsDefault)
                .Select(c => new GlobalCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type
                })
                .ToListAsync();
        }

        public async Task UpdateDefaultCategoryAsync(int id, GlobalCategoryDto dto)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDefault == true);

            if (category == null)
                throw new Exception("Default category not found");

            category.Name = dto.Name;
            category.Type = dto.Type;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDefaultCategoryAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDefault == true);

            if (category == null)
                throw new Exception("Default category not found");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

    }
}
  