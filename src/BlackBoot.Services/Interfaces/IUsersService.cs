namespace BlackBoot.Services.Interfaces;

public interface IUsersService : ITransientDependency
{

    Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> CheckPasswordAsync(User user, string password, CancellationToken cancellationToken = default);
}
