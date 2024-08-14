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

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy Exhibitions.xaml
    /// </summary>
    public partial class Exhibitions : Window
    {
        public Exhibitions()
        {
            InitializeComponent();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Hide();
            MessageBox.Show("Pomyślnie wylogowano!");
        }

        private void ExhibitionsBtn1_Click(object sender, RoutedEventArgs e)
        {
            Exhibits exhibits = new Exhibits();
            exhibits.Show();
            this.Hide();
        }

        private void exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void exhibition_addExhibitsBtn_Click(object sender, RoutedEventArgs e)
        {
            exhibitions_allExhibitsGrid.Visibility = Visibility.Visible;
            exhibitions_addExhibitionsGrid.Visibility = Visibility.Collapsed;
            exhibitions_editExhibitionsGrid.Visibility = Visibility.Collapsed;
        }

        private void exhibition_addBtn_Click(object sender, RoutedEventArgs e)
        {
            exhibitions_addExhibitionsGrid.Visibility = Visibility.Visible;
            exhibitions_allExhibitsGrid.Visibility = Visibility.Collapsed;
            exhibitions_editExhibitionsGrid.Visibility = Visibility.Collapsed;
        }

        private void exhibition_editBtn_Click(object sender, RoutedEventArgs e)
        {
            exhibitions_editExhibitionsGrid.Visibility= Visibility.Visible;
            exhibitions_addExhibitionsGrid.Visibility = Visibility.Collapsed;
            exhibitions_allExhibitsGrid.Visibility = Visibility.Collapsed;
        }
    }
}
