using FleetManagement.Data;
using FleetManagement.Models;
using FleetManagement.Patterns;
using FleetManagement.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
namespace FleetManagement.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private readonly IUserFactory _userFactory = new UserFactory();

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string BrojLicence { get; set; } = string.Empty;

        public List<string> Roles { get; } = new() { "Administrator", "Menadzer", "Vozac" };

        private string _selectedRole = string.Empty;
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsVozacSelected));
            }
        }

        public bool IsVozacSelected => SelectedRole == "Vozac";

        public ICommand RegisterCommand { get; }

        private readonly UserRegistrationNotifier _notifier = new();

        public RegisterViewModel(AppDbContext context)
        {
            _context = context;
            RegisterCommand = new RelayCommand(Register);
            _notifier.Attach(new LogObserver());
            _notifier.Attach(new UiObserver());

        }

        public event Action? RegistrationSucceeded;

        private void Register(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SelectedRole))
            {
                MessageBox.Show("Morate odabrati rolu!");
                return;
            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Popunite sva polja!");
                return;
            }

            if (IsVozacSelected && string.IsNullOrWhiteSpace(BrojLicence))
            {
                MessageBox.Show("Unesite broj licence za vozača!");
                return;
            }

            bool exists = _context.Set<User>().Any(u => u.Username.ToLower() == Username.ToLower());
            if (exists)
            {
                MessageBox.Show($"Korisničko ime '{Username}' već postoji!");
                return;
            }

            User user;
            try
            {
                user = _userFactory.CreateUser(Username, Password, SelectedRole, BrojLicence);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Morate odabrati validnu rolu!");
                return;
            }

            try
            {
                _context.Add(user);
                _context.SaveChanges();

                _notifier.Notify($"Korisnik '{user.Username}' uspešno registrovan!");
                RegistrationSucceeded?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom registracije: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}