using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Role = "user";
        public DbConnect dbConnect;

        public MainWindow()
        {
            InitializeComponent();
            dbConnect = new DbConnect();
        }

        //przenoszenie okna
        private void Move_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        //zamknięcie okna na "X"
        private void exitClick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dbConnect.ClearCurrentUser(); // Wyczyść zalogowanego użytkownika
            Application.Current.Shutdown();
        }

        //Obsługa logowania
        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string email = login_loginTxt.Text;
            string password = login_passTxt.Password;
            string hashedPass = HashPassword(password);

            string query = "SELECT * FROM users WHERE email = @Email AND password = @Password";

            using (SQLiteCommand command = new SQLiteCommand(query))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", hashedPass);

                DataTable result = dbConnect.GetData(command);

                if (result.Rows.Count > 0)
                {
                    Role = result.Rows[0]["role"].ToString();
                    dbConnect.SetCurrentUser(email);  // Zapisz zalogowanego użytkownika
                    Main_Panel_Btn_Click(sender, e); //przy poprawnym zalogowaniu otworzy panel administracyjny
                }
                else
                {
                    MessageBox.Show("Błędny login lub hasło");
                }
            }
        }

        //Otworzy okno rejestracji
        private void signBtn_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.Show();
            this.Close();
        }

        //Otworzy okno panelu głównego
        private void Main_Panel_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainPanel MainPanelWindow = new MainPanel();
            MainPanelWindow.Show();
            this.Close();
        }
        //Hashowanie hasła
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

}
