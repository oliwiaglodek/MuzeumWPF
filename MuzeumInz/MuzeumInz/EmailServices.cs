using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace MuzeumInz
{
    public class EmailServices
    {
        private SmtpClient _client;

        public EmailServices()
        {
            // Konfiguracja serwera SMTP
            _client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // Często używany port dla SMTP
                Credentials = new NetworkCredential("systemmuzeum@gmail.com", "ewuv obqk lalm tlxe"), //Inzynierka!0
                EnableSsl = true
            };
        }

        public void SendEmail(string recipient, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("systemmuzeum@gmail.com"), //adres nadawcy
                Subject = subject,
                Body = body
            };
            mailMessage.To.Add(recipient); //adres odbiorcy

            try
            {
                _client.Send(mailMessage);
                Console.WriteLine("Email wysłany pomyślnie!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd przy wysyłaniu emaila: " + ex.Message);
            }
        }
    }
}
