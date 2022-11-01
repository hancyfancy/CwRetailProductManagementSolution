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
        public static void Send(this UserVerification userVerification, UserContactTypeEnum userContactTypeEnum, string host, int port, bool useSsl, string username, string password, string title, string body)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
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
                        message.To.Add(new MailAddress($"{userVerification.Phone.Replace("+", "")}@txt.att.net"));
                        message.IsBodyHtml = false;
                    }

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
                using (HttpClient client = new HttpClient())
                {
                    NameValueCollection nameValueCollection = new NameValueCollection()
                    {
                        {"apikey" , smsApiKey},
                        {"numbers" , userVerification.Phone.Replace("+","")},
                        {"message" , body},
                        {"sender" , sender}
                    };

                    StringContent content = new StringContent(nameValueCollection.ToJson(), Encoding.UTF8, "application/json");

                    Uri uri = new Uri($"https://api.txtlocal.com/send/");

                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
                    {
                        Content = content,
                    };

                    HttpResponseMessage responseMessage = client.Send(httpRequestMessage);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
