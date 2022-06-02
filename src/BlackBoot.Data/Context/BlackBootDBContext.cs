using BlackBoot.Data.Extensions;

namespace BlackBoot.Data.Context;

public class BlackBootDBContext : DbContext
{
    public BlackBootDBContext() { }
    public BlackBootDBContext(DbContextOptions<BlackBootDBContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) => configurationBuilder.DefaultTypeMapping<string>().IsUnicode(false);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.WithdrawalWallet).IsUnique();

        modelBuilder.Entity<Subscription>().HasIndex(x => x.Email).IsUnique();

        modelBuilder.RegisterAllEntities<IEntity>(typeof(User).Assembly);
    }
}

