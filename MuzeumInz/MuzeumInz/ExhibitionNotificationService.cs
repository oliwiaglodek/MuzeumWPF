using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuzeumInz;

namespace MuzeumInz
{
    public class ExhibitionNotificationService
    {
        private readonly EmailServices _emailService; // wysyłaka maili
        private readonly DbConnect _dbConnect; 

        public ExhibitionNotificationService()
        {
            _emailService = new EmailServices(); 
            _dbConnect = new DbConnect(); 
        }
        // funkcja wysyłająca powiadomienie email o utworzonej wystawie
        public void SendNotificationEmail(AddExhibitions exhibition)
        {
            string recipientEmail = GetLoggedUserEmail(); // ustaw adres email odbiorcy
            string subject = "Powiadomienie o nadchodzącej wystawie";
            string body = $"Wystawa '{exhibition.Name}' rozpocznie się {exhibition.StartDate.Value.ToString("dd.MM.yyyy")}. " +
                          $"Miejsce: {exhibition.Location}. Przygotuj się!";

            _emailService.SendEmail(recipientEmail, subject, body);
        }

        // powiadomienie email o utworzeniu konta
        public void SendAccountCreationNotification(string newUserEmail)
        {
            string recipientEmail = newUserEmail; // adres email nowego użytkownika
            string subject = "Witamy w systemie Muzeum!";
            string body = $"Dziękujemy za założenie konta w naszym systemie! " +
                          $"Teraz możesz korzystać z naszego systemu do zarządzania wystawami i eksponatami. " +
                          $"Życzymy miłego korzystania!";

            _emailService.SendEmail(recipientEmail, subject, body);
        }
        // metoda do pobierania maila zalogowanego użytkownika
        private string GetLoggedUserEmail()
        {
            string query = "SELECT user_id FROM current_user LIMIT 1";
            using (var command = new SQLiteCommand(query, _dbConnect.GetConnection()))
            {
                var result = command.ExecuteScalar();
                return result != null ? result.ToString() : null;
            }
        }

    }
}
