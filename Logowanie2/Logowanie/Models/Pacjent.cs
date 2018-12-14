﻿using System;
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

        [Required]
        [MaxLength(11)]
     
        public long Pesel { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Imię")]
        public string Imie { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataUr { get; set; }

        public ICollection<Leki> Leki { get; set; }
        public ICollection<Wizyty> Wizyty { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]

        public int IdUser { get; set; }
        public ApplicationUser user { get; set; }



    }
}
