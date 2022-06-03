using System.Security.Claims;

namespace BlackBoot.Services.Interfaces;

public interface IUserJwtTokenFactory : ITransientDependency
{
    (string Token, int TokenExpirationMinutes) CreateToken(List<Claim> claims, JwtTokenType tokenType);
    ClaimsPrincipal ReadToken(string token);
}