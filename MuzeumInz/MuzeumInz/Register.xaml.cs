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
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Security.Cryptography; //hashowanie hasła algorytmem SHA-256

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public DbConnect dbConnect;
        public Register()
        {
            InitializeComponent();
            dbConnect = new DbConnect();
        }

        //Zamknięcie na "X"
        private void exitClick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Otworzenie okna logowania
        private void register_loginBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Hide();
        }

        private void register_signBtn_Click(object sender, RoutedEventArgs e)
        {
            string email = register_loginTxt.Text;
            string password = register_passTxt.Password;

            //Walidacja na email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Nieprawidłowy adres email");
                return;
            }

            //Walidacja na hasło
            if(!IsValidPassword(password))
            {
                MessageBox.Show("Haslo musi posiadać co najmniej: 8 znaków, jedna cyfrę oraz znak specjalny");
                return;
            }

            //Hashowanie hasła
            string hashedPass = HashPassword(password);

            //Sprawdzenie czy pola nie są puste
            if (string.IsNullOrEmpty(register_loginTxt.Text))
            {
                MessageBox.Show("Login nie moze być pusty");
                return;
            }
            else if (string.IsNullOrEmpty(register_passTxt.Password))
            {
                MessageBox.Show("Hasło nie moze być puste");
                return;
            }

            try
            {
                if(dbConnect.CheckUserExist(email))
                {
                    MessageBox.Show("Login już istnieje");
                    return;
                }

                dbConnect.RegisterUser(email, hashedPass);
                MessageBox.Show("Zarejestrowano poprawnie");

                //czyszczenie pól
                register_loginTxt.Text = string.Empty;
                register_passTxt.Password = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas rejestracji: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }   
        }

        // Metoda sprawdzająca poprawność adresu email za pomocą regex
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                // Regex dla sprawdzenia formatu email
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
                return emailRegex.IsMatch(email);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Metoda sprawdzająca poprawność hasła za pomocą regex
        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            try
            {
                // Regex dla sprawdzenia hasła
                var passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", RegexOptions.Compiled);
                return passwordRegex.IsMatch(password);
            }
            catch (Exception)
            {
                return false;
            }
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
