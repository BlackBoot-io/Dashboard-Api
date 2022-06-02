using System.Security.Claims;

namespace BlackBoot.Services.Interfaces;

public interface IUserJwtTokenFactory : ITransientDependency
{
    Task<UserTokenDto> GenerateTokenAsync(Guid userId);
    ClaimsPrincipal ReadToken(string token);
}
