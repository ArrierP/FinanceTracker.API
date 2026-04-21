using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        return Ok("User locked");
    }

    [HttpPost("unlock/{id}")]
    public async Task<IActionResult> Unlock(int id)
    {
        await _service.UnlockUserAsync(id);
        return Ok("User unlocked");
    }
}