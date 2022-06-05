namespace BlackBoot.Domain.Dtos;

public class UserChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}
