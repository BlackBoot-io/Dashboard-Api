namespace BlackBoot.Services.Interfaces;

public interface IUserService : IScopedDependency
{
    Task<IActionResponse<UserTokenDto>> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<IActionResponse<User>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IActionResponse<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IActionResponse<Guid>> UpdateAsync(User user, CancellationToken cancellationToken = default); 
    IActionResponse<bool> CheckPassword(User user, string password, CancellationToken cancellationToken = default);
}