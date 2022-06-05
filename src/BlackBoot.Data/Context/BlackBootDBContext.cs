using BlackBoot.Data.Extensions;
using BlackBoot.Shared.Extentions;

namespace BlackBoot.Data.Context;

public class BlackBootDBContext : DbContext
{
    public BlackBootDBContext() { }
    public BlackBootDBContext(DbContextOptions<BlackBootDBContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().AreUnicode(false);
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.WithdrawalWallet).IsUnique();

        //TODO: this code will be removed
        modelBuilder.Entity<User>().HasData(new User
        {
            UserId = Guid.NewGuid(),
            Fullname = "Admin",
            Email = "Admin@BlackBoot.io",
            Password = HashGenerator.Hash("Adm!nP@ssw0rd"),
            Gender = Domain.Enums.Gender.Male,
            IsActive = true,
            Nationality = "",
            RegistrationDate = DateTime.Now,
        });

        modelBuilder.Entity<Subscription>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.RegisterAllEntities<IEntity>(typeof(User).Assembly);
    }
}

