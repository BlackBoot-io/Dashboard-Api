namespace BlackBoot.Domain.Dtos;
public class EmailDto
{
    public EmailTemplate Template { get; set; }
    public string Receiver { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}

