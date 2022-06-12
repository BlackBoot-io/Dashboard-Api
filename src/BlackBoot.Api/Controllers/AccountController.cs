namespace BlackBoot.Api.Controllers;

public class AccountController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService) => _accountService = accountService;

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken) 
        => Ok(await _accountService.LoginAsync(userLoginDto, cancellationToken));

    public async Task<IActionResult> LoginByGoogleAsync(string token, CancellationToken cancellationToken)
    => Ok(await _accountService.LoginByGoogleAsync(token, cancellationToken));

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        => Ok(await _accountService.RefreshTokenAsync(refreshToken, cancellationToken));
    
    [HttpPost]
    public async Task<IActionResult> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default)
        => Ok(await _accountService.ChangePasswordAsync(userChangePasswordDto, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetCurrentUserAsync(CancellationToken cancellationToken)
        => Ok(await _accountService.GetCurrentUserAsync(cancellationToken));

    [HttpPut]
    public async Task<IActionResult> UpdateProfileAsync(UserDto userDto, CancellationToken cancellationToken = default)
        => Ok(await _accountService.UpdateProfileAsync(userDto, cancellationToken));

    [HttpPut]
    public async Task<IActionResult> UpdateWalletAsync(string withdrawalWallet, CancellationToken cancellationToken = default)
        => Ok(await _accountService.UpdateWalletAsync(withdrawalWallet, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> LogoutAsync(string refreshToken, CancellationToken cancellationToken)
        => Ok(await _accountService.LogoutAsync(refreshToken, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> SignupAsync(User user, CancellationToken cancellationToken)
        => Ok(await _accountService.SignupAsync(user, cancellationToken));
}