using System;
using System.Net.Mail;

namespace ProductFocus.Services
{
    public class EmailService : IEmailService
    {
        public void Send(string emailBody, string email)
        {
            try
            {
                MailMessage mail = new();
                mail.To.Add(email);
                mail.From = new MailAddress("info@intelli-h.com");
                mail.Subject = "Invitation to join Product Focus.";
                mail.Body = emailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new("smtp.gmail.com", 587);
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
