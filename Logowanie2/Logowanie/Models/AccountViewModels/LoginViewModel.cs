using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Logowanie.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Adres email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Hasło { get; set; }

        [Display(Name = "Zapamiętaj")]
        public bool Zapamiętaj { get; set; }

    }
}
