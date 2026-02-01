using FleetManagement.Data;
using FleetManagement.Models;
using FleetManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FleetManagement.ViewModels
{
    public class VoziloViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;

        public ObservableCollection<Vozilo> Vozila { get; set; }

        private Vozilo _selektovanoVozilo;
        public Vozilo SelektovanoVozilo
        {
            get => _selektovanoVozilo;
            set => SetProperty(ref _selektovanoVozilo, value);
        }

        private Vozilo _novoVozilo = new Vozilo();
        public Vozilo NovoVozilo
        {
            get => _novoVozilo;
            set => SetProperty(ref _novoVozilo, value);
        }

        public ICommand ObrisiVoziloCommand { get; }
        public ICommand IzmeniVoziloCommand { get; }
        public ICommand OtvoriDodajFormuCommand { get; }
        public ICommand DodajVoziloCommand { get; }
        public ICommand OtvoriIzmeniFormuCommand { get; }
        public ICommand SacuvajIzmeneCommand { get; }


        public VoziloViewModel(AppDbContext context)
        {
            _context = context;
            Vozila = new ObservableCollection<Vozilo>(_context.Vozila.ToList());


            ObrisiVoziloCommand = new RelayCommand(_ => ObrisiVozilo(), _ => SelektovanoVozilo != null);
            IzmeniVoziloCommand = new RelayCommand(_ => IzmeniVozilo(), _ => SelektovanoVozilo != null);
            OtvoriDodajFormuCommand = new RelayCommand(_ => OtvoriDodajFormu());
            OtvoriIzmeniFormuCommand = new RelayCommand(_ => OtvoriIzmeniFormu());
            DodajVoziloCommand = new RelayCommand(_ => DodajVozilo());
            SacuvajIzmeneCommand = new RelayCommand(_ => SacuvajIzmene(), _ => SelektovanoVozilo != null);
        }

        private void ObrisiVozilo()
        {
            if (SelektovanoVozilo == null) return;

            _context.Vozila.Remove(SelektovanoVozilo);
            _context.SaveChanges();

            Vozila.Remove(SelektovanoVozilo);
        }

        private void IzmeniVozilo()
        {
            if (SelektovanoVozilo == null) return;

            _context.Vozila.Update(SelektovanoVozilo);
            _context.SaveChanges();

            OnPropertyChanged(nameof(Vozila));
        }

        private void OtvoriDodajFormu()
        {
            var dodajView = new DodajVoziloView();
            dodajView.DataContext = this;
            dodajView.ShowDialog();
        }

        private void DodajVozilo()
        {
            if (string.IsNullOrWhiteSpace(NovoVozilo.Marka) ||
                string.IsNullOrWhiteSpace(NovoVozilo.Model) ||
                string.IsNullOrWhiteSpace(NovoVozilo.Registracija))
            {
                MessageBox.Show("Molimo popunite Marka, Model i Registraciju.",
                                "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NovoVozilo.GodinaProizvodnje <= 1900 || NovoVozilo.GodinaProizvodnje > DateTime.Now.Year)
            {
                MessageBox.Show("Godina proizvodnje mora biti između 1900 i trenutne godine.",
                                "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _context.Vozila.Add(NovoVozilo);
            _context.SaveChanges();

            Vozila.Add(NovoVozilo);

            NovoVozilo = new Vozilo();

            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w is DodajVoziloView)
                ?.Close();
        }

        private void OtvoriIzmeniFormu()
        {
            var izmeniView = new IzmeniVoziloView();
            izmeniView.DataContext = this;
            izmeniView.ShowDialog();
        }

        private void SacuvajIzmene()
        {
            if (SelektovanoVozilo == null) return;

            _context.Vozila.Update(SelektovanoVozilo);
            _context.SaveChanges();

            OnPropertyChanged(nameof(Vozila));

            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w is IzmeniVoziloView)
                ?.Close();
        }

    }

}
