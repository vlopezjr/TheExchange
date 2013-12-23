using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace TheExchange.MVC.Utilities
{
    public static class Email
    {

        public static void Send(string from, string to, string subject, string body)
        {
            var mail = new MailMessage(from, to, subject, body);
            
            SmtpClient smtp = new SmtpClient("mail.poshred.com"); 
          
            NetworkCredential Credentials = new NetworkCredential("postmaster@poshred.com", "password"); 
            smtp.Credentials = Credentials;
            smtp.Send(mail); 
        }

        public static void Send(MailAddress fromAddress, MailAddress toAddress, MailAddress ccAddress, MailAddress bccAddress, string subject, string body)
        {
            var message = new MailMessage(fromAddress, toAddress);
            //{
            //    //Bcc = { bccAddress },
            //    Subject = subject,
            //    Body = body
            //};
            if (bccAddress != null)
                message.Bcc.Add(bccAddress);
            if (ccAddress != null)
                message.CC.Add(ccAddress);

            message.Subject = subject;
            message.Body = body;

            //var client = new SmtpClient("localhost");
            var client = new SmtpClient();
            //#if DEBUG
            //client.Host = "localhost";
            //#endif
            client.Send(message);
        }

        public static void SendTemplatedEmail(string from, string to, string subject, string templatePath)
        {
            var message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = GetEmailBody(templatePath);

            if (message.Body != "")
            {
                message.IsBodyHtml = true;
                var client = new SmtpClient();
                //client.Host = "localhost";
                client.Send(message);
            }
        }

        public static string GetEmailBody(string templatePath)
        {
            System.Net.WebClient objWebClient = new System.Net.WebClient();
            byte[] aRequestedHTML = null;
            string strRequestedHTML = null;

            aRequestedHTML = objWebClient.DownloadData(templatePath);
            System.Text.UTF8Encoding objUTF8 = new System.Text.UTF8Encoding();
            strRequestedHTML = objUTF8.GetString(aRequestedHTML);

            return strRequestedHTML;
        }


        public static void NewAppointmentNotify()
        {

        }
                
    }
}