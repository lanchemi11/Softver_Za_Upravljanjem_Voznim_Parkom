using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models
{
    public class Vozilo
    {
        public int Id { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int GodinaProizvodnje { get; set; }
        public string Registracija { get; set; }
        public string Status { get; set; }


        // Asocijacija: Vozilo → Vozac (0..1)
        public int? VozacId { get; set; }
        public Vozac Vozac { get; set; }

        // Kompozicija: Vozilo → Servis (1 → *)
        public ICollection<Servis> Servisi { get; set; }
    }
}
