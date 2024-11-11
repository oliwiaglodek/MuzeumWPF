using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        private DbConnect dbConnect;
        private int? idUser;
        public Users()
        {
            InitializeComponent();
            dbConnect = new DbConnect();
            loadGrid();

            roleCmb.ItemsSource = new string[] { "user", "admin" };
            roleCmb.SelectedIndex = 0;
        }
        public void loadGrid()
        {
            usersGrid.ItemsSource = null;
            usersGrid.ItemsSource = dbConnect.GetUsers();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(email_Txt.Text))
            {
                MessageBox.Show("Email nie może być pusty");
            }
            else if (!isValidEmail(email_Txt.Text))
            {
                MessageBox.Show("Email nie jest poprawny");
            }
            else if (string.IsNullOrEmpty(password_Txt.Text) || !IsValidPassword(password_Txt.Text))
            {
                MessageBox.Show("Haslo musi posiadać co najmniej: 8 znaków, jedna cyfrę oraz znak specjalny");
                return;
            }
            else
            {

                User user = new User()
                {
                    Id = idUser.GetValueOrDefault(),
                    Email = email_Txt.Text,
                    Password = HashPassword(password_Txt.Text),
                    Role = roleCmb.SelectedValue.ToString()
                };

                if (idUser.HasValue)
                {
                    dbConnect.UpdateUser(user);
                }
                else
                {
                    dbConnect.InsertUser(user);
                }

                loadGrid();

                email_Txt.Clear();
                password_Txt.Clear();
            }
        }
        //Hashowanie hasła
        public string HashPassword(string password)
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

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (idUser != null)
            {
                dbConnect.DeleteUser(idUser.Value);
                loadGrid();
            }
            else
            {
                MessageBox.Show("Proszę zaznaczyć rekord do usunięcia.");
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

        private bool isValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void usersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersGrid.SelectedIndex != -1)
            {
                User user = (User)usersGrid.SelectedItem;
                idUser = user.Id;
                email_Txt.Text = user.Email;
                password_Txt.Text = user.Password;
                roleCmb.Text = user.Role;
                addUsersBtn.Content = "Zapisz";
            }
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            idUser = null;
            email_Txt.Clear();
            password_Txt.Clear();
            addUsersBtn.Content = "Dodaj";
        }

        private void ExhibitionsBtn_Click(object sender, RoutedEventArgs e)
        {
            var existingWindow = Application.Current.Windows.OfType<Exhibitions>().FirstOrDefault();

            if (existingWindow == null)
            {
                using (DbConnect dbConnect = new DbConnect())
                {
                    Exhibitions exhibitionsWindow = new Exhibitions();
                    exhibitionsWindow.Show();
                    this.Close();
                }
            }
            else
            {
                existingWindow.Focus();
            }
        }

        private void ExhibitsBtn_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzamy, czy okno Exhibits już istnieje
            var existingWindow = Application.Current.Windows.OfType<Exhibits>().FirstOrDefault();

            if (existingWindow == null)  // Jeśli okno nie istnieje
            {
                using (DbConnect dbConnect = new DbConnect())
                {
                    Exhibits exhibitsWindow = new Exhibits();
                    exhibitsWindow.Show();
                    this.Close();  // Zamykamy aktualne okno, aby zwolnić zasoby
                } // Po zakończeniu using dbConnect zostanie zamknięte
            }
            else  // Jeśli okno istnieje
            {
                existingWindow.Focus();  // Skupiamy się na istniejącym oknie
            }
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var existingWindow = Application.Current.Windows.OfType<ExhibitsHistory>().FirstOrDefault();

            if (existingWindow == null)
            {
                using (DbConnect dbConnect = new DbConnect())
                {
                    ExhibitsHistory exhibitsHistoryWindow = new ExhibitsHistory();
                    exhibitsHistoryWindow.Show();
                    this.Close();
                }
            }
            else
            {
                existingWindow.Focus();  // Skupiamy się na istniejącym oknie
            }
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            var existingWindow = Application.Current.Windows.OfType<Inventory>().FirstOrDefault();

            if (existingWindow == null)
            {
                using (DbConnect dbConnect = new DbConnect())
                {
                    Inventory inventoryWindow = new Inventory();
                    inventoryWindow.Show();
                    this.Close();
                }
            }
            else
            {
                existingWindow.Focus();
            }
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            var existingWindow = Application.Current.Windows.OfType<Reports>().FirstOrDefault();

            if (existingWindow == null)
            {
                using (DbConnect dbConnect = new DbConnect())
                {
                    Reports reportsWindow = new Reports();
                    reportsWindow.Show();
                    this.Close();
                }
            }
            else
            {
                existingWindow.Focus();
            }
        }

        private void UsersBtn_Click(object sender, RoutedEventArgs e)
        {
            var existingWindow = Application.Current.Windows.OfType<Users>().FirstOrDefault();

            if (existingWindow == null)
            {
                using (DbConnect dbConnect = new DbConnect())
                {
                    Users exhibitsHistoryWindow = new Users();
                    exhibitsHistoryWindow.Show();
                    this.Close();
                }
            }
            else
            {
                existingWindow.Focus();
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
            dbConnect.ClearCurrentUser();
            MessageBox.Show("Pomyślnie wylogowano!");
        }

        private void exitClick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dbConnect.ClearCurrentUser(); // Wyczyść zalogowanego użytkownika
            Application.Current.Shutdown();
        }
    }
}
