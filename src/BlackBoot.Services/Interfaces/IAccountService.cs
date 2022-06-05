namespace BlackBoot.Services.Interfaces;

public interface IAccountService : IScopedDependency
{
    Task<IActionResponse<UserTokenDto>> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default);
    Task<IActionResponse<UserTokenDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<IActionResponse> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<IActionResponse<UserDto>> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    Task<IActionResponse<bool>> UpdateProfileAsync(UserDto userDto, CancellationToken cancellationToken = default);
    Task<IActionResponse<bool>> ChangePassword(UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default);
    Task<IActionResponse<UserTokenDto>> SignupAsync(User user, CancellationToken cancellationToken = default);
}
