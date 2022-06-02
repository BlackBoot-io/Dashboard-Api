namespace BlackBoot.Services.Interfaces;

public interface IUsersService : ITransientDependency
{

    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
