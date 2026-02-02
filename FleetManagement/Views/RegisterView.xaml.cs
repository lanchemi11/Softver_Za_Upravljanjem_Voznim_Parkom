using FleetManagement.Data;
using FleetManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
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
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=fleet.db")
                .Options;
            var context = new AppDbContext(options);

            var vm = new RegisterViewModel(context);
            vm.RegistrationSucceeded += OnRegistrationSucceeded; 
            DataContext = vm;
        }

        private void OnRegistrationSucceeded()
        {
            var loginWindow = new LoginView();
            loginWindow.Show();
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}

