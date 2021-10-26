using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                /*MailMessage mailMessage = new MailMessage();
                //MailAddress fromAddress = new MailAddress("admin@dumanhilltechnologies.com");
                MailAddress fromAddress = new MailAddress("info@intelli-h.com");
                MailAddress toAddress = new MailAddress(email);
                //const string fromPassword = "Dewsacademy@1a";
                const string fromPassword = "reversal@1";
                const string subject = "Invitation to join Product Focus.";

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 578,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = emailBody
                })
                {
                    smtpClient.Send(message);
                }*/

                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("info@intelli-h.com");
                mail.Subject = "Invitation to join Product Focus.";
                mail.Body = emailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("info@intelli-h.com", "reversal@1");
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
