#nullable disable
namespace BlackBoot.Domain.Entities;

[Table(nameof(Notification))]
public class Notification
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NotificationId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    public Guid? UserId { get; set; }

    public MessageTarget Target { get; set; }
    public MessageType Type { get; set; }

    [Required, MaxLength(2000)]
    public string Message { get; set; }

    public DateTime Date { get; set; }

    public bool IsImportant { get; set; }
}