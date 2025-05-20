using AuthenticationService.API.Application.DTOs.Mail;
using AuthenticationService.API.Application.Shared.Constant;

namespace AuthenticationService.API.Core.Mail;
public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
public class MailService : IMailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    public MailService()
    {
        _smtpServer = MailKitConst.SmtpServer;
        _smtpPort = MailKitConst.SmtpPort;
        _smtpUsername = MailKitConst.SmtpUsername;
        _smtpPassword = MailKitConst.SmtpPassword;
    }

    public Task SendEmailAsync(MailRequest mailRequest)
    {
        var mimeMessage = new MimeKit.MimeMessage();
        mimeMessage.From.Add(new MimeKit.MailboxAddress("DotNet Template", _smtpUsername));
        mimeMessage.To.Add(new MimeKit.MailboxAddress("Receiver Name", mailRequest.ToEmail));

        mimeMessage.Subject = mailRequest.Subject;

        var bodyBuilder = new MimeKit.BodyBuilder
        {
            HtmlBody = mailRequest.Body
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_smtpUsername, _smtpPassword);
            client.Send(mimeMessage);
            client.Disconnect(true);
        }
        return Task.CompletedTask;
    }
}