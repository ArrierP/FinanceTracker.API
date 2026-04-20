namespace FinanceTracker.API.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
}