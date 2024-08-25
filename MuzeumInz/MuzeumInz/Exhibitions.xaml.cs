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
        private int? idExhibitions;
        List<AddExhibitions> addExhibitions;
        private List<ExhibitsInExhibitionsDto> exhibitsInExhibitionsDtos;
        public Exhibitions()
        {
            InitializeComponent();
            dbConnect = new DbConnect();
            loadGrid();
            LoadExhibitsInExhibitions();
        }
        public void loadGrid()
        {
            addExhibitions = dbConnect.GetExhibitions();
            exhibitionsDb.ItemsSource = null;
            exhibitionsDb.ItemsSource = addExhibitions;
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
            if (string.IsNullOrEmpty(exhibitions_addNameTxt.Text))
            {
                MessageBox.Show("Nazwa nie może być pusta");
            }
            else if (!exhibitions_addStartDateTxt.SelectedDate.HasValue)
            {
                MessageBox.Show("Data nie może być pusta");
            }
            else if (!exhibitions_addEndDateTxt.SelectedDate.HasValue)
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
                AddExhibitions exhibitions = new AddExhibitions(0, exhibitions_addNameTxt.Text, exhibitions_addDescriptionTxt.Text, exhibitions_addStartDateTxt.SelectedDate.Value, exhibitions_addEndDateTxt.SelectedDate.Value, exhibitions_addLocationTxt.Text, exhibitions_addResponsiblePersonTxt.Text, ((ComboBoxItem)exhibitions_addStatusList.SelectedItem).Content.ToString(), ((ComboBoxItem)exhibitions_addTypeTxt.SelectedItem).Content.ToString());
                dbConnect.InsertExhibitions(exhibitions);
                loadGrid();
            }       
               
        }
        //usupełnij pola edycji po zaznczeniu wiersza
        private void exhibitionsDb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exhibitionsDb.SelectedIndex != -1)
            {
                AddExhibitions exhibitions = (AddExhibitions)exhibitionsDb.SelectedItem;

                idExhibitions = exhibitions.Id;
                exhibitions_editNameTxt.Text = exhibitions.Name;
                exhibitions_editStatusList.Text = exhibitions.Status;
                exhibitions_editStartDateTxt.SelectedDate = exhibitions.StartDate;
                exhibitions_editEndDateTxt.SelectedDate = exhibitions.EndDate;
                exhibitions_editResponsiblePersonTxt.Text = exhibitions.ResponsiblePerson;
                exhibitions_editLocationTxt.Text = exhibitions.Location;
                exhibitions_editDescriptionTxt.Text = exhibitions.Description;
                exhibitions_editTypeTxt.Text = exhibitions.Type;

                exhibitions_exhibitsInExhibitionsGrid.Visibility = Visibility.Visible;
            } 
            else
            {
                idExhibitions = 0;
            }
        }
        //zapisz edycje wystawy
        private void exhibitions_saveEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(exhibitions_editNameTxt.Text))
            {
                MessageBox.Show("Nazwa nie może być pusta");
            }
            else if (!exhibitions_editStartDateTxt.SelectedDate.HasValue)
            {
                MessageBox.Show("Data nie może być pusta");
            }
            else if (!exhibitions_editEndDateTxt.SelectedDate.HasValue)
            {
                MessageBox.Show("Data nie może być pusta");
            }
            else if (string.IsNullOrEmpty(exhibitions_editResponsiblePersonTxt.Text))
            {
                MessageBox.Show("Organizator nie może być pusty");
            }
            else if (string.IsNullOrEmpty(exhibitions_editDescriptionTxt.Text))
            {
                MessageBox.Show("Opis nie może być pusty");
            }
            else if (string.IsNullOrEmpty(exhibitions_editLocationTxt.Text))
            {
                MessageBox.Show("Obecna lokalizacja nie może być pusta");
            }
            else
            {
                AddExhibitions exhibitions = new AddExhibitions(idExhibitions.Value, exhibitions_editNameTxt.Text, exhibitions_editDescriptionTxt.Text, exhibitions_editStartDateTxt.SelectedDate.Value, exhibitions_editEndDateTxt.SelectedDate.Value, exhibitions_editLocationTxt.Text, exhibitions_editResponsiblePersonTxt.Text, ((ComboBoxItem)exhibitions_editStatusList.SelectedItem).Content.ToString(), ((ComboBoxItem)exhibitions_editTypeTxt.SelectedItem).Content.ToString());
                dbConnect.UpdateExhibitions(exhibitions);
                loadGrid();
            }
        }
        //usuwanie wystawy
        private void exhibition_deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (idExhibitions != null)
            {
                dbConnect.DeleteExhibitions(idExhibitions.Value);
                loadGrid();
            }
            else
            {
                MessageBox.Show("Proszę zaznaczyć rekord do usunięcia.");
            }
        }
        //pokazuje w DataGridzie wszystkie wystawy i przypisane do nich eksponaty
        private void LoadExhibitsInExhibitions()
        {
            exhibitsInExhibitionsDtos = dbConnect.GetExhibitsInExhibitions();

            exhibitions_exhibitsInExhibitionDb.ItemsSource = null;
            exhibitions_exhibitsInExhibitionDb.ItemsSource = exhibitsInExhibitionsDtos;

            exhibitionsCmb.ItemsSource = dbConnect.GetExhibitions();
            exhibitionsCmb.SelectedIndex = 0;
            exhibitsCmb.ItemsSource = dbConnect.GetExhibits();
            exhibitsCmb.SelectedIndex = 0;
        }
    }
}
