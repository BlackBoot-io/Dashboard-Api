namespace BlackBoot.Services.Interfaces;

public interface IUserService : IScopedDependency
{
    Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    bool CheckPassword(User user, string password, CancellationToken cancellationToken = default);
}