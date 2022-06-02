using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlackBoot.Services.Implementations;

public class UserJwtTokenFactory : IUserJwtTokenFactory
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUsersService _usersservice;
    public UserJwtTokenFactory(IUsersService usersservice, IConfiguration configuration)
    {
        _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        _usersservice = usersservice;
    }

    public async Task<UserTokenDto> GenerateTokenAsync(Guid userId)
    {
        var user = await _usersservice.GetByIdAsync(userId, default);
        var accessToken = CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Email,user.Email)
            }, _jwtSettings.AccessTokenExpirationMinutes);

        var refreshToken = CreateToken(new List<Claim>
            {
               new Claim("AccessToken",accessToken)

            }, _jwtSettings.RefreshTokenExpirationMinutes);
        var result = new UserTokenDto()
        {
            AccessToken = accessToken,
            AccessTokenExpireTime = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            RefreshToken = refreshToken,
            RefreshTokenExpireTime = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationMinutes)
        };

        return result;
    }

    public ClaimsPrincipal ReadToken(string token)
    {
        var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var issuerSigningKey = new SymmetricSecurityKey(secretKey);

        var encryptionkey = Encoding.UTF8.GetBytes(_jwtSettings.EncryptionKey);
        var tokenDecryptionKey = new SymmetricSecurityKey(encryptionkey);


        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            TokenDecryptionKey = tokenDecryptionKey,
            IssuerSigningKey = issuerSigningKey
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.Aes256KW, StringComparison.InvariantCultureIgnoreCase)) return null;

        return principal;
    }

    public bool ValidateToken(string token)
    {
        var validatedToken = ReadToken(token);
        return validatedToken != null;
    }

    private string CreateToken(List<Claim> claims, int expiresTimeMinutes)
    {

        var tokenHandler = new JwtSecurityTokenHandler();

        var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var issuerSigningKey = new SymmetricSecurityKey(secretKey);

        var encryptionkey = Encoding.UTF8.GetBytes(_jwtSettings.EncryptionKey);
        var tokenDecryptionKey = new SymmetricSecurityKey(encryptionkey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(expiresTimeMinutes),
            SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            EncryptingCredentials = new EncryptingCredentials(tokenDecryptionKey,
                                                            SecurityAlgorithms.Aes256KW,
                                                            SecurityAlgorithms.Aes256CbcHmacSha512)
        };
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
