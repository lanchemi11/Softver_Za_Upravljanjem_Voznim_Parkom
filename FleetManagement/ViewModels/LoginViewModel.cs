using FleetManagement.Models;
using FleetManagement.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace FleetManagement.ViewModels
{
    public class LoginViewModel
    {
        private readonly AuthService _authService;

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICommand LoginCommand { get; }

        public event Action<User>? LoginSucceeded;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            var user = _authService.Authenticate(Username, Password);

            if (user == null)
            {
                MessageBox.Show("Pogrešno korisničko ime ili lozinka!");
                return;
            }

            LoginSucceeded?.Invoke(user);
        }
    }
}