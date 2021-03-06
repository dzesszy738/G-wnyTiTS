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

        
        public long Pesel { get; set; }

        
        [MaxLength(50)]
        
        [Display(Name = "Imię")]
        public string Imie { get; set; }

    
        [MaxLength(50)]
        
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; }



        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data urodzenia")]
        public DateTime DataUr { get; set; }
       
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name ="Numer telefonu")]
        public string Telefon { get; set; }

        [Display(Name ="Uczulenia")]
        public string Uczulenia { get; set; }

        public ICollection<Leki> Leki { get; set; }
        public ICollection<Wizyty> Wizyty { get; set; }





    }
}
