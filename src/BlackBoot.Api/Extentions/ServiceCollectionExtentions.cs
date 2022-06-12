#nullable disable
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BlackBoot.Api.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddBlockBootDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlackBootDBContext>((IServiceProvider serviceProvider, DbContextOptionsBuilder options) =>
        {
            options.UseSqlServer(configuration["BlckBootDbContext"]);
        });
    }
    public static void AddBlackBootAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddAuthorization();
        _ = services.AddAuthentication(options =>
          {
              options.DefaultChallengeScheme = "Bearer";
              options.DefaultSignInScheme = "Bearer";
              options.DefaultAuthenticateScheme = "Bearer";
          })
        .AddGoogle(options =>
        {
            var googleAuthNSection = configuration.GetSection("Authentication:Google");

            options.ClientId = googleAuthNSection["ClientId"];
            options.ClientSecret = googleAuthNSection["ClientSecret"];
            //options.CallbackPath = "";
        })
        .AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidateIssuer = true,
                ValidAudience = jwtSettings.Audience,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.EncryptionKey))
            };

            static async Task validate(TokenValidatedContext context)
            {

                var token = ((JwtSecurityToken)context.SecurityToken).RawData;
                var userTokenService = context.HttpContext.RequestServices.GetRequiredService<IUserJwtTokenService>();
                var userId = context?.Principal?.Identity?.GetUserIdAsGuid();
                if (userId is null || userId == Guid.Empty)
                {
                    context?.Fail(AppResource.InvalidUser);
                    return;
                }
                var validate = await userTokenService.VerifyTokenAsync(userId.Value, token, context.HttpContext.RequestAborted);
                if (!validate.Data)
                {
                    context.Fail(AppResource.InvalidUser);
                    return;
                }
                context.HttpContext.User = context.Principal;
            }

            cfg.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    return validate(context);
                }
            };
            cfg.SecurityTokenValidators.Add(new RequireEncryptedTokenHandler());
        });
    }
}