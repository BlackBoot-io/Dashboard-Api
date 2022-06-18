namespace BlackBoot.Services.Interfaces;

public interface IAccountService : IScopedDependency
{
    Task<IActionResponse<UserTokenDto>> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default);
    Task<IActionResponse<UserTokenDto>> LoginByGoogleAsync(string token, CancellationToken cancellationToken);
    Task<IActionResponse<UserTokenDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<IActionResponse> LogoutAsync(Guid userId,string refreshToken, CancellationToken cancellationToken = default);
    Task<IActionResponse<UserDto>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IActionResponse<Guid>> UpdateProfileAsync(Guid userId, UserDto userDto, CancellationToken cancellationToken = default);
    Task<IActionResponse<Guid>> ChangePasswordAsync(Guid userId,UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default);
    Task<IActionResponse<UserTokenDto>> SignupAsync(User user, CancellationToken cancellationToken = default);
    Task<IActionResponse<Guid>> UpdateWalletAsync(Guid userId, UserUpdateWalletDto userUpdateWalletDto, CancellationToken cancellationToken = default);
    Task<IActionResponse<bool>> RecoveryPassword(Guid userId, string email);
}
