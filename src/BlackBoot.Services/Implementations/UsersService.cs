using Microsoft.EntityFrameworkCore;

namespace BlackBoot.Services.Implementations;

public class UsersService : IUsersService
{

    private readonly BlackBootDBContext _context;
    private readonly DbSet<User> _users;

    public UsersService(BlackBootDBContext context)
    {
        _context = context;
        _users = context.Set<User>();
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default) => await _users.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper(), cancellationToken);

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await _users.FindAsync(new object[] { id }, cancellationToken);

}
