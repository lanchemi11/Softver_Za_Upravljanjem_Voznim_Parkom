using FleetManagement.Data;
using FleetManagement.Models;
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

        public RegisterViewModel(AppDbContext context)
        {
            _context = context;
            RegisterCommand = new RelayCommand(Register);
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

            User? user = SelectedRole switch
            {
                "Administrator" => new Administrator(),
                "Menadzer" => new Menadzer(),
                "Vozac" => new Vozac { BrojLicence = BrojLicence },
                _ => null
            };

            if (user == null)
            {
                MessageBox.Show("Morate odabrati rolu!");
                return;
            }

            user.Username = Username;
            user.Lozinka = Password;
            user.Rola = SelectedRole;

            try
            {
                _context.Add(user);
                _context.SaveChanges();

                MessageBox.Show("Uspešno ste registrovali korisnika!");

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