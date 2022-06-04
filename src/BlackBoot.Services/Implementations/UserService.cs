#nullable disable
namespace BlackBoot.Services.Implementations;

public class UserService : IUserService
{
    private readonly DbSet<User> _users;

    public UserService(BlackBootDBContext context)
    {
        _users = context.Set<User>();
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default) => await _users.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper(), cancellationToken);
    public async Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default) => await _users.FindAsync(new object[] { id }, cancellationToken);
    public bool CheckPassword(User user, string password, CancellationToken cancellationToken = default) => HashGenerator.Hash(password) == user.Password;
}