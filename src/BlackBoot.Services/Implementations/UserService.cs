#nullable disable
namespace BlackBoot.Services.Implementations;

public class UserService : IUserService
{
    private readonly DbSet<User> _users;

    public UserService(BlackBootDBContext context) => _users = context.Set<User>();

    public async Task<IActionResponse<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default) => new ActionResponse<User>(await _users.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper(), cancellationToken));
    public async Task<IActionResponse<User>> GetAsync(Guid id, CancellationToken cancellationToken = default) => new ActionResponse<User>(await _users.FindAsync(new object[] { id }, cancellationToken));
    public IActionResponse<bool> CheckPassword(User user, string password, CancellationToken cancellationToken = default) => new ActionResponse<bool>(HashGenerator.Hash(password) == user.Password);
}