using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.API.Services.Interfaces;
using FinanceTracker.API.DTOs.Admin;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _service.GetAllUsersAsync());
        }

        [HttpPost("lock/{id}")]
        public async Task<IActionResult> Lock(int id)
        {
            await _service.LockUserAsync(id);
            return Ok(new { message = "User locked" });
        }

        [HttpPost("unlock/{id}")]
        public async Task<IActionResult> Unlock(int id)
        {
            await _service.UnlockUserAsync(id);
            return Ok(new { message = "User unlocked" });
        }

        // --- THỐNG KÊ HỆ THỐNG ---
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            // Trả về dữ liệu tổng quát cho Dashboard (Tổng user, giao dịch, tăng trưởng)
            return Ok(await _service.GetSystemStatsAsync());
        }

        //--- QUẢN LÝ DANH MỤC MẶC ĐỊNH ---
        [HttpGet("categories/defaults")]
        public async Task<IActionResult> GetDefaultCategories()
        {
            // Lấy danh sách các danh mục mẫu (IsDefault = true)
            return Ok(await _service.GetDefaultCategoriesAsync());
        }

        [HttpPost("categories/defaults")]
        public async Task<IActionResult> CreateDefaultCategory([FromBody] GlobalCategoryDto dto)
        {
            // Admin tạo danh mục mới để dùng chung cho toàn hệ thống
            await _service.CreateDefaultCategoryAsync(dto);
            return Ok(new { message = "Tạo danh mục mặc định thành công!" });
        }
    }
}

