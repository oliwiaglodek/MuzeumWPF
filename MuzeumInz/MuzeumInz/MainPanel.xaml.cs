﻿using System;
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
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Threading;
using System.Data.Entity;
namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy MainPanel.xaml
    /// </summary>
    public partial class MainPanel : Window
    {

        private DbConnect dbConnect;
        public MainPanel()
        {
            InitializeComponent();
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
