using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace BlackBoot.Services.ExternalAdapter;

public class EmailGatwayAdapter
{
    private static bool SendWithGmail(IConfiguration configuration, EmailDto email)
    {
        try
        {
            #region Get Mail Template
            var template = @"Test Email Template";
            #endregion

            var bodyBuilder = new BodyBuilder();
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("inbox", configuration["EmailSettings:Username"]));
            mailMessage.To.Add(new MailboxAddress("inbox", email.Receiver));
            mailMessage.Subject = email.Subject;

            bodyBuilder.HtmlBody = template.Replace("@@@", email.Content);

            mailMessage.Body = bodyBuilder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect(configuration["EmailSettings:Server"], int.Parse(configuration["EmailSettings:Port"]), bool.Parse(configuration["EmailSettings:EnableSsl"]));
                smtpClient.Authenticate(configuration["EmailSettings:Username"], configuration["EmailSettings:Password"]);
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
    private static (bool, string) SendWithSendinBlue(EmailDto email, IConfiguration configuration)
    {
        try
        {
            var apiInstance = new TransactionalEmailsApi();
            if (!Configuration.Default.ApiKey.ContainsKey("api-key"))
                Configuration.Default.ApiKey.Add("api-key", configuration["EmailSettings:SendinBlueApiKey"]);

            SendSmtpEmailSender emailSender = new(configuration["EmailSettings:SenderName"], configuration["EmailSettings:Sender"]);
            SendSmtpEmailTo to = new(email.Receiver, email.Receiver);
            List<SendSmtpEmailTo> emailReceiver = new() { to };

            string Subject = email.Subject;
            string TextContent = null;
            string HtmlContent = email.Content;

            SendSmtpEmail smtpEmail = new(
                sender: emailSender,
                to: emailReceiver,
                bcc: null,
                cc: null,
                htmlContent: HtmlContent,
                textContent: TextContent,
                subject: Subject,
                replyTo: null,
                attachment: null,
                headers: null,
                templateId: null,
                _params: null,
                messageVersions: null,
                tags: null);

            var sendMailResult = apiInstance.SendTransacEmail(smtpEmail);
            if (!string.IsNullOrEmpty(sendMailResult.MessageId))
                return (true, sendMailResult.MessageId?.Split("@")[0]?.Substring(1));
            else
                return (false, string.Empty);
        }
        catch (SmtpCommandException ex)
        {
            return (false, $"SmtpException {ex.Message}");
        }
        catch (Exception e)
        {
            return (false, $"Exception {e.Message}");
        }
    }

    public static IActionResponse<bool> Send(EmailDto email, IServiceProvider serviceProvider)
    {
        ActionResponse<bool> response = new ();
        try
        {
            var configuration = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            var sendResult = SendWithSendinBlue(email, configuration);
            if (sendResult.Item1)
            {
                response.Data = true;
                response.IsSuccess = true;
                response.Message = $"Status: Success | ID: {sendResult.Item2}";
            }
            else
            {
                response.Data = false;
                response.Message = $"Status: Fail | ID: {sendResult.Item2}";
            }

            return response;
        }
        catch (Exception e)
        {
            response.Message = $"Exception {e.Message}";
            return response;
        }
    }
}