namespace BlackBoot.Domain.Dtos
{
    public class UserUpdateWalletDto
    {
        [Required]
        public string WalletAddress { get; set; }
    }
}