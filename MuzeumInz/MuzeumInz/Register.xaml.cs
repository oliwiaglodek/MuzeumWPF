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
                    MessageBox.Show("Login już istanieje");
                    return;
                }

                dbConnect.RegisterUser(email, password);
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
    }
}
