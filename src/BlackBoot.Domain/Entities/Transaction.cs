using System.Text.Json.Serialization;

namespace BlackBoot.Domain.Entities;

[Table(nameof(Transaction), Schema = nameof(EntitySchema.Payment))]
public class Transaction : IEntity
{
    public Guid TransactionId { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey(nameof(CrowdSaleScheduleId))]
    public CrowdSaleSchedule CrowdSaleSchedule { get; set; }

    public int? CrowdSaleScheduleId { get; set; }

    public BlockchainNetwork Network { get; set; }

    /// <summary>
    /// UsdtAmount
    /// </summary>
    public int UsdtAmount { get; set; }

    /// <summary>
    /// Crypto Amount
    /// </summary>
     [Column(TypeName = "decimal(21,9)")]
    public decimal CryptoAmount { get; set; }

    public DateTime Date { get; set; }
    public int TokenCount { get; set; }
    public int BonusCount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }

    public DateTime? ConfirmDate { get; set; }

    [MaxLength(256)]
    public string WalletAddress { get; set; }

    [MaxLength(256)]
    public string TxId { get; set; }

    [NotMapped]
    public int TotalToken => TokenCount + BonusCount;
}