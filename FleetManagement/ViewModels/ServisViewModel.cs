using FleetManagement.Data;
using FleetManagement.Models;
using FleetManagement.Utils;
using FleetManagement.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FleetManagement.ViewModels
{
    public class ServisViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;

        public ObservableCollection<Servis> Servisi { get; set; }
        public ObservableCollection<Vozilo> Vozila { get; set; }
        public ICollectionView ServisiView { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ServisiView.Refresh();
            }
        }

        private Servis _selektovaniServis;
        public Servis SelektovaniServis
        {
            get => _selektovaniServis;
            set => SetProperty(ref _selektovaniServis, value);
        }

        private Servis _noviServis = new Servis { Datum = DateTime.Today };
        public Servis NoviServis
        {
            get => _noviServis;
            set => SetProperty(ref _noviServis, value);
        }

        private Vozilo _izabranoVozilo;
        public Vozilo IzabranoVozilo
        {
            get => _izabranoVozilo;
            set => SetProperty(ref _izabranoVozilo, value);
        }

        public ICommand OtvoriDodajServisCommand { get; }
        public ICommand OtvoriIzmeniServisCommand { get; }
        public ICommand SacuvajServisCommand { get; }
        public ICommand SacuvajIzmeneServisCommand { get; }
        public ICommand ObrisiServisCommand { get; }
        public ICommand IzvestajServisiPoVoziluCommand { get; }
        public ICommand IzvestajBrojServisaCommand { get; }
        public ICommand ExportServisiCommand { get; }
        public ICommand ImportServisiCommand { get; }


        public ServisViewModel(AppDbContext context)
        {
            _context = context;

            Vozila = new ObservableCollection<Vozilo>(_context.Vozila.ToList());
            Servisi = new ObservableCollection<Servis>(context.Servisi.Include(s => s.Vozilo).ToList());
            ServisiView = CollectionViewSource.GetDefaultView(Servisi);
            ServisiView.Filter = FilterServisi;

            OtvoriDodajServisCommand = new RelayCommand(_ => OtvoriDodajServis());
            OtvoriIzmeniServisCommand = new RelayCommand(_ => OtvoriIzmeniServis(), _ => SelektovaniServis != null);
            SacuvajServisCommand = new RelayCommand(_ => SacuvajServis());
            SacuvajIzmeneServisCommand = new RelayCommand(_ => SacuvajIzmeneServis(), _ => SelektovaniServis != null);
            ObrisiServisCommand = new RelayCommand(_ => ObrisiServis(), _ => SelektovaniServis != null);
            IzvestajServisiPoVoziluCommand = new RelayCommand(_ => GenerisiIzvestajServisiPoVozilu());
            IzvestajBrojServisaCommand = new RelayCommand(_ => GenerisiIzvestajBrojServisa());
            ExportServisiCommand = new RelayCommand(_ => ExportServisi());
            ImportServisiCommand = new RelayCommand(_ => ImportServisi());

        }
        private bool FilterServisi(object obj)
        {
            if (obj is Servis s)
            {
                if (string.IsNullOrEmpty(SearchText)) return true;
                return s.Opis.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || s.Vozilo.Registracija.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || s.Vozilo.Marka.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || s.Vozilo.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
        private void OtvoriDodajServis()
        {
            var view = new DodajServisView();
            view.DataContext = this;
            view.ShowDialog();
        }

        private void OtvoriIzmeniServis()
        {
            var view = new IzmeniServisView();
            view.DataContext = this;
            view.ShowDialog();
        }

        private void SacuvajServis()
        {
            if (NoviServis == null || IzabranoVozilo == null)
            {
                MessageBox.Show("Morate uneti podatke i izabrati vozilo.",
                                "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(NoviServis.Opis))
            {
                MessageBox.Show("Opis servisa je obavezan.", "Greška",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NoviServis.Datum == default)
            {
                MessageBox.Show("Morate izabrati datum servisa.", "Greška",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NoviServis.VoziloId = IzabranoVozilo.Id;

            _context.Servisi.Add(NoviServis);
            _context.SaveChanges();

            Servisi.Add(NoviServis);

            NoviServis = new Servis();
            IzabranoVozilo = null;

            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w is DodajServisView)
                ?.Close();
        }

        private void SacuvajIzmeneServis()
        {
            if (SelektovaniServis == null)
                return;

            _context.Servisi.Update(SelektovaniServis);
            _context.SaveChanges();

            OnPropertyChanged(nameof(Servisi));

            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w is IzmeniServisView)
                ?.Close();
        }

        private void ObrisiServis()
        {
            if (SelektovaniServis == null) return;

            _context.Servisi.Remove(SelektovaniServis);
            _context.SaveChanges();

            Servisi.Remove(SelektovaniServis);
        }

        private void GenerisiIzvestajServisiPoVozilu()
        {
            var report = ReportGenerator.ServisiPoVozilu(Servisi.ToList(), Vozila.ToList());

            var sb = new StringBuilder();
            foreach (var item in report)
            {
                sb.AppendLine($"{item.Vozilo} | {item.Datum} | {item.Opis}");
            }

            MessageBox.Show(sb.ToString(), "Izveštaj Servisi po vozilu");
        }

        private void GenerisiIzvestajBrojServisa()
        {
            var report = ReportGenerator.BrojServisaPoVozilu(Servisi.ToList(), Vozila.ToList());

            var sb = new StringBuilder();
            foreach (var item in report)
            {
                sb.AppendLine($"{item.Vozilo} | Broj servisa: {item.BrojServisa}");
            }

            MessageBox.Show(sb.ToString(), "Izveštaj Broj servisa po vozilu");
        }


        private void ExportServisi()
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Sačuvaj servise",
                    Filter = "JSON fajl (*.json)|*.json",
                    FileName = "servisi.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    FleetManagement.Utils.DataSerializer.SaveServisi(Servisi.ToList(), dialog.FileName);
                    MessageBox.Show($"Servisi su uspešno eksportovani u {dialog.FileName}",
                                    "Eksport", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri eksportu: {ex.Message}", "Greška",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ImportServisi()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Title = "Učitaj servise",
                    Filter = "JSON fajl (*.json)|*.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    var ucitani = FleetManagement.Utils.DataSerializer.LoadServisi(dialog.FileName);

                    Servisi.Clear();
                    foreach (var s in ucitani)
                        Servisi.Add(s);

                    MessageBox.Show($"Servisi su uspešno učitani iz {dialog.FileName}",
                                    "Import", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri importu: {ex.Message}", "Greška",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}