using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace DMS.Service.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("mahmoudbadr29062000@gmail.com", "detd xzwf qjtc yyxl"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage("mahmoudbadr29062000@gmail.com", email, subject, htmlMessage);
            mailMessage.IsBodyHtml = true;

            return client.SendMailAsync(mailMessage);
        }
    }
}
