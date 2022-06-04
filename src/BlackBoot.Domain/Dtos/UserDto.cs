namespace BlackBoot.Domain.Dtos;

public class UserDto
{
    public string Email { get; set; }
    public string FirstName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string Nationality { get; set; } = null!;
}
