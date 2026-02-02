using FleetManagement.ViewModels;
using FleetManagement.Views;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FleetManagement.Data;

namespace FleetManagement
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context;
        public MainWindow()
        {
            InitializeComponent();
            var factory = new AppDbContextFactory();
            _context = factory.CreateDbContext(Array.Empty<string>());
        }

        private void OtvoriVozila_Click(object sender, RoutedEventArgs e)
        {
            VozilaView vozilaView = new VozilaView();
            vozilaView.Show();
        }

        private void OtvoriServise_Click(object sender, RoutedEventArgs e)
        {
            ServisiView servisiView = new ServisiView();
            servisiView.Show();
        }

        private void OtvoriIzvestaje_Click(object sender, RoutedEventArgs e)
        {
            var view = new IzvestajiView();
            view.DataContext = new IzvestajiViewModel(_context);
            view.ShowDialog();
        }
    }
}