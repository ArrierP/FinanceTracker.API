using FinanceTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthly()
    {
        var result = await _service.GetMonthlySummaryAsync();
        return Ok(result);
    }
}