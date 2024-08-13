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
        public void ExecuteQuery(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

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
        //wczytaj eksponaty
        public DataTable GetExhibits()
        {
            string sql = "SELECT id, name, Description, Year, Category, Author, Origin, Location FROM exhibits;";            
            DataTable dt = new DataTable(); //przechowuje wyniki z db

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();                
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, connection))
                {
                    adapter.Fill(dt);//wypełnia pole danymi
                } //wykonuje zapytanie sql i wypełnia tabele
            }
            return dt;
        }
        //dodaj eksponat
        public void InsertExhibits(AddExhibits addExhibits)
        {

            string sql = "INSERT INTO exhibits(Name,Description,Year,Category,Author,Origin,Location) VALUES(@Name,@Description,@Year,@Category,@Author,@Origin,@Location);";

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
                    //command.Parameters.AddWithValue("@Image", addExhibits.Image); //przerobić na tablice bytes
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
    }
}
