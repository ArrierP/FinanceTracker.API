using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.API.DTOs.User;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _mediator.Send(new GetAllUsersQuery());
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}/toggle-lock")]
        public async Task<IActionResult> ToggleLock(int id)
        {
            try
            {
                await _mediator.Send(new ToggleUserLockCommand { UserId = id });
                return Ok(new { message = "User lock toggled successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}