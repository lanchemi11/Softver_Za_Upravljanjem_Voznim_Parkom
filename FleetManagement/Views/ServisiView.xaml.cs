using FleetManagement.Data;
using FleetManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FleetManagement.Views
{
    /// <summary>
    /// Interaction logic for ServisiView.xaml
    /// </summary>
    public partial class ServisiView : Window
    {
        public ServisiView()
        {
            InitializeComponent();

            var context = new AppDbContextFactory().CreateDbContext(null);
            DataContext = new ServisViewModel(context);
        }
    }
}
