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

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DbConnect dbConnect;
        public MainWindow()
        {
            InitializeComponent();
            dbConnect = new DbConnect();
        }

        //zamknięcie okna na "X"
        private void exitClick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string email = loginTxt.Text;
            string password = loginPassTxt.Text;

            string query = $"SELECT * FROM users WHERE email='{email}' AND password='{password}'";

            SQLiteCommand command = new SQLiteCommand(query);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);

            DataTable result = dbConnect.GetData(command);

            if (result.Rows.Count > 0)
            {
                MessageBox.Show("Logowanie poprawne");
            }
            else
            {
                MessageBox.Show("Błedy login lub hasło");

            }

        }
    }

}
