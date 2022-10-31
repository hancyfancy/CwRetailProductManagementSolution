using System.Net.Mail;
using System.Net;
using CwRetail.Data.Models;
using System.Net.NetworkInformation;
using CwRetail.Data.Enumerations;

namespace CwRetail.Api.Extensions
{
    public static class UserContactExtensions
    {
        private static readonly SmtpClient _smtp;

        static UserContactExtensions()
        {
            _smtp = new SmtpClient();
        }

        public static void Send(this UserVerification userVerification, UserContactTypeEnum userContactTypeEnum, string host, int port, bool useSsl, string username, string password, string title, string body)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(username);
                message.Subject = title;
                message.Body = body;

                if (userContactTypeEnum == UserContactTypeEnum.Email)
                {
                    message.To.Add(new MailAddress(userVerification.Email));
                    message.IsBodyHtml = true;
                }
                else if (userContactTypeEnum == UserContactTypeEnum.Phone)
                {
                    message.To.Add(new MailAddress($"{userVerification.Phone}@txt.att.net"));
                    message.IsBodyHtml = false;
                }

                _smtp.Port = port;
                _smtp.Host = host; 
                _smtp.EnableSsl = useSsl;
                _smtp.UseDefaultCredentials = false;
                _smtp.Credentials = new NetworkCredential(username, password);
                _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                _smtp.Send(message);
            }
            catch (Exception e) 
            { 
                
            }
        }
    }
}
