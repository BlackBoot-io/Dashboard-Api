#nullable disable
namespace BlackBoot.Services.Implementations;

public class UsersService : IUsersService
{
    private readonly DbSet<User> _users;
    private readonly BlackBootDBContext _context;

    public UsersService(BlackBootDBContext context)
    {
        _users = context.Set<User>();
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default) => await _users.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper(), cancellationToken);
    public async Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default) => await _users.FindAsync(new object[] { id }, cancellationToken);
    public bool CheckPassword(User user, string password, CancellationToken cancellationToken = default) => HashGenerator.Hash(password) == user.Password;
    public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default) 
    {
        _users.Update(user); 
        // If 1 or more state entries were written to the DB, return true otherwise false
        return await _context.SaveChangesAsync() > 0;
    }
}