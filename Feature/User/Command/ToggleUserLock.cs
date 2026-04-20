using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTracker.Api.Data;

public class ToggleUserLockCommand : IRequest<Unit>
{
    public int UserId { get; set; }
}

public class ToggleUserLockHandler : IRequestHandler<ToggleUserLockCommand, Unit>
{
    private readonly ApplicationDbContext _context;

    public ToggleUserLockHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ToggleUserLockCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId);

        if (user == null)
            throw new Exception("User not found");

        user.IsLocked = !user.IsLocked;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}