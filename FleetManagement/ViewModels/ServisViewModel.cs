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
    public class ServisViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;

        public ObservableCollection<Servis> Servisi { get; set; }

        private Servis _selektovaniServis;
        public Servis SelektovaniServis
        {
            get => _selektovaniServis;
            set => SetProperty(ref _selektovaniServis, value);
        }

        public ICommand DodajServisCommand { get; }
        public ICommand ObrisiServisCommand { get; }
        public ICommand IzmeniServisCommand { get; }

        public ServisViewModel(AppDbContext context)
        {
            _context = context;

            Servisi = new ObservableCollection<Servis>(_context.Servisi.ToList());

            DodajServisCommand = new RelayCommand(_ => DodajServis());
            ObrisiServisCommand = new RelayCommand(_ => ObrisiServis(), _ => SelektovaniServis != null);
            IzmeniServisCommand = new RelayCommand(_ => IzmeniServis(), _ => SelektovaniServis != null);
        }

        private void DodajServis()
        {
            var novi = new Servis
            {
                Opis = "Redovan servis",
                Datum = System.DateTime.Now,
                VoziloId = _context.Vozila.First().Id // primer: vezujemo za prvo vozilo
            };

            _context.Servisi.Add(novi);
            _context.SaveChanges();

            Servisi.Add(novi);
        }

        private void ObrisiServis()
        {
            if (SelektovaniServis == null) return;

            _context.Servisi.Remove(SelektovaniServis);
            _context.SaveChanges();

            Servisi.Remove(SelektovaniServis);
        }

        private void IzmeniServis()
        {
            if (SelektovaniServis == null) return;

            SelektovaniServis.Opis = "Izmenjen opis servisa";
            _context.Servisi.Update(SelektovaniServis);
            _context.SaveChanges();

            OnPropertyChanged(nameof(Servisi));
        }
    }
}
