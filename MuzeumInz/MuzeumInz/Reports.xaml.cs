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
using Microsoft.Win32; //możliwość zapisu pliku wedle uznania

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

        private void SaveHistoryPDF(object sender, RoutedEventArgs e)
        {
            QuestPDF.Settings.License = LicenseType.Community;

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
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(QuestPDF.Helpers.Colors.White); //Musiałem zastosować pełna nazwę ponieważ inaczej te same odwołania ma system.windows.media.colors.white
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
                                    columns.RelativeColumn(2); // TableName
                                    columns.RelativeColumn(3); // RecordId
                                    columns.RelativeColumn(4); // Operation
                                    columns.RelativeColumn(5); // ChangedBy
                                    columns.RelativeColumn(6); // ChangedAt
                                    columns.RelativeColumn(7); // OldValues
                                    columns.RelativeColumn(8); // NewValues
                                });

                                // Nagłówki tabeli
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Table Name");
                                    header.Cell().Element(CellStyle).Text("Record ID");
                                    header.Cell().Element(CellStyle).Text("Operation");
                                    header.Cell().Element(CellStyle).Text("Changed By");
                                    header.Cell().Element(CellStyle).Text("Changed At");
                                    header.Cell().Element(CellStyle).Text("Old Values");
                                    header.Cell().Element(CellStyle).Text("New Values");

                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer cellContainer)
                                    {
                                        return cellContainer.DefaultTextStyle(x => x.SemiBold()).Padding(5).BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black);
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
                                    table.Cell().Element(BodyCellStyle).Text(record.old_values);
                                    table.Cell().Element(BodyCellStyle).Text(record.new_values);

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
                })
                .GeneratePdf(filePath); // Zapisuje plik PDF w wybranej przez użytkownika ścieżce
            }
        }



    }
}
