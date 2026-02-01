using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Lozinka { get; set; }
        public string Rola { get; set; }
    }
}
