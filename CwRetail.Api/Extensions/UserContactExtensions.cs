using System.Net.Mail;
using System.Net;
using CwRetail.Data.Models;
using System.Net.NetworkInformation;
using CwRetail.Data.Enumerations;
using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;

namespace CwRetail.Api.Extensions
{
    public static class UserContactExtensions
    {
        public static void SendEmail(this UserVerification userVerification, string host, int port, bool useSsl, string username, string password, string title, string body)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(username);
                    message.Subject = title;
                    message.Body = body;
                    message.To.Add(new MailAddress(userVerification.Email));
                    message.IsBodyHtml = true;
                    smtp.Port = port;
                    smtp.Host = host;
                    smtp.EnableSsl = useSsl;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(username, password);
                    smtp.Send(message);
                }
            }
            catch (Exception e) 
            { 
                
            }
        }

        public static void SendSms(this UserVerification userVerification, string smsApiKey, string sender, string body)
        {
            try
            {
                string message = HttpUtility.UrlEncode(body);

                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("https://api.txtlocal.com/send/", new NameValueCollection()
                    {
                        {"apikey" , smsApiKey},
                        {"numbers" , userVerification.Phone.Replace("+","")},
                        {"message" , message},
                        {"sender" , sender}
                    });

                    string result = Encoding.UTF8.GetString(response);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
