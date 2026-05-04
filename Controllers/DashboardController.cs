using FinanceTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    //[Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        // 1. Lấy tổng quan: Số dư thực có trong ví, Tổng thu, Tổng chi tháng hiện tại
        // Giải quyết lỗi người dùng không biết số dư thực tế trong file test
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            var result = await _service.GetOverviewAsync();
            return Ok(result);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthly([FromQuery] int days = 7)
        {
            var result = await _service.GetMonthlySummaryAsync(days);
            return Ok(result);
        }

        // 3. Lấy thống kê theo danh mục (Dữ liệu cho biểu đồ tròn)
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategorySummary()
        {
            var result = await _service.GetCategorySummaryAsync();
            return Ok(result);
        }

        // 4. Lấy danh sách ví và số dư chi tiết từng ví
        [HttpGet("wallets")]
        public async Task<IActionResult> GetWalletSummary()
        {
            var result = await _service.GetWalletSummaryAsync();
            return Ok(result);
        }

        // 5. Lấy xu hướng thu chi qua các tháng (Dữ liệu cho biểu đồ đường)
        [HttpGet("trend")]
        public async Task<IActionResult> GetMonthlyTrend([FromQuery] int months = 6)
        {
            var result = await _service.GetMonthlyTrendAsync(months);
            return Ok(result);
        }
    }
}

