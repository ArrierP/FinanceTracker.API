using FinanceTracker.Api.Data;
using FinanceTracker.API.DTOs.Category;
using FinanceTracker.API.Entities;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services.Implements;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CategoryService(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        // Kiểm tra xem User này đã có danh mục riêng chưa
        var userCategories = await _context.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync();

        // Nếu User đã có danh mục (kể cả do hệ thống copy sang lúc tạo account), 
        // thì chỉ trả về danh mục của họ thôi.
        if (userCategories.Any())
        {
            return userCategories;
        }

        // Nếu User hoàn toàn trống trải (vừa tạo acc xong), mới lấy hàng mặc định
        return await _context.Categories
            .Where(c => c.IsDefault && c.UserId == null)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task<Category> CreateAsync(CreateCategoryDto dto)
    {
        int userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        var category = new Category
        {
            Name = dto.Name,
            Type = dto.Type,
            UserId = userId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await GetByIdAsync(id);
        if (category == null) return false;

        category.Name = dto.Name;
        category.Type = dto.Type;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }
}