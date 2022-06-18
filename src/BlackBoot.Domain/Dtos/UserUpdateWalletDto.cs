namespace BlackBoot.Domain.Dtos
{
    public class UserUpdateWalletDto
    {
        [Required]
        public string WithdrawalWallet { get; set; }
    }
}