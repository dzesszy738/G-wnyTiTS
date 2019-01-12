using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Logowanie.Models.AccountViewModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Obecne hasło")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Nowe hasło")]
        [StringLength(100, ErrorMessage = "{0} musi się składać z co najmniej {2} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Nowe hasło", ErrorMessage = "Hasła się nie zgadzają")]
        public string ConfirmPassword { get; set; }
        
    }
}
