using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Logowanie.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Lekarz> Lekarze { get; set; }
        public ICollection<Recepcjonistka> Recepcjonistki { get; set; }
        public ICollection<Pacjent> Pacjenci { get; set; }
    }
}
