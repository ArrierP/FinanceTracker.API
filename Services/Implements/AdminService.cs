using FinanceTracker.Api.Data;
using FinanceTracker.API.DTOs.Admin;
using FinanceTracker.API.Entities;
using Microsoft.EntityFrameworkCore;

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
}