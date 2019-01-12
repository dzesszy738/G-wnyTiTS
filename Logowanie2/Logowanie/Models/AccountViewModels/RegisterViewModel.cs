using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Logowanie.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Adres email jest wymagany")]
        [EmailAddress(ErrorMessage ="Niepoprawny adres email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "{0} musi się składać z co najmniej {2} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Hasło { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Hasło", ErrorMessage = "Hasła się nie zgadzają")]
        public string ConfirmPassword { get; set; }

        public Rola Role { get; set; }
    }

    public enum Rola
    {
        Recepcjonistka,
        Lekarz
   }
    
}
