#nullable disable
namespace BlackBoot.Services.Implementations;

public class UserService : IUserService
{
    private readonly DbSet<User> _users;
    private readonly BlackBootDBContext _context;

    public UserService(BlackBootDBContext context)
    {
        _context = context;
        _users = context.Set<User>();
    }

    public async Task<IActionResponse<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => new ActionResponse<User>(await _users.FirstOrDefaultAsync(x => x.Email == email.ToLower(), cancellationToken));

    public async Task<IActionResponse<User>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => new ActionResponse<User>(await _users.FindAsync(new object[] { id }, cancellationToken));

    public async Task<IActionResponse<bool>> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Update(user);
        await _context.SaveChangesAsync();
        return new ActionResponse<bool>(true);
    }

    public IActionResponse<bool> CheckPassword(User user, string password, CancellationToken cancellationToken = default)
        => new ActionResponse<bool>(HashGenerator.Hash(password) == user.Password);
}