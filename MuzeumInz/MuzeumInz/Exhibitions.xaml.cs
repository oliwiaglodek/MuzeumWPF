using System;
using System.Collections.Generic;
using System.Data;
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
        private DbConnect dbConnect;
        public Exhibitions()
        {
            InitializeComponent();
            dbConnect = new DbConnect();
            loadGrid();
        }
        public void loadGrid()
        {
            DataTable addExhibitions = dbConnect.GetExhibitions();
            exhibitionsDb.ItemsSource = addExhibitions.DefaultView;
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
        //dodaj wystawe
        private void exhibitions_saveAddBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = exhibitions_addStartDateTxt.SelectedDate.Value.Date;//data z godziną :( pomyślec jak to sformatować
            DateTime? endDate = exhibitions_addEndDateTxt.SelectedDate.Value.Date;

            if (string.IsNullOrEmpty(exhibitions_addNameTxt.Text))
            {
                MessageBox.Show("Nazwa nie może być pusta");
            }
            else if (!startDate.HasValue)
            {
                MessageBox.Show("Data nie może być pusta");
            }
            else if (!endDate.HasValue) 
            {
                MessageBox.Show("Data nie może być pusta");
            }
            else if (string.IsNullOrEmpty(exhibitions_addResponsiblePersonTxt.Text))
            {
                MessageBox.Show("Organizator nie może być pusty");
            }
            else if (string.IsNullOrEmpty(exhibitions_addDescriptionTxt.Text))
            {
                MessageBox.Show("Opis nie może być pusty");
            }
            else if (string.IsNullOrEmpty(exhibitions_addLocationTxt.Text))
            {
                MessageBox.Show("Obecna lokalizacja nie może być pusta");
            }
            else
            {
                AddExhibitions exhibitions = new AddExhibitions(0, exhibitions_addNameTxt.Text, exhibitions_addDescriptionTxt.Text, startDate,endDate, exhibitions_addLocationTxt.Text, exhibitions_addResponsiblePersonTxt.Text, ((ComboBoxItem)exhibitions_addStatusList.SelectedItem).Content.ToString(), ((ComboBoxItem)exhibitions_addTypeTxt.SelectedItem).Content.ToString());
                dbConnect.InsertExhibitions(exhibitions);
                loadGrid();
            }
        }
    }
}
