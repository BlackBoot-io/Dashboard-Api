namespace BlackBoot.Services.Interfaces;

public interface IUsersService : ITransientDependency
{
    Task<ApiResult<User>> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<User> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    bool CheckPassword(User user, string password, CancellationToken cancellationToken = default);
}