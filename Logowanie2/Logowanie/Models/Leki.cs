using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Logowanie.Models
{
    public class Leki
    {
            
        [Key]
        [Required]
        public int IdLek { get; set; }
        [Required]
        public bool Staly { get; set; }
        [Required]
        public bool Czasowy { get; set; }

        public string Nazwa { get; set; }

        [DataType(DataType.Date)]
        public string Od { get; set; }

        [DataType(DataType.Date)]
        public string Do { get; set; }


        [Required]
        [ForeignKey("Pacjent")]
        public int IdPacjent { get; set; }
        public Pacjent Pacjent { get; set; }

        [Required]
        [ForeignKey("Wizyty")]
        public int IdWizyty { get; set; }
        public Wizyty Wizyta { get; set; }
        public CzyCzas stcz { get; set; }
    }

    public enum CzyCzas
    {
        Staly,
        Czasowy
    }


}

