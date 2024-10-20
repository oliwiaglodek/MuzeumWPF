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
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections;
using System.Runtime.Remoting.Contexts;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Runtime.Remoting.Messaging;
using System.IO;

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy Exhibits.xaml
    /// </summary>
    public partial class Exhibits : Window
    {
        private BitmapImage image;        
        private DbConnect dbConnect;
        List<AddExhibits> addExhibits;
        private int? selectedId;
        private byte[] imageBytes;

        public Exhibits()
        {
            InitializeComponent();
            dbConnect = new DbConnect();

            selectedId = null;

            loadGrid();            
        }

        //przenoszenie okna
        private void Move_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        //Zamknięcie na "X"
        private void exitClick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dbConnect.ClearCurrentUser(); // Wyczyść zalogowanego użytkownika
            Application.Current.Shutdown();
        }

        //Wyloguj
        private void logout_Btn_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
            dbConnect.ClearCurrentUser();
            MessageBox.Show("Pomyślnie wylogowano!");
        }

        //Widoczność pól dodania nowego eksponatu
        private void exhibits_addBtn_Click(object sender, RoutedEventArgs e)
        {
            exhibits_addGrid.Visibility = Visibility.Visible;
            exhibits_editGrid.Visibility = Visibility.Collapsed;
        }

        public void loadGrid()
        {
            addExhibits = dbConnect.GetExhibits();
            exhibits_exhibitsDb.ItemsSource = null;
            exhibits_exhibitsDb.ItemsSource = addExhibits;
        }
        //przy zaznaczeniu wiersza, automatycznie uzupełnia pola z edycji
        private void exhibits_exhibitsDb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exhibits_exhibitsDb.SelectedIndex != -1)
            {
                AddExhibits exhibits = (AddExhibits)exhibits_exhibitsDb.SelectedItem;

                selectedId = exhibits.Id;

                exhibitsEdit_nameTxt.Text = exhibits.Name;
                exhibitsEdit_authorTxt.Text = exhibits.Author;
                exhibitsEdit_categoryList.Text = exhibits.Category;
                exhibitsEdit_yearTxt.Text = exhibits.Year.ToString();
                exhibitsEdit_originTxt.Text = exhibits.Origin;
                exhibitsEdit_locationTxt.Text = exhibits.Location;
                exhibitsEdit_descriptionTxt.Text = exhibits.Description;
                image = exhibits.Image;

                if(image != null)
                {
                    exhibitsEdit_imageBtn.Content = "Załadowano";
                }
                else
                {
                    exhibitsEdit_imageBtn.Content = "Wybierz";
                }
                exhibits_selectedImageBox.Source = image;
            }         
        }

        private void exhibits_saveBtn_Click(object sender, RoutedEventArgs e)
        {
            int year;

            if (string.IsNullOrEmpty(exhibits_nameTxt.Text))
            {
                MessageBox.Show("Nazwa nie może być pusta");
            }
            else if (string.IsNullOrEmpty(exhibits_yearTxt.Text) || !int.TryParse(exhibits_yearTxt.Text, out year))
            {
                MessageBox.Show("Podano niepoprawny rok");
            }
            else if (string.IsNullOrEmpty(exhibits_authorTxt.Text))
            {
                MessageBox.Show("Autor nie może być pusty");
            } 
            else if(string.IsNullOrEmpty(exhibits_originTxt.Text))
            {
                MessageBox.Show("Pochodzenie nie może być puste");
            }
            else if (string.IsNullOrEmpty(exhibits_locationTxt.Text))
            {
                MessageBox.Show("Obecna lokalizacja nie może być pusta");
            }
            else
            {
                AddExhibits exhibit = new AddExhibits(0, exhibits_nameTxt.Text, exhibits_descriptionTxt.Text, year, ((ComboBoxItem)exhibits_categoryList.SelectedItem).Content.ToString(), exhibits_authorTxt.Text, exhibits_originTxt.Text, image, exhibits_locationTxt.Text);
                dbConnect.InsertExhibits(exhibit);
                loadGrid();
            }
        }
        //Przycisk edytuj
        private void exhibits_editBtn_Click(object sender, RoutedEventArgs e)
        {
            exhibits_editGrid.Visibility = Visibility.Visible;
            exhibits_addGrid.Visibility = Visibility.Collapsed;
        }
        //usuwanie eksponatu
        private void exhibits_deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(selectedId != null)
            {
                dbConnect.DeleteExhibits(selectedId.Value);                
                loadGrid();
                exhibits_selectedImageBox.Source = null;
            }
            else
            {
                MessageBox.Show("Proszę zaznaczyć rekord do usunięcia.");
            }
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

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
            MessageBox.Show("Pomyślnie wylogowano!");
        }

        private void exhibitsEdit_saveBtn_Click(object sender, RoutedEventArgs e)
        {
            int year;

            if (string.IsNullOrEmpty(exhibitsEdit_nameTxt.Text))
            {
                MessageBox.Show("Nazwa nie może być pusta");
            }
            else if (string.IsNullOrEmpty(exhibitsEdit_yearTxt.Text) || !int.TryParse(exhibitsEdit_yearTxt.Text, out year))
            {
                MessageBox.Show("Podano niepoprawny rok");
            }
            else if (string.IsNullOrEmpty(exhibitsEdit_authorTxt.Text))
            {
                MessageBox.Show("Autor nie może być pusty");
            }
            else if (string.IsNullOrEmpty(exhibitsEdit_originTxt.Text))
            {
                MessageBox.Show("Pochodzenie nie może być puste");
            }
            else if (string.IsNullOrEmpty(exhibitsEdit_locationTxt.Text))
            {
                MessageBox.Show("Obecna lokalizacja nie może być pusta");
            }
            else
            {
                AddExhibits exhibit = new AddExhibits(selectedId.Value, exhibitsEdit_nameTxt.Text, exhibitsEdit_descriptionTxt.Text, year, ((ComboBoxItem)exhibitsEdit_categoryList.SelectedItem).Content.ToString(), exhibitsEdit_authorTxt.Text, exhibitsEdit_originTxt.Text, image, exhibitsEdit_locationTxt.Text);
                dbConnect.UpdateExhibits(exhibit);
                loadGrid();
            }
        }
        //przycisk wybór obrazka
        private void exhibits_imageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(openFileDialog.FileName);
                image.EndInit();

                exhibits_imageBtn.Content = System.IO.Path.GetFileName(openFileDialog.FileName);
               //zamiana obrazka na tablice byte
               using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, (int)fs.Length);
                }
            }
        }
        //metoda konwertuje tablice byte na obrazek bitmap
        private BitmapImage convertBytesToBitmap(byte[] imageBytes)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageBytes);
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
        //obrazek w edycji
        private void exhibitsEdit_imageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(openFileDialog.FileName);
                image.EndInit();

                exhibits_imageBtn.Content = System.IO.Path.GetFileName(openFileDialog.FileName);
                //zamiana obrazka na tablice byte
                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, (int)fs.Length);
                }
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchExhibits();
        }
        //wyszukaj eksponat
        private void SearchExhibits()
        {
            string nameFilter = searchByNameTxt.Text.ToLower();
            string authorFilter = searchByAuthorTxt.Text.ToLower();
            string locationFilter = searchByLocationTxt.Text.ToLower();
            string yearFilter = searchByYearTxt.Text;         

            var filteredExhibits = addExhibits.Where(exhibit =>
                (string.IsNullOrEmpty(nameFilter) || exhibit.Name.ToLower().Contains(nameFilter)) &&
                (string.IsNullOrEmpty(authorFilter) || exhibit.Author.ToLower().Contains(authorFilter)) &&
                (string.IsNullOrEmpty(locationFilter) || exhibit.Location.ToLower().Contains(locationFilter)) &&
                (string.IsNullOrEmpty(yearFilter) || exhibit.Year == int.Parse(yearFilter)) 
            ).ToList();

            exhibits_exhibitsDb.ItemsSource = filteredExhibits;
        }

        //wyszukiwanie w czasie rzeczywistym
        private void searchByNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibits();
        }

        private void searchByAuthorTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibits();
        }

        private void searchByLocationTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibits();
        }

        private void searchByYearTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchExhibits();
        }
    }
}
