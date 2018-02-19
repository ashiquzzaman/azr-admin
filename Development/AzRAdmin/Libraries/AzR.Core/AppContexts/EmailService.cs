using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AzR.Core.AppContexts
{
    public class EmailService : IIdentityMessageService
    {
        public static string Sender = ConfigurationManager.AppSettings["SiteName"];
        private static SmtpSection _smtp = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");


        public Task SendAsync(IdentityMessage message)
        {
            return ConfigSendGridasync(message);
        }

        private async Task ConfigSendGridasync(IdentityMessage message)
        {
            var email = new MailMessage(new MailAddress(_smtp.From, Sender),
                new MailAddress(message.Destination))
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };
            using (var client = new SmtpClient())
            {
                await client.SendMailAsync(email);
            }
        }
    }
}