using System.Net.Mail;
using System.Net;
using CwRetail.Data.Models;
using System.Net.NetworkInformation;

namespace CwRetail.Api.Helpers
{
    public static class UserContactHelper
    {
        private static readonly SmtpClient _smtp;

        static UserContactHelper()
        {
            _smtp = new SmtpClient();
        }

        public static void SendEmail(this UserVerification userVerification, string title, string htmlString, string host, int port, NetworkCredential credentials)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("atalmalavdework@gmail.com");
                message.To.Add(new MailAddress(userVerification.Email));
                message.Subject = title;
                message.IsBodyHtml = true; 
                message.Body = htmlString;
                _smtp.Port = port;
                _smtp.Host = host; 
                _smtp.EnableSsl = true;
                _smtp.UseDefaultCredentials = false;
                _smtp.Credentials = credentials;
                _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                _smtp.Send(message);
            }
            catch (Exception e) 
            { 
                
            }
        }

        public static void SendSms(this UserVerification userVerification, string title, string plainString)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("CwRetailBot");

                message.To.Add(new MailAddress($"{userVerification.Phone}@txt.att.net"));

                message.Subject = title;
                message.Body = plainString;

                _smtp.Send(message);
            }
            catch (Exception e)
            {

            }
        }
    }
}
