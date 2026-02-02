using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Utils
{
    public static class ReportGenerator
    {
        public static List<(string Vozilo, string Datum, string Opis)> ServisiPoVozilu(List<Servis> servisi, List<Vozilo> vozila)
        {
            var rezultat = new List<(string Vozilo, string Datum, string Opis)>();

            var grouped = servisi.GroupBy(s => s.VoziloId);

            foreach (var group in grouped)
            {
                var vozilo = vozila.FirstOrDefault(v => v.Id == group.Key);
                string voziloInfo = vozilo != null
                    ? $"{vozilo.Marka} {vozilo.Model} ({vozilo.Registracija})"
                    : $"Vozilo ID {group.Key}";

                foreach (var servis in group)
                {
                    rezultat.Add((
                        Vozilo: voziloInfo,
                        Datum: servis.Datum.ToShortDateString(),
                        Opis: servis.Opis
                    ));
                }
            }

            return rezultat;
        }


        public static List<(string Vozilo, string BrojServisa)> BrojServisaPoVozilu(List<Servis> servisi, List<Vozilo> vozila)
        {
            var rezultat = new List<(string Vozilo, string BrojServisa)>();

            var grouped = servisi.GroupBy(s => s.VoziloId);

            foreach (var group in grouped)
            {
                var vozilo = vozila.FirstOrDefault(v => v.Id == group.Key);
                string voziloInfo = vozilo != null
                    ? $"{vozilo.Marka} {vozilo.Model} ({vozilo.Registracija})"
                    : $"Vozilo ID {group.Key}";

                rezultat.Add((voziloInfo, group.Count().ToString()));
            }

            return rezultat;
        }
    }
}
