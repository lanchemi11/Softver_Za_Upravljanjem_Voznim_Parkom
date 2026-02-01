using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Services
{
    public class ReportService : IReportGenerator
    {
        public void PrikaziInfo()
        {
            // Ovde ide logika za prikaz informacija o izveštaju
            Console.WriteLine("Prikaz izveštaja o voznom parku...");
        }

        public void GenerateReport()
        {
            // Metod iz dijagrama koji generiše izveštaj
            Console.WriteLine("Generišem izveštaj...");
        }
    }
}

