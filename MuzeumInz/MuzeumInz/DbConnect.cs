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

namespace MuzeumInz
{
    public class DbConnect
    {
        private string connectionString;

        public DbConnect()
        {
            connectionString =ConfigurationManager.ConnectionStrings["Muzeum"].ConnectionString;//"Data Source=Muzeum.db;";
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
        /*public void ExecuteQuery(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }*/

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        //sprawdzenie czy użytkonik istanieje w bazie - do rejestracji
        public bool CheckUserExist(string email)
        {
            using (SQLiteConnection connection = GetConnection())
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
            using (SQLiteConnection connection = GetConnection())
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
            CREATE TABLE IF NOT EXISTS current_user (
                user_id TEXT
            );
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
    }
}
