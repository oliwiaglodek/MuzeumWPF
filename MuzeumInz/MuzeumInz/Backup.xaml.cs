using System;
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
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Data.Common;

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy Backup.xaml
    /// </summary>
    public partial class Backup : Window
    {
        DbConnect dbConnect;
        public Backup()
        {
            InitializeComponent();
            LoadBackupList();
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

        private readonly string _backupDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Backup");
        private readonly string _sourceFile = "Muzeum.db";

        // Funkcja eksportu bazy danych z szyfrowaniem
        private async void BackupDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadingSpinner.Visibility = Visibility.Visible;
            LoadingText.Visibility = Visibility.Visible;

            await Task.Run(() =>
            {
                string backupFile = System.IO.Path.Combine(_backupDirectory, $"Muzeum_{DateTime.Now:yyyyMMdd_HHmmss}.db");
                string encryptedBackupFile = backupFile + ".enc";

                try
                {
                    if (!Directory.Exists(_backupDirectory))
                    {
                        Directory.CreateDirectory(_backupDirectory);
                    }

                    // Tworzenie kopii zapasowej
                    File.Copy(_sourceFile, backupFile, overwrite: true);

                    // Szyfrowanie pliku kopii zapasowej
                    EncryptFile(backupFile, encryptedBackupFile);

                    // Usunięcie nieszyfrowanej kopii
                    File.Delete(backupFile);

                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Kopia zapasowa została utworzona i zabezpieczona: {encryptedBackupFile}");
                        LoadBackupList(); // Odświeżenie listy rozwijanej
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Błąd podczas tworzenia kopii zapasowej: " + ex.Message);
                    });
                }
            });

            LoadingSpinner.Visibility = Visibility.Collapsed;
            LoadingText.Visibility = Visibility.Collapsed;
        }

        // Funkcja importu i przywrócenia bazy danych z listy
        private async void ImportDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BackupList.SelectedItem is string selectedBackup)
            {
                string encryptedBackupFile = System.IO.Path.Combine(_backupDirectory, selectedBackup);
                string tempDecryptedFile = System.IO.Path.Combine(_backupDirectory, "temp.db"); // Tymczasowy plik

                LoadingSpinner2.Visibility = Visibility.Visible;
                LoadingText2.Visibility = Visibility.Visible;

                await Task.Run(() =>
                {
                    try
                    {
                        // Odszyfrowanie pliku kopii zapasowej do tymczasowego pliku
                        DecryptFile(encryptedBackupFile, tempDecryptedFile);

                        // Zamknięcie połączenia z aktualną bazą danych
                        CloseDatabaseConnections();

                        // Podmiana bazy danych
                        File.Copy(tempDecryptedFile, _sourceFile, overwrite: true);

                        // Usunięcie tymczasowego pliku
                        File.Delete(tempDecryptedFile);

                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Baza danych została pomyślnie przywrócona.");
                        });
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Błąd podczas przywracania bazy danych: " + ex.Message);
                        });
                    }
                });

                LoadingSpinner2.Visibility = Visibility.Collapsed;
                LoadingText2.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Wybierz kopię zapasową z listy.");
            }
        }

        private void CloseDatabaseConnections()
        {
            // Jeśli używasz globalnego obiektu połączenia
            if (dbConnect != null)
            {
                dbConnect.Dispose();
                dbConnect = null; // Ustawienie na null pozwala na utworzenie nowego połączenia w razie potrzeby
            }
        }


        // Funkcja szyfrowania pliku
        private void EncryptFile(string sourceFile, string destinationFile)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("1234567890123456"); // Klucz szyfrowania
                aesAlg.IV = Encoding.UTF8.GetBytes("1234567890123456"); // Wektor inicjalizacyjny

                using (FileStream fsOutput = new FileStream(destinationFile, System.IO.FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsOutput, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(sourceFile, System.IO.FileMode.Open))
                        {
                            fsInput.CopyTo(cs);
                        }
                    }
                }
            }
        }

        // Funkcja odszyfrowania pliku
        private void DecryptFile(string sourceFile, string destinationFile)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("1234567890123456"); // Klucz szyfrowania
                aesAlg.IV = Encoding.UTF8.GetBytes("1234567890123456"); // Wektor inicjalizacyjny

                using (FileStream fsInput = new FileStream(sourceFile,FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(destinationFile, FileMode.Create))
                        {
                            cs.CopyTo(fsOutput);
                        }
                    }
                }
            }
        }

        // Ładowanie listy kopii zapasowych
        private void LoadBackupList()
        {
            if (Directory.Exists(_backupDirectory))
            {
                var files = Directory.GetFiles(_backupDirectory, "*.enc")
                                     .Select(System.IO.Path.GetFileName)
                                     .ToList();

                BackupList.ItemsSource = files; // Załaduj listę kopii do ComboBoxa
            }
        }
    }
}
