using BlackBoot.Services.ExternalAdapter;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace BlackBoot.Services.Implementations;

public class AccountService : IAccountService
{

    private readonly IUserService _userService;
    private readonly IUserJwtTokenService _userTokenService;
    private readonly IJwtTokenFactory _tokenFactoryService;
    private readonly IConfiguration _configuration;
    private readonly EmailGatwayAdapter _emailGatwayAdapter;

    private string _defaultAvatar = "iVBORw0KGgoAAAANSUhEUgAAACIAAAAiCAYAAAA6RwvCAAAABmJLR0QA/wD/AP+gvaeTAAADDUlEQVRYhe3XTYiVZRQH8N8dFSJrpZGpq6hwVVAGCqHt+lpYGdQisggac4J2umgnhIsUjCCCQDNKmgicvoSMFn1BqWBljTY50WiZm0HKzxTmtjjnxdvlve97753JTf3h4bn3fPzveb7OOZf/8U80+vBZglW4DQtxXcp/x3Hsx7v4cSYCbMcAHschNDGFUezBjhx7Uj+VNqNYk74zgpU4mORfYVDsRCcsxFp8nT7fYsV0gxjCRYzhQb0dZQOr0/cC1vUbxBaxol24ql+S9B1Jrs29Og+l41Yzc8YDeDE5n+7WaaU4jl01QdwnVnoUE2l/d00wI+KY7qgLYha+E0+v03EM4FWxul/wWo6JlL2i8126Gj/hGzU7/USSPVBh81zabMTsFvkcPJ+6DRX+D6XNY1WBHBJPtGpFp/FWBcc7+BNzO+gb2IsfOhEsyUgHK37k3rSpygt3pk3VfVmXNjcVgtZzuj+VH1QQXJPz0QqbiZyvrbApnvOqskBuxWH8VkFwIufrK2xuyPnXCpvjItEtLVN+iY8qnOFKnMR7HfQN7MYkrqjh+hiflyl+Fs+wDs+KbX0pA2sN8uXUDXXBswPjZYrxVNahgRdElZ3E+zkmU7apCw54HUfKFF+I7eoWy7FdJKcD2IZlPfh/gs/KFG+LHuJy4TCGiy+tr2a/yCWLL0MQi0QO2VumvFF31fEWrBer2See4ViSDqfu5hqOZ7QltHaMis6qPcU38Ai+T4KmuNy7sRNv4kNx+YpW8WD6lHHtS66OWJMkq1tkC/Bpyg+IEjC/gmM+nkrbZvouaNE/nPJHqwIZED3mmGgD5olVnhLNcy9NUtFwnxKlf54omkcyyFquFaJ5GRHbfha39xBAO5YlxxsiI3fVGBUoquOU6DGmi00u3Z21vTrvTMcT4rn1i8XJ0UzOvjCcBBdEWu/1jmxO36Y4mmlhEOeS7EwSlpbvxFLxnM+kzzk8Od0gCswRTfF5l/LIX6LQjeeYTFmhPy+q8ewSvhnBPaKLO4Y/xF+Pi/n5mKjEd/1bP/7fwd9Yt9Wn5+1cgAAAAABJRU5ErkJggg==";

    public AccountService(IUserService userService,
                          IUserJwtTokenService userTokenService,
                          IJwtTokenFactory tokenFactoryService,
                          IConfiguration configuration)
    {
        _userService = userService;
        _userTokenService = userTokenService;
        _tokenFactoryService = tokenFactoryService;
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
    public async Task<IActionResponse> LogoutAsync(Guid userId, string refreshtoken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(refreshtoken))
            return new ActionResponse(ActionResponseStatusCode.NotFound, AppResource.InvalidUser);

        await _userTokenService.RevokeUserTokensAsync(userId, refreshtoken, cancellationToken);
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
    public async Task<IActionResponse<UserDto>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetAsync(userId, cancellationToken);
        return new ActionResponse<UserDto>(new UserDto
        {
            Email = user.Data.Email,
            FullName = user.Data.FullName,
            Gender = user.Data.Gender,
            BirthdayDate = user.Data.BirthdayDate.Value,
            Nationality = user.Data.Nationality,
            WalletAddress = user.Data.WithdrawalWallet,
            Avatar = "data:image/png;base64," + Convert.ToBase64String(user.Data.Avatar),
            UserId = user.Data.UserId
        });
    }
    public async Task<IActionResponse<Guid>> UpdateProfileAsync(Guid userId, UserDto userDto, CancellationToken cancellationToken = default)
    {
        var userResponse = await _userService.GetAsync(userId, cancellationToken);
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
    public async Task<IActionResponse<Guid>> ChangePasswordAsync(Guid userId, UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default)
    {
        if (userChangePasswordDto.ConfirmNewPassword != userChangePasswordDto.NewPassword)
            return new ActionResponse<Guid>(ActionResponseStatusCode.BadRequest, AppResource.NewAndConfirmPasswordsDoNotMatch);

        var userGetResponse = await _userService.GetAsync(userId, cancellationToken);
        var user = userGetResponse.Data;
        if (user == null) return new ActionResponse<Guid>(ActionResponseStatusCode.NotFound, AppResource.UserNotFound);

        if (HashGenerator.Hash(userChangePasswordDto.OldPassword) != user.Password)
            return new ActionResponse<Guid>(ActionResponseStatusCode.Forbidden, AppResource.PreviousPasswordsDoNotMatch);

        var hashedNewPassword = HashGenerator.Hash(userChangePasswordDto.NewPassword);
        user.Password = hashedNewPassword;

        return await _userService.UpdateAsync(user, cancellationToken);
    }
    public async Task<IActionResponse<Guid>> UpdateWalletAsync(Guid userId, UserUpdateWalletDto userUpdateWalletDto, CancellationToken cancellationToken = default)
    {
        var userGetResponse = await _userService.GetAsync(userId, cancellationToken);
        var user = userGetResponse.Data;
        if (user == null) return new ActionResponse<Guid>(ActionResponseStatusCode.NotFound, AppResource.UserNotFound);

        user.WithdrawalWallet = userUpdateWalletDto.WalletAddress;

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
                Nationality = user.Data.Nationality,
                BirthdayDate = user.Data.BirthdayDate,
                WalletAddress = user.Data.WithdrawalWallet,
                Avatar = "data:image/png;base64," + user.Data.Avatar is null ? _defaultAvatar : Convert.ToBase64String(user.Data.Avatar),
            }
        };

        return result;
    }

    public async Task<IActionResponse<UserTokenDto>> SignupAsync(User user, CancellationToken cancellationToken = default)
    {
        user.Password = HashGenerator.Hash(user.Password);
        user.Avatar = Convert.FromBase64String(_defaultAvatar);
        var addedUser = await _userService.AddAsync(user, cancellationToken);

        if (!addedUser.IsSuccess)
            return new ActionResponse<UserTokenDto>(ActionResponseStatusCode.BadRequest, AppResource.TransactionFailed);
        var usertokens = await GenerateTokenAsync(user.UserId, cancellationToken);
        await _userTokenService.AddUserTokenAsync(user.UserId, usertokens.AccessToken, usertokens.RefreshToken, cancellationToken);
        return new ActionResponse<UserTokenDto>(usertokens);
    }


    public async Task<IActionResponse<bool>> RecoveryPassword(Guid userId, string email)
    {
        var user = await _userService.GetAsync(userId);
        if (user is null) return new ActionResponse<bool>(ActionResponseStatusCode.BadRequest);
        var password = new Random().Next(150000, 999999).ToString();
        user.Data.Password = HashGenerator.Hash(password);

        var dbResult = await _userService.UpdateAsync(user.Data);
        if (!dbResult.IsSuccess)
            return new ActionResponse<bool>(ActionResponseStatusCode.ServerError);

        _emailGatwayAdapter.Send(new EmailDto
        {
            Content = password,
            Receiver = email,
            Subject = "Recovery Password",
            Template = EmailTemplate.ForgetPassword
        });
        return new ActionResponse<bool>();
    }
}