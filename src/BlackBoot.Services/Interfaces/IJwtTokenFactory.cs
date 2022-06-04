using System.Security.Claims;

namespace BlackBoot.Services.Interfaces;

public interface IJwtTokenFactory : IScopedDependency
{
    (string Token, int TokenExpirationMinutes) CreateToken(List<Claim> claims, JwtTokenType tokenType);
    ClaimsPrincipal ReadToken(string token);
}