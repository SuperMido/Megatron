using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Megatron.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailOptions> emailOptions)
        {
            Options = emailOptions.Value;
        }

        public EmailOptions Options { get; set; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        private async Task<Response> Execute(string optionsSendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(optionsSendGridKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("megatrongwdn@gmail.com", "Megatron"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);
            return await client.SendEmailAsync(msg);
        }
    }
}