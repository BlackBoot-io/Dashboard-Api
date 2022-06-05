using System.Security.Claims;

namespace BlackBoot.Services.Interfaces;

public interface IJwtTokenFactory : IScopedDependency
{
    IActionResponse<(string Token, int TokenExpirationMinutes)> CreateToken(List<Claim> claims, JwtTokenType tokenType);
    IActionResponse<ClaimsPrincipal> ReadToken(string token);
}