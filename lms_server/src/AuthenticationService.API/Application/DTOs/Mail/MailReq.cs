namespace AuthenticationService.API.Application.DTOs.Mail;
public record MailRequest
{
    public required string ToEmail { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}