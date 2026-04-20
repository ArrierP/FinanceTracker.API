using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.API.DTOs.Dashboard;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(int month, int year)
    {
        try
        {
            var result = await _mediator.Send(new GetMonthlySummaryQuery
            {
                Month = month,
                Year = year
            });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("category-chart")]
    public async Task<IActionResult> GetCategoryChart()
    {
        var result = await _mediator.Send(new GetCategoryChartDataQuery());
        return Ok(result);
    }

    [HttpGet("top-expenses")]
    public async Task<IActionResult> GetTopExpenses()
    {
        var result = await _mediator.Send(new GetTopExpensesQuery());
        return Ok(result);
    }
}