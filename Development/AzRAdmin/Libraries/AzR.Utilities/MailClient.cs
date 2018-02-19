using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AzR.Utilities
{

    public class MailClient
    {

        public static string Sender = ConfigurationManager.AppSettings["SiteName"];
        private static SmtpSection _smtp = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        public static void Send(List<string> to, string subject, string body, bool isBodyHtml = true, List<string> cc = null, List<string> bcc = null)
        {
            MailMessage message = new MailMessage();

            foreach (string address in to)
            {
                message.To.Add(address);
            }

            if (cc != null)
            {
                foreach (string address in cc)
                {
                    message.CC.Add(address);
                }
            }
            if (bcc != null)
            {
                foreach (string address in bcc)
                {
                    message.Bcc.Add(address);
                }
            }


            message.Subject = subject;

            message.Body = body;

            message.IsBodyHtml = isBodyHtml;

            Send(message);
        }

        public static void Send(string to, string subject, string body, bool isBodyHtml = true)
        {
            Send(new List<string> { to }, subject, body, isBodyHtml);
        }
        /// <summary>
        /// Sends an email synchronously
        /// </summary>
        private static void Send(MailMessage mail)
        {
            using (var client = new SmtpClient())
            {
                mail.From = new MailAddress(_smtp.From, Sender);
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                client.Send(mail);
            }
        }


        public static async Task SendAsync(List<string> to, string subject, string body, bool isBodyHtml = true, List<string> cc = null, List<string> bcc = null)
        {
            MailMessage message = new MailMessage();

            foreach (string address in to)
            {
                message.To.Add(address);
            }

            if (cc != null)
            {
                foreach (string address in cc)
                {
                    message.CC.Add(address);
                }
            }
            if (bcc != null)
            {
                foreach (string address in bcc)
                {
                    message.Bcc.Add(address);
                }
            }


            message.Subject = subject;

            message.Body = body;

            message.IsBodyHtml = isBodyHtml;

            await SendAsync(message);
        }

        public static async void SendAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendAsync(new List<string> { to }, subject, body, isBodyHtml);
        }

        /// <summary>
        /// Sends an email asynchronously
        /// </summary>
        private static async Task SendAsync(MailMessage mail)
        {
            using (var smtp = new SmtpClient())
            {
                mail.From = new MailAddress(_smtp.From, Sender);
                await smtp.SendMailAsync(mail);
                smtp.SendCompleted += client_SendCompleted;
            }
        }

        static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error != null)
            {
                throw e.Error;
            }
        }


    }
}
