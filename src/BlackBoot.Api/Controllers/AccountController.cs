using BlackBoot.Domain.Entities;

namespace BlackBoot.Api.Controllers;

public class AccountController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService) => _accountService = accountService;

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<UserTokenDto>> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken) => Ok(await _accountService.LoginAsync(userLoginDto, cancellationToken));

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<UserTokenDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken) => Ok(await _accountService.RefreshTokenAsync(refreshToken, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<bool>> ChangePassword(UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default) => Ok(await _accountService.ChangePassword(userChangePasswordDto, cancellationToken));

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUserAsync(CancellationToken cancellationToken) => Ok(await _accountService.GetCurrentUserAsync(cancellationToken));

    [HttpPut]
    public async Task<ActionResult<bool>> UpdateProfileAsync(UserDto userDto, CancellationToken cancellationToken = default)
        => Ok(await _accountService.UpdateProfileAsync(userDto, cancellationToken));

    [HttpDelete]
    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken) 
        => await _accountService.LogoutAsync(refreshToken, cancellationToken);

    [HttpPost]
    public async Task SignupAsync(User user, CancellationToken cancellationToken) 
        => await _accountService.SignupAsync(user, cancellationToken);
}