namespace BlackBoot.Domain.Dtos;

public class TransactionDto
{
    public Guid TransactionId { get; set; }
    public string WalletAddress { get; set; }
    public BlockchainNetwork Network { get; set; }
    public string Qr { get; set; }
}
