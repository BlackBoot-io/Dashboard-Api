#nullable disable
namespace BlackBoot.Domain.Entities;

[Table(nameof(Transaction), Schema = "Payment")]
public class Transaction : IEntity
{
    public Guid TransactionId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    public Guid UserId { get; set; }

    [ForeignKey(nameof(CrowdSaleScheduleId))]
    public CrowdSaleSchedule CrowdSaleSchedule { get; set; }
    public int CrowdSaleScheduleId { get; set; }

    public BlockchainNetwork Network { get; set; }
    /// <summary>
    /// UsdtAmount
    /// </summary>
    public int UsdtAmount { get; set; }
    /// <summary>
    /// Crypto Amount
    /// </summary>
    public int CryptoAmount { get; set; }
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
    public int TotalToken => TotalToken + BonusCount;
}