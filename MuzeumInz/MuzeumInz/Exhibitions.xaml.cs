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
        private int idExhibitions;
        private List<ExhibitsInExhibitionsDto> exhibitsInExhibitionsDtos;
        public Exhibitions()
        {
            InitializeComponent();
            dbConnect = new DbConnect();
            loadGrid();
            LoadExhibitsInExhibitions(idExhibitions);
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

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            ExhibitsHistory ExhibitsHistory = new ExhibitsHistory();
            ExhibitsHistory.Show();
            this.Hide();
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            Inventory Inventory = new Inventory();
            Inventory.Show();
            this.Hide();
        }

        private void exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dbConnect.ClearCurrentUser(); // Wyczyść zalogowanego użytkownika
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
            exhibitions_exhibitsInExhibitionsGrid.Visibility = Visibility.Collapsed;
        }

        private void exhibition_editBtn_Click(object sender, RoutedEventArgs e)
        {
            exhibitions_editExhibitionsGrid.Visibility= Visibility.Visible;
            exhibitions_addExhibitionsGrid.Visibility = Visibility.Collapsed;
            exhibitions_allExhibitsGrid.Visibility = Visibility.Collapsed;
            exhibitions_exhibitsInExhibitionsGrid.Visibility = Visibility.Collapsed;
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
        //usupełnij pola edycji po zaznczeniu wiersza
        private void exhibitionsDb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exhibitionsDb.SelectedItem is DataRowView selectedRow)
            {
                exhibitions_editNameTxt.Text = selectedRow["name"].ToString();
                exhibitions_editStatusList.Text = selectedRow["Status"].ToString();
                exhibitions_editStartDateTxt.Text = selectedRow["StartDate"].ToString();
                exhibitions_editEndDateTxt.Text = selectedRow["EndDate"].ToString();
                exhibitions_editResponsiblePersonTxt.Text = selectedRow["ResponsiblePerson"].ToString();
                exhibitions_editLocationTxt.Text = selectedRow["Location"].ToString();
                exhibitions_editDescriptionTxt.Text = selectedRow["Description"].ToString();
                exhibitions_editTypeTxt.Text = selectedRow["Type"].ToString();

                idExhibitions = Convert.ToInt32(selectedRow["id"]);
                //dbConnect.LoadExhibitsInExhibition(idExhibitions);
                exhibitions_exhibitsInExhibitionsGrid.Visibility = Visibility.Visible;
            }
        }
        //zapisz edycje wystawy
        private void exhibitions_saveEditBtn_Click(object sender, RoutedEventArgs e)
        {          
            var selectedItem = (DataRowView)exhibitionsDb.SelectedItem;
            int id = Convert.ToInt32(selectedItem["id"]); 
            string name = exhibitions_editNameTxt.Text;
            string description = exhibitions_editDescriptionTxt.Text;
            DateTime startDate = exhibitions_editStartDateTxt.SelectedDate.Value;
            DateTime endDate = exhibitions_editEndDateTxt.SelectedDate.Value;
            string location = exhibitions_editLocationTxt.Text;
            string responsiblePerson = exhibitions_editResponsiblePersonTxt.Text;
            string status = exhibitions_editStatusList.Text;
            string type = exhibitions_editTypeTxt.Text;

            // Wywołaj metodę aktualizacji
            AddExhibitions addExhibitions = new AddExhibitions(id, name, description, startDate, endDate, location, responsiblePerson, status, type);
            dbConnect.UpdateExhibitions(addExhibitions);
            loadGrid();
        }
        //usuwanie wystawy
        private void exhibition_deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (exhibitionsDb.SelectedItem != null)
            {
                // Pobranie zaznaczonego rekordu (wiersza) jako obiekt
                var selectedItem = (DataRowView)exhibitionsDb.SelectedItem;
                int id = Convert.ToInt32(selectedItem["id"]);                

                dbConnect.DeleteExhibitions(id);
                loadGrid();
            }
            else
            {
                MessageBox.Show("Proszę zaznaczyć rekord do usunięcia.");
            }
        }
        //pokazuje w DataGridzie wszystkie wystawy i przypisane do nich eksponaty
        private void LoadExhibitsInExhibitions(int idExhibition)
        {
            exhibitsInExhibitionsDtos = dbConnect.GetExhibitsInExhibitions();

            exhibitions_exhibitsInExhibitionDb.ItemsSource = null;
            exhibitions_exhibitsInExhibitionDb.ItemsSource = exhibitsInExhibitionsDtos;


        }
    }
}
