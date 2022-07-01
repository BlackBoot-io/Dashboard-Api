namespace BlackBoot.Domain.Entities;

[Table(nameof(User), Schema = nameof(EntitySchema.Base))]
public class User : IEntity
{
    public User() => UserId = Guid.NewGuid();

       
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserId { get; set; }

    public bool IsActive { get; set; }

    public Gender Gender { get; set; }

    [Required, MaxLength(150)]
    public string FullName { get; set; } = null!;

    [Required, MaxLength(20)]
    public string Nationality { get; set; } = "USA";

    [Required, MaxLength(128)]
    public string Email { get; set; }

    [Required, MaxLength(256)]
    public string Password { get; set; }

    public string PasswordSalt { get; set; }

    [MaxLength(256)]
    public string WithdrawalWallet { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime? BirthdayDate { get; set; }

    public byte[] Avatar { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<WalletPool> WalletPools { get; set; }
    public ICollection<UserJwtToken> UserJwtTokens { get; set; }
}