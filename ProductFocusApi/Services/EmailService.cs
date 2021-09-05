using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProductFocus.Services
{
    public class EmailService : IEmailService
    {
        public void send(string emailBody, string email)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                MailAddress fromAddress = new MailAddress("mail.address@gmail.com");
                mailMessage.From = fromAddress;
                mailMessage.To.Add(email);
                mailMessage.Body = emailBody;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = " Testing Email";
                SmtpClient smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = @"C:\My Work\Product Focus"
                };
                smtpClient.Host = "localhost";
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
            }
        }
    }
}
