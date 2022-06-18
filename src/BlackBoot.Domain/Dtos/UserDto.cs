namespace BlackBoot.Domain.Dtos;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string Nationality { get; set; } = null!;
    public DateTime BirthdayDate { get; set; }
    public string WithdrawalWallet { get; set; }
}
