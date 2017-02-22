using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace artfriks.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
    
        static public IConfigurationRoot Configuration { get; set; }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Artfreaks", "cocospicesindia@gmail.com"));
                emailMessage.To.Add(new MailboxAddress(email, email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = message };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback =
                               delegate (object s, X509Certificate certificate,
                                        X509Chain chain, SslPolicyErrors sslPolicyErrors)
                               { return true; };
                    client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("cocospicesindia@gmail.com", "cocospices01");
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



        public Task SendSmsAsync(string number, string message)
        {
            string longurl = "http://cloud.smsindiahub.in/vendorsms/pushsms.aspx?user=cocospices&password=aeon123&sid=COSPCS&msisdn=" + number + "&msg=" + message + "&fl=0&gwid=2";
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
