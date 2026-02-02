using FleetManagement.Data;
using FleetManagement.Utils;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Fonts;
using System.Configuration;
using System.Data;
using System.Windows;
using PdfSharp.Fonts;
using FleetManagement.Utils;

namespace FleetManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppDbContext DbContext { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // For fonts in pdf
            GlobalFontSettings.FontResolver = new CustomFontResolver();
            base.OnStartup(e);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=fleet.db")
                .Options;

            DbContext = new AppDbContext(options);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
