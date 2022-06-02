
using Microsoft.AspNetCore.Http;

namespace BlackBoot.Services.Implementations;

public class AccountService : IAccountService
{

    private readonly IUsersService _usersservice;
    private readonly IUserJwtTokensService _userTokensService;
    private readonly IUserJwtTokenFactory _userTokenFactoryService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AccountService(IUsersService usersservice, IUserJwtTokensService userTokensService, IUserJwtTokenFactory userTokenFactoryService, IHttpContextAccessor httpContextAccessor)
    {
        _usersservice = usersservice;
        _userTokensService = userTokensService;
        _userTokenFactoryService = userTokenFactoryService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserTokenDto> LoginAsync(UserLoginDto item, CancellationToken cancellationToken = default)
    {

        var user = await _usersservice.GetByEmailAsync(item.Email, cancellationToken);
        if (user == null) throw new NotFoundException(AppResource.InvalidUser);
        var result = HashGenerator.Hash(user.Password) == item.Password;
        if (HashGenerator.Hash(user.Password) != item.Password) throw new NotFoundException(AppResource.InvalidUser);

        var usertokens = await _userTokenFactoryService.GenerateTokenAsync(user.UserId);
        await _userTokensService.AddUserTokenAsync(user.UserId, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
        return usertokens;
    }
    public async Task LogoutAsync(string refreshtoken, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext.User.Identity.GetUserIdAsGuid();
        if (userId == null || string.IsNullOrEmpty(refreshtoken))
            throw new BadRequestException(AppResource.InvalidUser);

        await _userTokensService.RevokeUserTokensAsync(userId.Value, refreshtoken, cancellationToken);
    }
    public async Task<UserTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshTokenModel = await _userTokensService.GetRefreshTokenAsync(refreshToken, cancellationToken);
        if (refreshTokenModel == null)
            throw new BadRequestException(AppResource.InvalidUser);

        var usertokens = await _userTokenFactoryService.GenerateTokenAsync(refreshTokenModel.UserId.Value);
        await _userTokensService.AddUserTokenAsync(refreshTokenModel.UserId.Value, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
        return usertokens;
    }
    public async Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext.User.Identity.GetUserIdAsGuid();
        if (userId == null)
            throw new BadRequestException(AppResource.InvalidUser);
        var user = await _usersservice.GetByIdAsync(userId.Value, cancellationToken);
        return new UserDto { };
    }
}