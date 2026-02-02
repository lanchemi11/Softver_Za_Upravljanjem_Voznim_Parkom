using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;

namespace FleetManagement.Utils
{
    public static class PdfHelper
    {
        public static void GenerateServisiPoVozilu(List<(string Vozilo, string Datum, string Opis)> podaci, string filePath)
        {
            using (PdfDocument document = new PdfDocument())
            {
                document.Info.Title = "Izveštaj - Servisi po vozilu";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont headerFont = new XFont("Arial", 14);
                XFont cellFont = new XFont("Arial", 12);

                double y = 40;

                // Naslov
                gfx.DrawString("Izveštaj: Servisi po vozilu", headerFont, XBrushes.Black,
                    new XRect(20, y, page.Width - 40, 30), XStringFormats.TopLeft);
                y += 40;

                // Header tabela
                gfx.DrawRectangle(XPens.Black, 20, y, page.Width - 40, 25);
                gfx.DrawString("Vozilo", cellFont, XBrushes.Black, new XRect(25, y + 5, 150, 20), XStringFormats.TopLeft);
                gfx.DrawString("Datum", cellFont, XBrushes.Black, new XRect(200, y + 5, 100, 20), XStringFormats.TopLeft);
                gfx.DrawString("Opis", cellFont, XBrushes.Black, new XRect(320, y + 5, page.Width - 360, 20), XStringFormats.TopLeft);
                y += 25;

                // Redovi
                foreach (var item in podaci)
                {
                    gfx.DrawRectangle(XPens.Black, 20, y, page.Width - 40, 25);
                    gfx.DrawString(item.Vozilo, cellFont, XBrushes.Black, new XRect(25, y + 5, 150, 20), XStringFormats.TopLeft);
                    gfx.DrawString(item.Datum, cellFont, XBrushes.Black, new XRect(200, y + 5, 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString(item.Opis, cellFont, XBrushes.Black, new XRect(320, y + 5, page.Width - 360, 20), XStringFormats.TopLeft);
                    y += 25;
                }

                document.Save(filePath);
            }
        }

        public static void GenerateBrojServisaPoVozilu(List<(string Vozilo, string BrojServisa)> podaci, string filePath)
        {
            using (PdfDocument document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                var headerFont = new XFont("Arial", 14);
                var cellFont = new XFont("Arial", 12);

                double y = 40;

                // Naslov
                gfx.DrawString("Izveštaj: Broj servisa po vozilu", headerFont, XBrushes.Black,
                    new XRect(20, y, page.Width - 40, 30), XStringFormats.TopLeft);
                y += 40;

                // Header
                gfx.DrawString("Vozilo", cellFont, XBrushes.Black, new XRect(25, y, 250, 20), XStringFormats.TopLeft);
                gfx.DrawString("Broj servisa", cellFont, XBrushes.Black, new XRect(300, y, 100, 20), XStringFormats.TopLeft);
                y += 25;

                // Redovi
                foreach (var item in podaci)
                {
                    gfx.DrawString(item.Vozilo, cellFont, XBrushes.Black, new XRect(25, y, 250, 20), XStringFormats.TopLeft);
                    gfx.DrawString(item.BrojServisa, cellFont, XBrushes.Black, new XRect(300, y, 100, 20), XStringFormats.TopLeft);
                    y += 25;
                }

                document.Save(filePath);
            }
        }
    }
}
