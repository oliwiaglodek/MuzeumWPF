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

        //przenoszenie okna
        private void Move_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
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

                ExhibitionNotificationService emailServices = new ExhibitionNotificationService();
                emailServices.SendNotificationEmail(exhibitions);

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

        //wyszukaj wystawe
        private void SearchExhibitions()
        {
            string nameFilter = searchByNameTxt.Text.ToLower();
            string locationFilter = searchByLocationTxt.Text.ToLower();
            string responsiblePersonFilter = searchByResponsiblePersonTxt.Text.ToLower();
            string statusFilter = searchByStatusTxt.Text.ToLower();

            // Parsowanie startDateFilter i endDateFilter jako daty
            DateTime? startDateFilter = null;
            DateTime? endDateFilter = null;

            if (DateTime.TryParse(searchByStartDate.Text, out DateTime parsedStartDate))
            {
                startDateFilter = parsedStartDate;
            }

            if (DateTime.TryParse(searchByEndDate.Text, out DateTime parsedEndDate))
            {
                endDateFilter = parsedEndDate;
            }

            var filteredExhibitions = addExhibitions.Where(exhibitions =>
                (string.IsNullOrEmpty(nameFilter) || exhibitions.Name.ToLower().Contains(nameFilter)) &&
                (!startDateFilter.HasValue || exhibitions.StartDate >= startDateFilter.Value) &&
                (!endDateFilter.HasValue || exhibitions.EndDate <= endDateFilter.Value) &&
                (string.IsNullOrEmpty(locationFilter) || exhibitions.Location.ToLower().Contains(locationFilter)) &&
                (string.IsNullOrEmpty(responsiblePersonFilter) || exhibitions.ResponsiblePerson.ToLower().Contains(responsiblePersonFilter)) &&
                (string.IsNullOrEmpty(statusFilter) || exhibitions.Status.ToLower().Contains(statusFilter))
                ).ToList();

            exhibitionsDb.ItemsSource = filteredExhibitions;
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchExhibitions();
        }

        private void searchByNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibitions();
        }

        private void searchByLocationTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibitions();
        }

        private void searchByResponsiblePersonTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibitions();
        }

        private void searchByStatusTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibitions();
        }

        private void searchByStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchExhibitions();
        }

        private void searchByEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchExhibitions();
        }


        //wyszukaj eksponatów w wystawie
        private void SearchExhibits()
        {
            string exhibitionFilter = serachByExhibitionTxt.Text.ToLower();
            string exhibitsFilter = serahByExhibitsTxt.Text.ToLower();

            var filteredExhibitions = exhibitsInExhibitionsDtos.Where(exhibitions =>
                (string.IsNullOrEmpty(exhibitionFilter) || exhibitions.ExhibitionName.ToLower().Contains(exhibitionFilter)) &&
                (string.IsNullOrEmpty(exhibitsFilter) || exhibitions.ExhibitName.ToLower().Contains(exhibitsFilter)) 
                ).ToList();

            exhibitions_exhibitsInExhibitionDb.ItemsSource = filteredExhibitions;
        }

        private void serachByExhibitionTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibits();
        }

        private void serahByExhibitsTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibits();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchExhibits();
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
    }
}
