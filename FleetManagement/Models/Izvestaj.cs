using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models
{
    public class Izvestaj
    {
        public int Id { get; set; }
        public string Naziv { get; set; }

        public DateTime DatumKreiranja { get; set; }

        // Agregacija
        public ICollection<Vozilo> Vozila { get; set; }
    }
}
