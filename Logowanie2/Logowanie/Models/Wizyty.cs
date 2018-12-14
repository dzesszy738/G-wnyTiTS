using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Logowanie.Models
{
    public class Wizyty
    {
        [Key]
        [Required]
        public int IdWizyty { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataWizyty { get; set; }

        [Required]
        [ForeignKey("Pacjent")]
        public int IdPacjent { get; set; }
        public Pacjent Pacjent { get; set; }

        public ICollection<Leki> Leki { get; set; }


    }
}
