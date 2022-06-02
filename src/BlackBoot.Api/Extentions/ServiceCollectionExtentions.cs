using BlackBoot.Data.Context;
using BlackBoot.Services.Resources;
using BlackBoot.Shared.Core;
using BlackBoot.Shared.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlackBoot.Api.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddBlockBootDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlackBootDBContext>((IServiceProvider serviceProvider, DbContextOptionsBuilder options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("BlckBootDbContext"));
        });
    }
    public static void AddBlackBootAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = "Bearer";
            options.DefaultSignInScheme = "Bearer";
            options.DefaultAuthenticateScheme = "Bearer";
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

            async Task validate(TokenValidatedContext context)
            {

                var token = ((JwtSecurityToken)context.SecurityToken).RawData;
                var userTokenService = context.HttpContext.RequestServices.GetRequiredService<IUserJwtTokensService>();
                var userId = context.Principal.Identity.GetUserIdAsGuid();
                if (userId == Guid.Empty)
                {
                    context.Fail(AppResource.InvalidUser);
                    return;
                }
                var validate = await userTokenService.VerifyTokenAsync(userId.Value, token, context.HttpContext.RequestAborted);
                if (!validate)
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


public class RequireEncryptedTokenHandler : JwtSecurityTokenHandler
{
    public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token));

        if (validationParameters == null)
            throw new ArgumentNullException(nameof(validationParameters));

        if (token.Length > MaximumTokenSizeInBytes)
            throw new ArgumentException(
                $"IDX10209: token has length: '{token.Length}' which is larger than the MaximumTokenSizeInBytes: '{MaximumTokenSizeInBytes}'.");

        var strArray = token.Split(new[] { '.' }, 6);
        if (strArray.Length == 5)
            return base.ValidateToken(token, validationParameters, out validatedToken);

        throw new SecurityTokenDecryptionFailedException();
    }
}