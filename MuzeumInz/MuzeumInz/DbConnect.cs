using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Media.Imaging;
using System.Data.Entity;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.ComTypes;
using System.Data.Common;

namespace MuzeumInz
{
    public class DbConnect : IDisposable
    {
        private string connectionString;
        private SQLiteConnection _connection;

        public DbConnect()
        {
            // Pobieramy connectionString z pliku konfiguracyjnego
            connectionString = ConfigurationManager.ConnectionStrings["Muzeum"].ConnectionString;
            _connection = new SQLiteConnection(connectionString);
            _connection.Open(); // Otwieramy połączenie
        }
        // Implementacja IDisposable do poprawnego zarządzania zasobami
        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close(); // Zamykamy połączenie do bazy danych

                _connection.Dispose(); // Zwalniamy zasoby
                _connection = null;
            }
        }

        public DataTable GetData(SQLiteCommand command)
        {
            DataTable dt = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                command.Connection = connection;
                connection.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(dt);
            }

            return dt;
        }
        public void ExecuteQuery(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Zwracamy istniejące połączenie, aby inne metody mogły z niego korzystać
        public SQLiteConnection GetConnection()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            return _connection; 
        }

        //sprawdzenie czy użytkonik istanieje w bazie - do rejestracji
        public bool CheckUserExist(string email)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM users WHERE email = @email";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);

                int userCount = Convert.ToInt32(command.ExecuteScalar());
                return userCount > 0;
            }
        }

        //dodanie użytkownika
        public void RegisterUser(string email, string password)
        {
            using (var connection = new SQLiteConnection(connectionString)) 
             {
                 connection.Open();
                 string query = "INSERT INTO users (email, password) VALUES (@email, @password)";
                 SQLiteCommand command = new SQLiteCommand(query, connection);
                 command.Parameters.AddWithValue("@email", email);
                 command.Parameters.AddWithValue("@password", password);

                 command.ExecuteNonQuery();
             }
        }
        //ustawia w tymczasowej tabeli użytkownika a raczej jego nazwę - potem można rozszerzyć o dane logowania i itp
        public void SetCurrentUser(string userEmail)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Sprawdź, czy tabela current_user istnieje, jeśli nie, to ją stwórz
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS current_user ( user_id TEXT);
                    DELETE FROM current_user; -- usuń istniejącego użytkownika
                 ";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Dodaj nowego użytkownika do tabeli
                string insertUserQuery = "INSERT INTO current_user (user_id) VALUES (@UserEmail)";
                using (var command = new SQLiteCommand(insertUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserEmail", userEmail);
                    command.ExecuteNonQuery();
                }
            }
        }
       
        public void ClearCurrentUser()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Usuń użytkownika z tabeli current_user
                string deleteUserQuery = "DELETE FROM current_user";
                using (var command = new SQLiteCommand(deleteUserQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        //wczytaj eksponaty
        public List<AddExhibits> GetExhibits()
        {
            string sql = "SELECT id, name, Description, Year, Category, Author, Origin, Image, Location FROM exhibits;";
            List < AddExhibits > list = new List<AddExhibits>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string description = reader.IsDBNull(2) ? null : reader.GetString(2);
                            int year = reader.GetInt32(3);
                            string category = reader.GetString(4);
                            string author = reader.GetString(5);
                            string origin = reader.GetString(6);
                            byte[] imageBytes = reader.IsDBNull(7) ? null : (byte[])reader["Image"];
                            BitmapImage image = imageBytes == null ? null : convertBytesToBitmap(imageBytes);
                            string location = reader.IsDBNull(8) ? null : reader.GetString(8);

                            AddExhibits exhibits = new AddExhibits(id, name, description, year, category, author, origin, image, location);
                            list.Add(exhibits);
                        }
                    }
                }
            }
            return list;
        }
        //dodaj eksponat
        public void InsertExhibits(AddExhibits addExhibits)
        {

            string sql = @"INSERT INTO exhibits(Name,Description,Year,Category,Author,Origin, Image,Location) 
                    VALUES(@Name,@Description,@Year,@Category,@Author,@Origin,@Image,@Location);";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", addExhibits.Name);
                    command.Parameters.AddWithValue("@Description", addExhibits.Description);
                    command.Parameters.AddWithValue("@Year", addExhibits.Year);
                    command.Parameters.AddWithValue("@Category", addExhibits.Category);
                    command.Parameters.AddWithValue("@Author", addExhibits.Author);
                    command.Parameters.AddWithValue("@Origin", addExhibits.Origin);
                    command.Parameters.AddWithValue("@Image", addExhibits.Image != null ? convertBitmapToBytes(addExhibits.Image) : null); 
                    command.Parameters.AddWithValue("@Location", addExhibits.Location);
                    command.ExecuteNonQuery();
                }
            }
        }
        //Usuń eksponat z bazy
        public void DeleteExhibits(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM exhibits WHERE id = @Id";

                using (var command = new SQLiteCommand(sql,connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            
        }
        // edytuj eksponat
        public void UpdateExhibits(AddExhibits addExhibits)
        {
            string sql = @"UPDATE exhibits 
                    SET name = @Name, 
                    description = @Description, 
                    year = @Year, 
                    category = @Category, 
                    author = @Author, 
                    origin = @Origin,
                    image = @Image, 
                    location = @Location 
                    WHERE id = @Id";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", addExhibits.Name);
                    command.Parameters.AddWithValue("@Description", addExhibits.Description);
                    command.Parameters.AddWithValue("@Year", addExhibits.Year);
                    command.Parameters.AddWithValue("@Category", addExhibits.Category);
                    command.Parameters.AddWithValue("@Author", addExhibits.Author);
                    command.Parameters.AddWithValue("@Origin", addExhibits.Origin);
                    command.Parameters.AddWithValue("@Image", addExhibits.Image != null ? convertBitmapToBytes(addExhibits.Image) : null);
                    command.Parameters.AddWithValue("@Location", addExhibits.Location);
                    command.Parameters.AddWithValue("@Id", addExhibits.Id);
                    command.ExecuteNonQuery();
                }
            }
        }
        //wczytaj wystawy
        public List<AddExhibitions> GetExhibitions()
        {
            string sql = "SELECT id,name,description,startDate,endDate, location, responsiblePerson, status, type FROM exhibitions;";
            List<AddExhibitions> list = new List<AddExhibitions>();            

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string description = reader.IsDBNull(2) ? null : reader.GetString(2);                            
                            DateTime startDate = DateTime.Parse(reader.GetString(3));
                            DateTime endDate = DateTime.Parse(reader.GetString(4));
                            string location = reader.GetString(5);
                            string responsiblePerson = reader.GetString(6);
                            string status = reader.GetString(7);
                            string type = reader.GetString(8);

                            AddExhibitions exhibitions = new AddExhibitions(id, name, description, startDate, endDate, location, responsiblePerson, status, type);
                            list.Add(exhibitions);
                        }
                    }
                }               
            }
            return list;
        }
        //dodaj wystawe
        public void InsertExhibitions(AddExhibitions addExhibitions)
        {
            string sql = @"INSERT INTO exhibitions(Name,Description,StartDate,EndDate,Location,ResponsiblePerson,Status,Type) 
                    VALUES(@Name,@Description,@StartDate,@EndDate,@Location,@ResponsiblePerson,@Status,@Type);";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", addExhibitions.Name);
                    command.Parameters.AddWithValue("@Description", addExhibitions.Description);
                    command.Parameters.AddWithValue("@StartDate", addExhibitions.StartDate);
                    command.Parameters.AddWithValue("@EndDate", addExhibitions.EndDate);
                    command.Parameters.AddWithValue("@Location", addExhibitions.Location);
                    command.Parameters.AddWithValue("@ResponsiblePerson", addExhibitions.ResponsiblePerson);
                    command.Parameters.AddWithValue("@Status", addExhibitions.Status);
                    command.Parameters.AddWithValue("@Type", addExhibitions.Type);
                    command.ExecuteNonQuery();
                }
            }
        }
        //Update wystawy
        public void UpdateExhibitions(AddExhibitions addExhibitions)
        {
            string sql = @"UPDATE exhibitions 
                SET name = @Name, 
                description = @Description, 
                startDate = @StartDate, 
                endDate = @EndDate, 
                location = @Location, 
                responsiblePerson = @ResponsiblePerson, 
                status = @Status, 
                type = @Type 
                WHERE id = @Id";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();                

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", addExhibitions.Name);
                    command.Parameters.AddWithValue("@Description", addExhibitions.Description);
                    command.Parameters.AddWithValue("@StartDate", addExhibitions.StartDate);
                    command.Parameters.AddWithValue("@EndDate", addExhibitions.EndDate);
                    command.Parameters.AddWithValue("@Location", addExhibitions.Location);
                    command.Parameters.AddWithValue("@ResponsiblePerson", addExhibitions.ResponsiblePerson);
                    command.Parameters.AddWithValue("@Status", addExhibitions.Status);
                    command.Parameters.AddWithValue("@Type", addExhibitions.Type);
                    command.Parameters.AddWithValue("@Id", addExhibitions.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        //metoda konwertuje tablice byte na obrazek bitmap 
        private BitmapImage convertBytesToBitmap(byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream ms = new MemoryStream(bytes);
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
        //metoda konwertuje obrazek bitmap na tablice byte 
        private byte[] convertBitmapToBytes(BitmapImage image)
        {
            byte[] data;

            MemoryStream ms = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(ms);

            data = ms.ToArray();

            return data;
        }
        //usuwanie wystawy
        public void DeleteExhibitions(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM exhibitions WHERE id = @Id";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        //Pobierz eksponty przypisane do wystawy
        public List<ExhibitsInExhibitionsDto> GetExhibitsInExhibitions()
        {
            string sql = @"SELECT 
                exhibitions.id, 
                exhibits.id,
                exhibitions.name,
                exhibitions.startDate,
                exhibitions.endDate,
                exhibits.name
                from exhibits_exhibitions
                join exhibitions on idExhibitions = exhibitions.id
                join exhibits on idExhibits = exhibits.id;";
            
            List<ExhibitsInExhibitionsDto> list = new List<ExhibitsInExhibitionsDto>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int exhibitionId = reader.GetInt32(0);
                            int exhibitId = reader.GetInt32(1);
                            string exhibitionName = reader.GetString(2);
                            DateTime exhibitionStart = DateTime.Parse(reader.GetString(3));
                            DateTime exhibitionEnd = DateTime.Parse(reader.GetString(4));
                            string exhibitName = reader.GetString(5);

                            ExhibitsInExhibitionsDto expositionAssetDto = new ExhibitsInExhibitionsDto(exhibitionId, exhibitId, exhibitionName, exhibitionStart, exhibitionEnd, exhibitName);
                            list.Add(expositionAssetDto);

                        }
                    }
                }
            }
            return list;
        }
        //pobierz z tabeli historia
        public List<History> GetHistory()
        {
            string sql = @"SELECT * FROM history";

            List<History> list = new List<History>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int historyId = reader.GetInt32(0);
                            string tableName = reader.GetString(1);
                            int recordId = reader.GetInt32(2);
                            string operation = reader.GetString(3);
                            string changed_by = reader.IsDBNull(4) ? null : reader.GetString(4);//reader.IsDBNull(4) sprawdza, czy wartość w kolumnie o indeksie 4 jest NULL.
                            DateTime changed_at = DateTime.Parse(reader.GetString(5));
                            string old_values = reader.IsDBNull(6) ? null : reader.GetString(6);
                            string new_values = reader.IsDBNull(7) ? null : reader.GetString(7);

                            History HistoryList = new History(historyId, tableName, recordId, operation, changed_by, changed_at, old_values, new_values);
                            list.Add(HistoryList);

                        }
                    }
                }
            }
            return list;
        }
        public List<AddExhibitions> GetUpcomingExhibitions()
        {
            string sql = "SELECT id, name, description, startDate, endDate, location, responsiblePerson, status, type " +
                         "FROM exhibitions " +
                         "WHERE startDate >= @CurrentDate AND startDate <= @Next30Days";

            List<AddExhibitions> list = new List<AddExhibitions>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    // Dodaj parametry dla obecnej daty i daty za 30 dni
                    command.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Next30Days", DateTime.Now.AddDays(30));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string description = reader.IsDBNull(2) ? null : reader.GetString(2);
                            DateTime startDate = DateTime.Parse(reader.GetString(3));
                            DateTime endDate = DateTime.Parse(reader.GetString(4));
                            string location = reader.GetString(5);
                            string responsiblePerson = reader.GetString(6);
                            string status = reader.GetString(7);
                            string type = reader.GetString(8);

                            // Utwórz obiekt AddExhibitions z danymi z bazy
                            AddExhibitions exhibition = new AddExhibitions(id, name, description, startDate, endDate, location, responsiblePerson, status, type);
                            list.Add(exhibition);
                        }
                    }
                }
            }

            return list;
        }
        //pobranie do dataGrida w Users.xaml
        public List<User> GetUsers()
        {
            string sql = @"SELECT id, email, password, rola FROM users";

            List<User> list = new List<User>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string email = reader.GetString(1);
                            string password = reader.GetString(2);
                            string rola = reader.GetString(3);
                          
                            list.Add(new User()
                            {
                                Id = id,
                                Email = email,
                                Password = password,
                                Role = rola,
                            });

                        }
                    }
                }
            }
            return list;
        }
        //Dodanie uzytkowników dla admina
        public void InsertUser(User user)
        {
            string sql = @"INSERT INTO users(email,password, rola) VALUES(@Email,@Password, @Role);";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Role", user.Role);

                    command.ExecuteNonQuery();
                }
            }
        }

        //usuwanie dla admina
        public void DeleteUser(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM users WHERE id = @Id";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        //update dla admina
        public void UpdateUser(User user)
        {
            string sql = @"UPDATE users SET email = @Email, password = @Password, rola = @Role WHERE id = @Id;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Role", user.Role);

                    command.ExecuteNonQuery();
                }
            }
        }

        //dodanie eksponatu do wystawy 
        public void AddExhibitToExhibition(int exhibitionId, int exhibitId)
        {
            string sql = @"INSERT INTO exhibits_exhibitions (idExhibitions, idExhibits) 
                   VALUES (@ExhibitionId, @ExhibitId);";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ExhibitionId", exhibitionId);
                    command.Parameters.AddWithValue("@ExhibitId", exhibitId);
                    command.ExecuteNonQuery();
                }
            }
        }

        //usuń eksponat z wystawy
        public void DeleteExhibitFromExhibition(int exhibitionId, int exhibitId)
        {
            string sql = @"DELETE FROM exhibits_exhibitions 
                   WHERE idExhibitions = @ExhibitionId AND idExhibits = @ExhibitId;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ExhibitionId", exhibitionId);
                    command.Parameters.AddWithValue("@ExhibitId", exhibitId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
