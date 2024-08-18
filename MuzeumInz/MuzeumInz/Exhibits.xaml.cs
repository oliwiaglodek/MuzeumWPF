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
        public ObservableCollection<AddExhibits> Items { get; set; } //powiązania danych z interfejsem użytkownika
        private int? selectedId;
        private byte[] imageBytes;

        public Exhibits()
        {
            InitializeComponent();
            dbConnect = new DbConnect();

            Items = new ObservableCollection<AddExhibits>();
            exhibits_exhibitsDb.ItemsSource = Items;

            selectedId = null;

            loadGrid();            
        }

        //Zamknięcie na "X"
        private void exitClick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Wyloguj
        private void logout_Btn_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Hide();
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
            DataTable addExhibits = dbConnect.GetExhibits();          
            exhibits_exhibitsDb.ItemsSource = addExhibits.DefaultView;
        }
        //przy zaznaczeniu wiersza, automatycznie uzupełnia pola z edycji
        private void exhibits_exhibitsDb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exhibits_exhibitsDb.SelectedItem is DataRowView selectedRow)
            {
                exhibitsEdit_nameTxt.Text = selectedRow["name"].ToString();
                exhibitsEdit_authorTxt.Text = selectedRow["Author"].ToString();
                exhibitsEdit_categoryList.Text = selectedRow["Category"].ToString();
                exhibitsEdit_yearTxt.Text = selectedRow["Year"].ToString();
                exhibitsEdit_originTxt.Text = selectedRow["Origin"].ToString();
                exhibitsEdit_locationTxt.Text = selectedRow["Location"].ToString();
                exhibitsEdit_descriptionTxt.Text = selectedRow["Description"].ToString();
                if(selectedRow["Image"] != DBNull.Value)
                {
                    imageBytes = (byte[])selectedRow["Image"];
                }
                else
                {
                    imageBytes = null; 
                }
                BitmapImage image = imageBytes != null ? convertBytesToBitmap(imageBytes) : null; 
                exhibitsEdit_nameTxt.Focus();

                if (image == null)
                {
                    exhibitsEdit_imageBtn.Content = "Wybierz";
                    exhibits_selectedImageBox.Source = null;
                    exhibits_selectedImageBox.UpdateLayout();
                }
                else
                {
                    exhibitsEdit_imageBtn.Content = "Załadowano"; //tutaj nie potrafie wczytać nazwy obrazka :(
                    exhibits_selectedImageBox.Source = image;
                }       
                
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
            if(exhibits_exhibitsDb.SelectedItem != null)
            {
                // Pobranie zaznaczonego rekordu (wiersza) jako obiekt
                var selectedItem = (DataRowView)exhibits_exhibitsDb.SelectedItem;
                int id = Convert.ToInt32(selectedItem["id"]);
                exhibits_selectedImageBox.Source = null;

                dbConnect.DeleteExhibits(id);                
                loadGrid();                
            }
            else
            {
                MessageBox.Show("Proszę zaznaczyć rekord do usunięcia.");
            }
        }

        private void ExhibitionsBtn_Click(object sender, RoutedEventArgs e)
        {
            Exhibitions Exhibitions = new Exhibitions();
            Exhibitions.Show();
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

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Hide();
            MessageBox.Show("Pomyślnie wylogowano!");
        }

        private void exhibitsEdit_saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (DataRowView)exhibits_exhibitsDb.SelectedItem;
            int id = Convert.ToInt32(selectedItem["id"]);
            string name = exhibitsEdit_nameTxt.Text;
            string description = exhibitsEdit_descriptionTxt.Text;
            int year = Convert.ToInt32(exhibitsEdit_yearTxt.Text);
            string category = exhibitsEdit_categoryList.Text;
            string author = exhibitsEdit_authorTxt.Text;
            string origin = exhibitsEdit_originTxt.Text;
            BitmapImage image = imageBytes != null ? convertBytesToBitmap(imageBytes) : null;
            string location = exhibitsEdit_locationTxt.Text;

            // Wywołaj metodę aktualizacji
            AddExhibits addExhibits = new AddExhibits(id, name, description, year, category, author, origin, image, location);
            dbConnect.UpdateExhibits(addExhibits);
            loadGrid();
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
    }
}
