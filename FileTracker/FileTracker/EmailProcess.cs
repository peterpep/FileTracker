using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace TaskScheduler
{
    class EmailProcess
    {
        private MailMessage mail;
        private SmtpClient client;
        

        public EmailProcess(string emailFrom, string passwordFrom, string sendTo)
        {
            mail = new MailMessage(emailFrom, sendTo);
            client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.Credentials = new NetworkCredential(emailFrom, passwordFrom);
            
        }

        public void SendMail(string subject, string message)
        {

            ////////
            var tempstring = mail.To.ToString().Trim();

            var addresses = tempstring.Split(',');

            mail.To.Clear();
            foreach (var sendingTo in addresses)
            {
                mail.To.Add(sendingTo);
            }
            ///////

            mail.Subject = subject;
            mail.Body = message;
            mail.BodyEncoding = Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mail);
        }
        
        
    }
}
