using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProductFocus.Services
{
    public class EmailService : IEmailService
    {
        public void send()
        {
            try
            {
                //MailMessage mailMessage = new MailMessage();
                //MailAddress fromAddress = new MailAddress("banerjee.tapas@gmail.com");
                //mailMessage.From = fromAddress;
                //mailMessage.To.Add("banerjee.tapas@in.ibm.com");
                //mailMessage.Body = "This is Testing Email Without Configured SMTP Server";
                //mailMessage.IsBodyHtml = true;
                //mailMessage.Subject = " Testing Email";
                //SmtpClient smtpClient = new SmtpClient
                //{
                //    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                //    PickupDirectoryLocation = @"C:\My Work\Product Focus"
                //};
                //smtpClient.Host = "localhost";
                //smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
            }
        }
    }
}
