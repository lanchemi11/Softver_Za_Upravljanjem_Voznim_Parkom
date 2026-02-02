using FleetManagement.Utils;
using FleetManagement.Data;
using FleetManagement.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PdfSharp.Pdf;

namespace FleetManagement.ViewModels
{
    public class IzvestajiViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;

        public ICommand IzvestajServisiPoVoziluCommand { get; }
        public ICommand IzvestajBrojServisaCommand { get; }
        public ICommand ExportServisiCommand { get; }

        public IzvestajiViewModel(AppDbContext context)
        {
            _context = context;

            IzvestajServisiPoVoziluCommand = new RelayCommand(_ => GenerisiIzvestajServisiPoVozilu());
            IzvestajBrojServisaCommand = new RelayCommand(_ => GenerisiIzvestajBrojServisa());
            ExportServisiCommand = new RelayCommand(_ => ExportServisi());
        }

        private void GenerisiIzvestajServisiPoVozilu()
        {
            var servisi = _context.Servisi.ToList();
            var vozila = _context.Vozila.ToList();
            var report = ReportGenerator.ServisiPoVozilu(servisi, vozila);

            var dialog = new SaveFileDialog
            {
                Title = "Sačuvaj izveštaj",
                Filter = "PDF fajl (*.pdf)|*.pdf",
                FileName = "ServisiPoVozilu.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                PdfHelper.GenerateServisiPoVozilu(report, dialog.FileName);
                MessageBox.Show("PDF izveštaj je uspešno generisan!", "Izveštaji",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GenerisiIzvestajBrojServisa()
        {
            var servisi = _context.Servisi.ToList();
            var vozila = _context.Vozila.ToList();
            var report = ReportGenerator.BrojServisaPoVozilu(servisi, vozila);

            var dialog = new SaveFileDialog
            {
                Title = "Sačuvaj izveštaj",
                Filter = "PDF fajl (*.pdf)|*.pdf",
                FileName = "BrojServisaPoVozilu.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                PdfHelper.GenerateBrojServisaPoVozilu(report, dialog.FileName);
                MessageBox.Show("PDF izveštaj je uspešno generisan!", "Izveštaji",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportServisi()
        {
            var dialog = new SaveFileDialog
            {
                Title = "Sačuvaj JSON",
                Filter = "JSON fajl (*.json)|*.json",
                FileName = "servisi.json"
            };

            if (dialog.ShowDialog() == true)
            {
                DataSerializer.SaveServisi(_context.Servisi.ToList(), dialog.FileName);
                MessageBox.Show("Servisi su uspešno eksportovani u JSON!", "Eksport",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}