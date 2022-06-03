namespace BlackBoot.Services.Interfaces;

public interface IUserJwtTokensService : ITransientDependency
{
    Task AddUserTokenAsync(Guid userId, string accessToken, string refreshToken, CancellationToken cancellationToken = default);
    Task RevokeUserTokensAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> VerifyTokenAsync(Guid userId, string accessToken, CancellationToken cancellationToken = default);
    Task<UserJwtToken> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
