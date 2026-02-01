using FleetManagement.Data;
using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // Komande
        public ICommand DodajVoziloCommand { get; }
        public ICommand ObrisiVoziloCommand { get; }
        public ICommand IzmeniVoziloCommand { get; }

        public VoziloViewModel(AppDbContext context)
        {
            _context = context;

            try
            {
                Vozila = new ObservableCollection<Vozilo>(_context.Vozila.ToList());
            }
            catch (Exception ex)
            {
                Vozila = new ObservableCollection<Vozilo>();
                System.Windows.MessageBox.Show($"Greška pri učitavanju vozila: {ex.Message}");
            }


            DodajVoziloCommand = new RelayCommand(_ => DodajVozilo());
            ObrisiVoziloCommand = new RelayCommand(_ => ObrisiVozilo(), _ => SelektovanoVozilo != null);
            IzmeniVoziloCommand = new RelayCommand(_ => IzmeniVozilo(), _ => SelektovanoVozilo != null);
        }

        private void DodajVozilo()
        {
            var novo = new Vozilo
            {
                Marka = "Dacia",
                Model = "Logan",
                GodinaProizvodnje = 2006,
                Registracija = "CU-003-NS",
                Status = "Aktivno"
            };

            _context.Vozila.Add(novo);
            _context.SaveChanges();

            Vozila.Add(novo);
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

            // primer izmene
            SelektovanoVozilo.Status = "Servisirano";
            _context.Vozila.Update(SelektovanoVozilo);
            _context.SaveChanges();

            // osvežavanje UI-a
            OnPropertyChanged(nameof(Vozila));
        }
    }

}
