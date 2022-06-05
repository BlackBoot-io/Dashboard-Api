using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlackBoot.Services.Implementations;

public class AccountService : IAccountService
{

    private readonly IUsersService _usersservice;
    private readonly IUserJwtTokensService _userTokensService;
    private readonly IJwtTokenFactory _userTokenFactoryService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AccountService(IUsersService usersservice,
                          IUserJwtTokensService userTokensService,
                          IJwtTokenFactory userTokenFactoryService,
                          IHttpContextAccessor httpContextAccessor)
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

        var checkPasswordResult = _usersservice.CheckPassword(user, item.Password, cancellationToken);
        if (!checkPasswordResult) throw new NotFoundException(AppResource.InvalidUser);

        var usertokens = await GenerateTokenAsync(user.UserId);
        await _userTokensService.AddUserTokenAsync(user.UserId, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
        return usertokens;
    }
    public async Task LogoutAsync(string refreshtoken, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        if (userId is null || string.IsNullOrEmpty(refreshtoken))
            throw new BadRequestException(AppResource.InvalidUser);

        await _userTokensService.RevokeUserTokensAsync(userId.Value, refreshtoken, cancellationToken);
    }
    public async Task<UserTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshTokenModel = await _userTokensService.GetRefreshTokenAsync(refreshToken, cancellationToken);
        if (refreshTokenModel is null)
            throw new BadRequestException(AppResource.InvalidUser);

        var usertokens = await GenerateTokenAsync(refreshTokenModel.UserId.Value);
        await _userTokensService.AddUserTokenAsync(refreshTokenModel.UserId.Value, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
        return usertokens;
    }
    public async Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        if (userId is null)
            throw new BadRequestException(AppResource.InvalidUser);
        var user = await _usersservice.GetAsync(userId.Value, cancellationToken);
        return new UserDto
        {
            Email = user.Email,
            FullName = user.FullName,
            Gender = user.Gender,
            Nationality = user.Nationality,
            BirthdayDate = user.BirthdayDate
        };
    }
    public async Task<bool> UpdateProfileAsync(UserDto userDto, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        if (userId is null)
            throw new BadRequestException(AppResource.InvalidUser);
        var user = await _usersservice.GetAsync(userId.Value, cancellationToken);
        if (user == null) throw new NotFoundException(AppResource.InvalidUser);

        #region Update Profile
        user.Email = userDto.Email;
        user.FullName = userDto.FullName;
        user.Gender = userDto.Gender;
        user.Nationality = userDto.Nationality;
        user.BirthdayDate = userDto.BirthdayDate;
        #endregion

        return await _usersservice.UpdateAsync(user, cancellationToken);
    }
    private async Task<UserTokenDto> GenerateTokenAsync(Guid userId)
    {
        var user = await _usersservice.GetAsync(userId, default);
        var accessToken = _userTokenFactoryService.CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Email,user.Email)
            }, JwtTokenType.AccessToken);

        var refreshToken = _userTokenFactoryService.CreateToken(new List<Claim>
            {
               new Claim("AccessToken",accessToken.Token)

            }, JwtTokenType.RefreshToken);
        var result = new UserTokenDto()
        {
            AccessToken = accessToken.Token,
            AccessTokenExpireTime = DateTimeOffset.UtcNow.AddMinutes(accessToken.TokenExpirationMinutes),
            RefreshToken = refreshToken.Token,
            RefreshTokenExpireTime = DateTimeOffset.UtcNow.AddMinutes(refreshToken.TokenExpirationMinutes),
            User = new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Gender = user.Gender,
                Nationality = user.Nationality
            }
        };

        return result;
    }
}