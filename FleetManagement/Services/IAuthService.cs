using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Services
{
    public interface IAuthService
    {
        User Login(string username, string password);
    }
}
