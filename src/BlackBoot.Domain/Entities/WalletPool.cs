#nullable disable
namespace BlackBoot.Domain.Entities;

[Table(nameof(WalletPool), Schema = nameof(EntitySchema.Base))]
public class WalletPool : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WalletPoolId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    public Guid? UserId { get; set; }

    public BlockchainNetwork Network { get; set; }

    [Required]
    [MaxLength(256)]
    public string Address { get; set; }

    public bool IsUsed { get; set; }
}