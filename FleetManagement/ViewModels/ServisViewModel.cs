using FleetManagement.Data;
using FleetManagement.Models;
using FleetManagement.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FleetManagement.ViewModels
{
    public class ServisViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;

        public ObservableCollection<Servis> Servisi { get; set; }
        public ObservableCollection<Vozilo> Vozila { get; set; }

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

        // Komande
        public ICommand OtvoriDodajServisCommand { get; }
        public ICommand OtvoriIzmeniServisCommand { get; }
        public ICommand SacuvajServisCommand { get; }
        public ICommand SacuvajIzmeneServisCommand { get; }
        public ICommand ObrisiServisCommand { get; }

        public ServisViewModel(AppDbContext context)
        {
            _context = context;

            Servisi = new ObservableCollection<Servis>(_context.Servisi.ToList());
            Vozila = new ObservableCollection<Vozilo>(_context.Vozila.ToList());

            OtvoriDodajServisCommand = new RelayCommand(_ => OtvoriDodajServis());
            OtvoriIzmeniServisCommand = new RelayCommand(_ => OtvoriIzmeniServis(), _ => SelektovaniServis != null);
            SacuvajServisCommand = new RelayCommand(_ => SacuvajServis());
            SacuvajIzmeneServisCommand = new RelayCommand(_ => SacuvajIzmeneServis(), _ => SelektovaniServis != null);
            ObrisiServisCommand = new RelayCommand(_ => ObrisiServis(), _ => SelektovaniServis != null);
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
    }
}