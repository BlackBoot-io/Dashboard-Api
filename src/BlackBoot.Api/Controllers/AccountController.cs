namespace BlackBoot.Api.Controllers;

public class AccountController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService) => _accountService = accountService;

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken)
        => Ok(await _accountService.LoginAsync(userLoginDto, cancellationToken));

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> LoginByGoogleAsync(string token, CancellationToken cancellationToken)
    => Ok(await _accountService.LoginByGoogleAsync(token, cancellationToken));

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        => Ok(await _accountService.RefreshTokenAsync(refreshToken, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> ChangePasswordAsync(Guid userId, UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default)
        => Ok(await _accountService.ChangePasswordAsync(userId, userChangePasswordDto, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
        => Ok(await _accountService.GetCurrentUserAsync(userId, cancellationToken));

    [HttpPut]
    public async Task<IActionResult> UpdateProfileAsync(UserDto userDto, CancellationToken cancellationToken = default)
        => Ok(await _accountService.UpdateProfileAsync(userDto, cancellationToken));

    [HttpPut]
    public async Task<IActionResult> UpdateWalletAsync(Guid userId, string withdrawalWallet, CancellationToken cancellationToken = default)
        => Ok(await _accountService.UpdateWalletAsync(userId, withdrawalWallet, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> LogoutAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
        => Ok(await _accountService.LogoutAsync(userId, refreshToken, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> SignupAsync(User user, CancellationToken cancellationToken)
        => Ok(await _accountService.SignupAsync(user, cancellationToken));

    [HttpPost]
    public IActionResult RecoveryPassword(Guid userId, string email)
        => Ok(_accountService.RecoveryPassword(userId, email));
}