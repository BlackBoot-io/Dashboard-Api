namespace BlackBoot.Domain.Entities;

[Table(nameof(User), Schema = nameof(EntitySchema.Base))]
public class User : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserId { get; set; }

    public bool IsActive { get; set; }

    public Gender Gender { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(20)]
    public string Nationality { get; set; } = null!;

    [Required, MaxLength(128)]
    public string Email { get; set; }

    [Required, MaxLength(256)]
    public string Password { get; set; }

    [MaxLength(256)]
    public string WithdrawalWallet { get; set; }

    public DateTime RegistrationDate { get; set; }

}