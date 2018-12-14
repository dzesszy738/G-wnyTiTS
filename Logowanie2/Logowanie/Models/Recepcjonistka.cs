using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Logowanie.Models
{
    public class Recepcjonistka
    {
        [Key]
        [Required]
        public int IdRec { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Imię")]
        public string Imie { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]

        public int IdUser { get; set; }
        public ApplicationUser user { get; set; }
    }
}
