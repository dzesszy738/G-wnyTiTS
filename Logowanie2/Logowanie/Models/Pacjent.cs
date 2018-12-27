using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Logowanie.Models
{
    public class Pacjent
    { 
        [Key]
        [Required]
        public int IdPacjent { get; set; }

        
        [MaxLength(11)]
     
        public long Pesel { get; set; }

        
        [MaxLength(50)]
        [Display(Name = "Imię")]
        public string Imie { get; set; }

    
        [MaxLength(50)]
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; }

      
       
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime DataUr { get; set; }
       
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public ICollection<Leki> Leki { get; set; }
        public ICollection<Wizyty> Wizyty { get; set; }





    }
}
