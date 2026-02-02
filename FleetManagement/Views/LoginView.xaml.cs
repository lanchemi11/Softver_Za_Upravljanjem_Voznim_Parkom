using FleetManagement.Data;
using FleetManagement.Models;
using FleetManagement.Services;
using FleetManagement.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FleetManagement.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

            var context = new AppDbContext();
            var authService = new AuthService(context);

            var vm = new LoginViewModel(authService);
            vm.LoginSucceeded += OnLoginSucceeded;

            DataContext = vm;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }

        private void OpenRegister(object sender, RoutedEventArgs e)
        {
            var registerView = new RegisterView();
            registerView.Show();

            this.Close();
        }

        private void OnLoginSucceeded(User user)
        {
            Window nextWindow = null;

            if (user is Administrator)
                MessageBox.Show("Ulogovan Administrator", "Login OK");
            else if (user is Menadzer)
                MessageBox.Show("Ulogovan Menadžer", "Login OK");
            else if (user is Vozac)
                MessageBox.Show("Ulogovan Vozač", "Login OK");

            nextWindow = new MainWindow();

            nextWindow.Show();
            this.Close();
        }
    }
}