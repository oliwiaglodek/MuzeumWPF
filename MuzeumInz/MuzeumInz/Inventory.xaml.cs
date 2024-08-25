﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Logika interakcji dla klasy Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        private DbConnect dbConnect;
        public Inventory()
        {
            InitializeComponent();
        }

        //Zamknięcie na "X"
        private void exitClick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            dbConnect.ClearCurrentUser();
            this.Hide();
            MessageBox.Show("Pomyślnie wylogowano!");
        }

        private void ExhibitsBtn_Click(object sender, RoutedEventArgs e)
        {
            Exhibits Exhibits = new Exhibits();
            Exhibits.Show();
            this.Hide();
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
    }
}
