using Microsoft.Extensions.Configuration;

namespace BlackBoot.Services.Extensions;
public static class ConfigurationExtension
{
    public static string ToAvatarUrl(this IConfiguration configuration, string avatar)
        => $"{configuration.GetSection("AppKeys:AvatarBaseUrl").Value}{avatar}";
}
