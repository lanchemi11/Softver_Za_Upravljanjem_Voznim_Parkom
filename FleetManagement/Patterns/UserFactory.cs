using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Patterns
{
    public interface IUserFactory
    {
        User CreateUser(string username, string password, string role, string? brojLicence = null);
    }

    public class UserFactory : IUserFactory
    {
        public User CreateUser(string username, string password, string role, string? brojLicence = null)
        {
            return role switch
            {
                "Administrator" => new Administrator
                {
                    Username = username,
                    Lozinka = password,
                    Rola = role
                },
                "Menadzer" => new Menadzer
                {
                    Username = username,
                    Lozinka = password,
                    Rola = role
                },
                "Vozac" => new Vozac
                {
                    Username = username,
                    Lozinka = password,
                    Rola = role,
                    BrojLicence = brojLicence ?? string.Empty
                },
                _ => throw new ArgumentException("Nepoznata rola!")
            };
        }
    }
}
