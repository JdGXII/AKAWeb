using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace AKAWeb_v01.Classes
{
    public class EmailService
    {
        public string password = WebConfigurationManager.AppSettings.Get("emailPassword");
        public string email = WebConfigurationManager.AppSettings.Get("emailAddress");
        public string smpt_server = WebConfigurationManager.AppSettings.Get("smptServer");
        public string message;
        public string sendTo;
        public string subject;
        public bool isHtml;

        public EmailService() { }

        public EmailService(string message, string sendTo, string subject, bool isHtml)
        {
            this.message = message;
            this.sendTo = sendTo;
            this.subject = subject;
            this.isHtml = isHtml;
    
        }

        public bool sendEmail()
        {


            try
            {
                
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smpt_server);

                mail.From = new MailAddress(email);
                mail.To.Add(sendTo);
                mail.Subject = subject;
                mail.Body = message;

                SmtpServer.Port = 25;
               
                SmtpServer.Credentials = new System.Net.NetworkCredential(email, password);
                //SmtpServer.EnableSsl = true;
                
                //mail.IsBodyHtml = isHtml;

                SmtpServer.Send(mail);

                return true;

            }
            catch (Exception ex)
            {
                return false;
 
            }
        }
    }
}