using BlackBoot.Services.ExternalAdapter;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace BlackBoot.Services.Implementations;

public class AccountService : IAccountService
{
    
    private readonly IUserService _userService;
    private readonly IUserJwtTokenService _userTokenService;
    private readonly IJwtTokenFactory _tokenFactoryService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration; 
 
    private readonly EmailGatwayAdapter _emailGatwayAdapter;

    public AccountService(IUserService userService,
                          IUserJwtTokenService userTokenService,
                          IJwtTokenFactory tokenFactoryService,
                          IHttpContextAccessor httpContextAccessor,
                          IConfiguration configuration )
    {
        _userService = userService;
        _userTokenService = userTokenService;
        _tokenFactoryService = tokenFactoryService;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration; 
    }

    public async Task<IActionResponse<UserTokenDto>> LoginAsync(UserLoginDto item, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetByEmailAsync(item.Email, cancellationToken);
        if (user.Data is null) return new ActionResponse<UserTokenDto>(ActionResponseStatusCode.NotFound, AppResource.InvalidUser);

        var checkPasswordResult = _userService.CheckPassword(user.Data, item.Password, cancellationToken);
        if (!checkPasswordResult.Data) return new ActionResponse<UserTokenDto>(ActionResponseStatusCode.NotFound, AppResource.InvalidUser);

        var usertokens = await GenerateTokenAsync(user.Data.UserId, cancellationToken);
        await _userTokenService.AddUserTokenAsync(user.Data.UserId, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
        return new ActionResponse<UserTokenDto>(usertokens);
    }
    public async Task<IActionResponse<UserTokenDto>> LoginByGoogleAsync(string token, CancellationToken cancellationToken)
    {
        try
        {
            GoogleJsonWebSignature.ValidationSettings settings = new();
            settings.Audience = new List<string>() { _configuration.GetSection("Authentication:Google:ClientId").Value };
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

            var user = await _userService.GetByEmailAsync(payload.Email, cancellationToken);
            if (user.Data is null)
            {
                user.Data = new User
                {
                    Email = payload.Email,
                    FullName = payload.Name,
                };
                await _userService.AddAsync(user.Data, cancellationToken);
            }
            var usertokens = await GenerateTokenAsync(user.Data.UserId, cancellationToken);
            await _userTokenService.AddUserTokenAsync(user.Data.UserId, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
            return new ActionResponse<UserTokenDto>(usertokens);

        }
        catch (Exception ex) when (ex is InvalidJwtException)
        {
            return new ActionResponse<UserTokenDto>(ActionResponseStatusCode.BadRequest);
        }
    }
    public async Task<IActionResponse> LogoutAsync(string refreshtoken, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        if (userId is null || string.IsNullOrEmpty(refreshtoken))
            return new ActionResponse(ActionResponseStatusCode.NotFound, AppResource.InvalidUser);

        await _userTokenService.RevokeUserTokensAsync(userId.Value, refreshtoken, cancellationToken);
        return new ActionResponse();
    }
    public async Task<IActionResponse<UserTokenDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshTokenModel = await _userTokenService.GetRefreshTokenAsync(refreshToken, cancellationToken);
        if (refreshTokenModel.Data is null)
            return new ActionResponse<UserTokenDto>(ActionResponseStatusCode.NotFound, AppResource.InvalidUser);

        var usertokens = await GenerateTokenAsync(refreshTokenModel.Data.UserId.Value, cancellationToken);
        await _userTokenService.AddUserTokenAsync(refreshTokenModel.Data.UserId.Value, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
        return new ActionResponse<UserTokenDto>(usertokens);
    }
    public async Task<IActionResponse<UserDto>> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        if (userId is null)
            return new ActionResponse<UserDto>(ActionResponseStatusCode.NotFound, AppResource.InvalidUser);
        var user = await _userService.GetAsync(userId.Value, cancellationToken);
        return new ActionResponse<UserDto>(new UserDto
        {
            Email = user.Data.Email,
            FullName = user.Data.FullName,
            Gender = user.Data.Gender,
            Nationality = user.Data.Nationality,
        });
    }
    public async Task<IActionResponse<Guid>> UpdateProfileAsync(UserDto userDto, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        if (userId is null)
            return new ActionResponse<Guid>(ActionResponseStatusCode.NotFound, AppResource.InvalidUser);
        var userResponse = await _userService.GetAsync(userId.Value, cancellationToken);
        var user = userResponse.Data;
        if (user == null) return new ActionResponse<Guid>(ActionResponseStatusCode.NotFound, AppResource.InvalidUser);

        #region Update Profile
        user.Email = userDto.Email;
        user.FullName = userDto.FullName;
        user.Gender = userDto.Gender;
        user.Nationality = userDto.Nationality;
        user.BirthdayDate = userDto.BirthdayDate;
        #endregion

        return await _userService.UpdateAsync(user, cancellationToken);
    }
    public async Task<IActionResponse<Guid>> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default)
    {
        if (userChangePasswordDto.ConfirmPassword != userChangePasswordDto.NewPassword)
            return new ActionResponse<Guid>(ActionResponseStatusCode.BadRequest, AppResource.NewAndConfirmPasswordsDoNotMatch);

        var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        if (userId is null) return new ActionResponse<Guid>(ActionResponseStatusCode.NotFound, AppResource.UserNotFound);

        var userGetResponse = await _userService.GetAsync(userId.Value, cancellationToken);
        var user = userGetResponse.Data;
        if (user == null) return new ActionResponse<Guid>(ActionResponseStatusCode.NotFound, AppResource.UserNotFound);

        if (HashGenerator.Hash(userChangePasswordDto.OldPassword) != user.Password)
            return new ActionResponse<Guid>(ActionResponseStatusCode.Forbidden, AppResource.PreviousPasswordsDoNotMatch);

        var hashedNewPassword = HashGenerator.Hash(userChangePasswordDto.NewPassword);
        user.Password = hashedNewPassword;

        return await _userService.UpdateAsync(user, cancellationToken);
    }
    public async Task<IActionResponse<Guid>> UpdateWalletAsync(string withdrawalWallet, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        if (userId is null) return new ActionResponse<Guid>(ActionResponseStatusCode.NotFound, AppResource.UserNotFound);

        var userGetResponse = await _userService.GetAsync(userId.Value, cancellationToken);
        var user = userGetResponse.Data;
        if (user == null) return new ActionResponse<Guid>(ActionResponseStatusCode.NotFound, AppResource.UserNotFound);

        user.WithdrawalWallet = withdrawalWallet;

        return await _userService.UpdateAsync(user, cancellationToken);
    }
    private async Task<UserTokenDto> GenerateTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userService.GetAsync(userId, cancellationToken);
        var accessToken = _tokenFactoryService.CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email,user.Data.Email)
            }, JwtTokenType.AccessToken);

        var refreshToken = _tokenFactoryService.CreateToken(new List<Claim>
            {
               new Claim("AccessToken",accessToken.Data.Token)

            }, JwtTokenType.RefreshToken);
        UserTokenDto result = new()
        {
            AccessToken = accessToken.Data.Token,
            AccessTokenExpireTime = DateTimeOffset.UtcNow.AddMinutes(accessToken.Data.TokenExpirationMinutes),
            RefreshToken = refreshToken.Data.Token,
            RefreshTokenExpireTime = DateTimeOffset.UtcNow.AddMinutes(refreshToken.Data.TokenExpirationMinutes),
            User = new UserDto
            {
                Email = user.Data.Email,
                FullName = user.Data.FullName,
                Gender = user.Data.Gender,
                Nationality = user.Data.Nationality
            }
        };

        return result;
    }

    public async Task<IActionResponse<UserTokenDto>> SignupAsync(User user, CancellationToken cancellationToken = default)
    {
        user.Password = HashGenerator.Hash(user.Password);
        var addedUser = await _userService.AddAsync(user, cancellationToken);

        if (!addedUser.IsSuccess) 
            return new ActionResponse<UserTokenDto>(ActionResponseStatusCode.BadRequest, AppResource.TransactionFailed);
        var usertokens = await GenerateTokenAsync(user.UserId, cancellationToken);
        await _userTokenService.AddUserTokenAsync(user.UserId, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
        return new ActionResponse<UserTokenDto>(usertokens);
    }


    public IActionResponse<bool> RecoveryPassword(string email)
    {
        EmailDto emailInfo = new EmailDto
        {
            Content = "Email Recovery",
            Receiver = email,
            Subject = "Recovery Password"
        };

        var response = _emailGatwayAdapter.Send(emailInfo);
        return new ActionResponse<bool>(response.Data);
    }
}