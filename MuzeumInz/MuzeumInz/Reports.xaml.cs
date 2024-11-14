using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Controls;
using System.Threading; //możliwość zapisu pliku wedle uznania

namespace MuzeumInz
{
    /// <summary>
    /// Logika interakcji dla klasy Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        DbConnect dbConnect;
        public Reports()
        {        
            dbConnect = new DbConnect();
            InitializeComponent();
        }

        private List<History> GetHistoryRecordsFromDatabase()
        {
            // Implementacja logiki pobierania danych z bazy danych za pomocą klasy DbConnect
            return dbConnect.GetHistory();
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
                existingWindow.Focus();  // Skupiamy się na istniejącym oknie
            }
        }


        private void Backup_Click(object sender, RoutedEventArgs e)
        {
            var existingWindow = Application.Current.Windows.OfType<Backup>().FirstOrDefault();

            if (existingWindow == null)
            {
                using (DbConnect dbConnect = new DbConnect())
                {
                    Backup BackupWindow = new Backup();
                    BackupWindow.Show();
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

        private async void SaveHistoryPDF(object sender, RoutedEventArgs e) //async bo przetwarzamy tyle ile trwa proces
        {
            QuestPDF.Settings.License = LicenseType.Community;

            // Pokaż animację ładowania
            LoadingSpinner.Visibility = Visibility.Visible;
            LoadingText.Visibility = Visibility.Visible;

            // Uruchom generowanie PDF w tle, aby nie blokować interfejsu użytkownika
            await Task.Run(() =>
            {
                try
                {
                    // Odczytaj dane z bazy danych
                    List<History> historyRecords = GetHistoryRecordsFromDatabase();

                    // Użycie SaveFileDialog, aby pozwolić użytkownikowi wybrać lokalizację zapisu pliku
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf"; // Filtr plików PDF
                    saveFileDialog.Title = "Zapisz raport jako PDF";

                    // Jeśli użytkownik wybierze lokalizację i kliknie "Zapisz"
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filePath = saveFileDialog.FileName;



                        // Tworzenie dokumentu PDF
                        Document.Create(container =>
                        {
                            container.Page(page =>
                            {
                                page.Size(PageSizes.A4);
                                page.Margin(1, Unit.Centimetre);
                                page.PageColor(QuestPDF.Helpers.Colors.White);
                                page.DefaultTextStyle(x => x.FontSize(10));

                                // Nagłówek dokumentu
                                page.Header()
                                    .Text("Historia zmian")
                                    .SemiBold().FontSize(20).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                                // Zawartość dokumentu: tabela
                                page.Content()
                                    .Table(table =>
                                    {
                                        // Definicja kolumn
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(2); // Typ (TableName)
                                            columns.RelativeColumn(1); // ID Rekordu (RecordId)
                                            columns.RelativeColumn(2); // Operacja (Operation)
                                            columns.RelativeColumn(2); // Użytkownik (ChangedBy)
                                            columns.RelativeColumn(2); // Czas (ChangedAt)
                                            columns.RelativeColumn(3); // Przed (OldValues)
                                            columns.RelativeColumn(3); // Po (NewValues)
                                        });

                                        // Nagłówki tabeli
                                        table.Header(header =>
                                        {
                                            header.Cell().Element(CellStyle).Text("Typ");
                                            header.Cell().Element(CellStyle).Text("ID");
                                            header.Cell().Element(CellStyle).Text("Operacja");
                                            header.Cell().Element(CellStyle).Text("Użytkownik");
                                            header.Cell().Element(CellStyle).Text("Czas");
                                            header.Cell().Element(CellStyle).Text("Przed");
                                            header.Cell().Element(CellStyle).Text("Po");

                                            QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer cellContainer)
                                            {
                                                return cellContainer.DefaultTextStyle(x => x.SemiBold()).Padding(5).BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                                    .EnsureSpace(); // Dodano łamanie słów
                                            }
                                        });

                                        // Dane tabeli
                                        foreach (var record in historyRecords)
                                        {
                                            table.Cell().Element(BodyCellStyle).Text(record.tableName);
                                            table.Cell().Element(BodyCellStyle).Text(record.recordId.ToString());
                                            table.Cell().Element(BodyCellStyle).Text(record.operation);
                                            table.Cell().Element(BodyCellStyle).Text(record.changed_by);
                                            table.Cell().Element(BodyCellStyle).Text(record.changed_at.ToString("yyyy-MM-dd HH:mm:ss"));

                                            // Wartości przed i po (z przetworzeniem tekstu)
                                            table.Cell().Element(BodyCellStyle).Text(FormatJsonValues(record.old_values));
                                            table.Cell().Element(BodyCellStyle).Text(FormatJsonValues(record.new_values));

                                            QuestPDF.Infrastructure.IContainer BodyCellStyle(QuestPDF.Infrastructure.IContainer bodyCell)
                                            {
                                                return bodyCell.Padding(5); // Dodano łamanie słów
                                            }
                                        }
                                    });

                                // Stopka dokumentu
                                page.Footer()
                                    .AlignCenter()
                                    .Text(x =>
                                    {
                                        x.CurrentPageNumber();
                                        x.Span(" / ");
                                        x.TotalPages();
                                    });
                            });
                        })
                        .GeneratePdf(filePath); // Zapisuje plik PDF w wybranej przez użytkownika ścieżce
                        MessageBox.Show("PDF zostało zapisane pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd podczas generowania: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                    
            });
            //ukryj animację ładowania po zakończeniu generowania PDF
            LoadingSpinner.Visibility = Visibility.Collapsed;
            LoadingText.Visibility = Visibility.Collapsed;
        }

        private async void PreviewAndPrint(object sender, RoutedEventArgs e)
        {
            // Pokaż animację ładowania
            LoadingSpinner.Visibility = Visibility.Visible;
            LoadingText.Visibility = Visibility.Visible;

            // Uruchom generowanie PDF w tle, aby nie blokować interfejsu użytkownika
            await Task.Run(() =>
            {
                try
            {
                // Ścieżka do folderu Temp w katalogu roboczym aplikacji
                string tempFolderPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Temp");

                // Tworzenie folderu Temp, jeśli jeszcze nie istnieje
                if (!System.IO.Directory.Exists(tempFolderPath))
                {
                    System.IO.Directory.CreateDirectory(tempFolderPath);
                }

                // Ścieżka zapisu pliku PDF w folderze Temp
                string filePath = System.IO.Path.Combine(tempFolderPath, "preview.pdf");

                // Generowanie dokumentu PDF
                QuestPDF.Settings.License = LicenseType.Community;
                List<History> historyRecords = GetHistoryRecordsFromDatabase();

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(QuestPDF.Helpers.Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        // Nagłówek dokumentu
                        page.Header()
                            .Text("Historia zmian")
                            .SemiBold().FontSize(20).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                        // Zawartość dokumentu: tabela
                        page.Content()
                            .Table(table =>
                            {
                                // Definicja kolumn
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2); // Typ (TableName)
                                    columns.RelativeColumn(1); // ID Rekordu (RecordId)
                                    columns.RelativeColumn(2); // Operacja (Operation)
                                    columns.RelativeColumn(2); // Użytkownik (ChangedBy)
                                    columns.RelativeColumn(2); // Czas (ChangedAt)
                                    columns.RelativeColumn(3); // Przed (OldValues)
                                    columns.RelativeColumn(3); // Po (NewValues)
                                });

                                // Nagłówki tabeli
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Typ");
                                    header.Cell().Element(CellStyle).Text("ID");
                                    header.Cell().Element(CellStyle).Text("Operacja");
                                    header.Cell().Element(CellStyle).Text("Użytkownik");
                                    header.Cell().Element(CellStyle).Text("Czas");
                                    header.Cell().Element(CellStyle).Text("Przed");
                                    header.Cell().Element(CellStyle).Text("Po");

                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer cellContainer)
                                    {
                                        return cellContainer.DefaultTextStyle(x => x.SemiBold()).Padding(5).BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black)
                                            .EnsureSpace();
                                    }
                                });

                                // Dane tabeli
                                foreach (var record in historyRecords)
                                {
                                    table.Cell().Element(BodyCellStyle).Text(record.tableName);
                                    table.Cell().Element(BodyCellStyle).Text(record.recordId.ToString());
                                    table.Cell().Element(BodyCellStyle).Text(record.operation);
                                    table.Cell().Element(BodyCellStyle).Text(record.changed_by);
                                    table.Cell().Element(BodyCellStyle).Text(record.changed_at.ToString("yyyy-MM-dd HH:mm:ss"));
                                    table.Cell().Element(BodyCellStyle).Text(FormatJsonValues(record.old_values));
                                    table.Cell().Element(BodyCellStyle).Text(FormatJsonValues(record.new_values));

                                    QuestPDF.Infrastructure.IContainer BodyCellStyle(QuestPDF.Infrastructure.IContainer bodyCell)
                                    {
                                        return bodyCell.Padding(5);
                                    }
                                }
                            });

                        // Stopka dokumentu
                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.CurrentPageNumber();
                                x.Span(" / ");
                                x.TotalPages();
                            });
                    });
                }).GeneratePdf(filePath); // Zapisuje plik PDF w folderze Temp

                // Otwórz plik PDF za pomocą domyślnej aplikacji
                Process pdfProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    }
                };

                pdfProcess.Start();

                // Usunięcie pliku po zamknięciu aplikacji przeglądającej PDF
                Task.Run(() =>
                {
                    pdfProcess.WaitForExit();

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas generowania lub otwierania pliku PDF: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            });
            //ukryj animację ładowania po zakończeniu generowania PDF
            LoadingSpinner.Visibility = Visibility.Collapsed;
            LoadingText.Visibility = Visibility.Collapsed;

        }

        private async void SaveHistoryCSV(object sender, RoutedEventArgs e)
        {
             // Pokaż animację ładowania
            LoadingSpinner.Visibility = Visibility.Visible;
            LoadingText.Visibility = Visibility.Visible;

            // Uruchom generowanie PDF w tle, aby nie blokować interfejsu użytkownika
            await Task.Run(() =>
            {
                try
            {
            // Odczytanie danych z bazy danych
            List<History> historyRecords = GetHistoryRecordsFromDatabase();

            // Użycie SaveFileDialog, aby pozwolić użytkownikowi wybrać lokalizację zapisu pliku
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv"; // Filtr plików CSV
            saveFileDialog.Title = "Zapisz raport jako CSV";

            // Jeśli użytkownik wybierze lokalizację i kliknie "Zapisz"
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                // Zapisz dane do pliku CSV
                using (var writer = new System.IO.StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Zapisz nagłówki CSV
                    writer.WriteLine("Typ,ID,Operacja,Użytkownik,Czas,Przed,Po");

                    // Zapisz dane wiersz po wierszu
                    foreach (var record in historyRecords)
                    {
                        var oldValuesFormatted = FormatJsonValues(record.old_values);
                        var newValuesFormatted = FormatJsonValues(record.new_values);

                        // Zapisanie wiersza w formacie CSV
                        string line = $"{record.tableName},{record.recordId},{record.operation},{record.changed_by}," +
                                      $"{record.changed_at:yyyy-MM-dd HH:mm:ss},{oldValuesFormatted},{newValuesFormatted}";

                        writer.WriteLine(line);
                    }
                }

                MessageBox.Show("CSV zostało zapisane pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd podczas generowania lub otwierania pliku PDF: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
            //ukryj animację ładowania po zakończeniu generowania PDF
            LoadingSpinner.Visibility = Visibility.Collapsed;
            LoadingText.Visibility = Visibility.Collapsed;
        }

        // Funkcja do formatowania wartości w kolumnach "Przed" i "Po" do formatu kolumna:wartość,
        private string FormatJsonValues(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            // Usuwamy nawiasy klamrowe i cudzysłowy
            value = value.Trim(new char[] { '{', '}', '"' });

            // Zamieniamy przecinki na format "kolumna: wartość"
            var formattedValues = new List<string>();
            var keyValuePairs = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in keyValuePairs)
            {
                var keyValue = pair.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (keyValue.Length == 2)
                {
                    // Usuwamy nadmiarowe białe znaki oraz ewentualne cudzysłowy
                    formattedValues.Add($"{keyValue[0].Trim().Replace("\"", "")}: {keyValue[1].Trim().Replace("\"", "")}");
                }
            }

            // Łączymy wyniki w jedną linię
            return string.Join(", ", formattedValues);
        }



        // Funkcja do przetwarzania wartości w polach "Przed" i "Po"
        private string ProcessValues(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            // Usuwanie cudzysłowów i nadmiarowych nawiasów
            value = value.Replace("\"", "").Replace("\\n", " ").Replace("\\", "");

            // Dodatkowa obróbka, np. usunięcie białych znaków z początku i końca
            return value.Trim();
        }
   
    }
}
