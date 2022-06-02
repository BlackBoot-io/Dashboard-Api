using BlackBoot.Data.Context;
using Microsoft.EntityFrameworkCore;

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
}
