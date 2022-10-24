﻿using System.Net.Mail;
using System.Net;
using CwRetail.Data.Models;
using System.Net.NetworkInformation;

namespace CwRetail.Api.Helpers
{
    public static class EmailHelper
    {
        private static readonly SmtpClient _smtp;

        static EmailHelper()
        {
            _smtp = new SmtpClient();
        }

        public static void SendEmail(this UserVerification userVerification, string htmlString, string host, int port, NetworkCredential credentials)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("atalmalavdework@gmail.com");
                message.To.Add(new MailAddress(userVerification.Email));
                message.Subject = "Verify CwRetail account";
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
    }
}
