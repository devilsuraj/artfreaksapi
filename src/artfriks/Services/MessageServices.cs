using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace artfriks.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        //const string DOMAIN = "sandbox702392bd182f42ca9afde1f2db9e30cb.mailgun.org";
        //const string API_KEY = "key-90610484192486ed24c74808d5ee613f";

        const string DOMAIN = "mail.artfreaksglobal.com";
        const string API_KEY = "key-62a55fa32f128ed01c3b126dd7a37c99";

        static public IConfigurationRoot Configuration { get; set; }
        public Task SendEmailAsync1(string email, string subject, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Artfreaks", "support@artfreaksglobal.com"));
                emailMessage.To.Add(new MailboxAddress(email, email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = message };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback =
                               delegate (object s, X509Certificate certificate,
                                        X509Chain chain, SslPolicyErrors sslPolicyErrors)
                               { return true; };
                    client.Connect("smtp.zoho.com", 465, MailKit.Security.SecureSocketOptions.Auto);
                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("support@artfreaksglobal.com", "art@2017!");
                    client.Send(emailMessage);
                    client.Disconnect(true);
                    return Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }

        }


        public async Task SendEmailAsync(string to, string subject, string message)
        {
           
            var from = new EmailAddress("support@artfreaksglobal.com", "Artfreaks Global");
            var tos = new EmailAddress(to, to);
            var msg = MailHelper.CreateSingleEmail(from, tos, subject, message, message);
            var response = await client.SendEmailAsync(msg);
        }

        public Task SendSmsAsync(string number, string message)
        {
            string longurl = "http://cloud.smsindiahub.in/vendorsms/pushsms.aspx?user=artfreaks&password=*artfreaks*@2017!&sid=ARTFGL&msisdn=" + number + "&msg=" + message + "&fl=0&gwid=2";
            try
            {
                using (var client = new HttpClient())
                {
                    var responseString = client.GetAsync(longurl).Result;
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            { return Task.FromException(ex); }
        }
    }
}
