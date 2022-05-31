using BlackBoot.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlackBoot.Api.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddBlockinoContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlockinoDBContext>((IServiceProvider serviceProvider, DbContextOptionsBuilder options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Blockino"));
        });
    }
}
