using BlackBoot.Data.Extensions;

namespace BlackBoot.Data.Context;

public class BlockinoDBContext : DbContext
{
    public BlockinoDBContext() { }
    public BlockinoDBContext(DbContextOptions<BlockinoDBContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) => configurationBuilder.DefaultTypeMapping<string>().IsUnicode(false);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.WithdrawalWallet).IsUnique();

        modelBuilder.Entity<Subscription>().HasIndex(x => x.Email).IsUnique();

        modelBuilder.RegisterAllEntities<IEntity>(typeof(User).Assembly);
    }
}

