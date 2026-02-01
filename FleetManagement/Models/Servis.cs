using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models
{
    public class Servis
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Opis { get; set; }
        public decimal Cena { get; set; }

        // FK ka Vozilo (kompozicija)
        public int VoziloId { get; set; }
        public Vozilo Vozilo { get; set; }
    }
}
