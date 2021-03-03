using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Megatron.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public EmailOptions Options { get; set; }
        public EmailSender(IOptions<EmailOptions> emailOptions)
        {
            Options = emailOptions.Value;
        }
        
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        private Task Execute(string optionsSendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(optionsSendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("admin@gcd-gw.com", "Megatron"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
            }
    }
}