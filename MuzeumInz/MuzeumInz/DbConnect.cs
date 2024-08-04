using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Configuration;

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
    }
}
