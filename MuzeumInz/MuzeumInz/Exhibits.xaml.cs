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

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy Exhibits.xaml
    /// </summary>
    public partial class Exhibits : Window
    {
        private BitmapImage image;        
        private DbConnect dbConnect;
        public Exhibits()
        {
            InitializeComponent();
            dbConnect = new DbConnect();

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //Widoczność pól dodania nowego eksponatu
        private void exhibits_addBtn_Click(object sender, RoutedEventArgs e)
        {
            exhibits_addGrid.Visibility = Visibility.Visible;
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
                //exhibitsEdit_imageBtn.Text = selectedRow["Image"].ToString(); //do przemyślenia
                exhibitsEdit_nameTxt.Focus();
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
                AddExhibits exhibit = new AddExhibits(0, exhibits_nameTxt.Text, exhibits_descriptionTxt.Text, year, ((ComboBoxItem)exhibits_categoryList.SelectedItem).Content.ToString(), exhibits_authorTxt.Text, exhibits_originTxt.Text, /*image*/ exhibits_locationTxt.Text);
                dbConnect.InsertExhibits(exhibit);
                loadGrid();
            }
        }
        //Przycisk edytuj
        private void exhibits_editBtn_Click(object sender, RoutedEventArgs e)
        {
            exhibits_editGrid.Visibility = Visibility.Visible;
        }
        
    }
}
