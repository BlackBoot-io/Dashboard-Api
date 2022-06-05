namespace BlackBoot.Services.Interfaces;

public interface IAccountService : ITransientDependency
{
    Task<UserTokenDto> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default);
    Task<UserTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateProfileAsync(UserDto userDto, CancellationToken cancellationToken = default);
}
