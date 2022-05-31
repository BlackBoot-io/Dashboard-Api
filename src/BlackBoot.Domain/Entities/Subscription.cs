#nullable disable
namespace BlackBoot.Domain.Entities;

[Table(nameof(Subscription), Schema = nameof(EntitySchema.Base))]
public class Subscription : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubscriptionId { get; set; }

    [Required, MaxLength(120), EmailAddress]
    public string Email { get; set; }

    public DateTime Date { get; set; }
}