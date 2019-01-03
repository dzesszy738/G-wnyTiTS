using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logowanie.Data;
using Logowanie.Models;
using Logowanie.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Logowanie.Controllers
{
    public class LekarzController : Controller
    {
        ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public LekarzController(
            UserManager<ApplicationUser> userManager,
                    ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;

        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {


            var mail = _userManager.GetUserName(User);
            var user = _userManager.FindByNameAsync(mail).Result;
            if (user == null || !(_userManager.
                      IsEmailConfirmedAsync(user).Result))
            {
                ViewBag.Message = "Error while resetting your password!";
                return View("ErrorReset");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);


            return View("Reset");

        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {

                var mode = await _db.Pacjenci.ToListAsync();
                return View(mode);


            }

            var model = await _db.Pacjenci.ToListAsync();

            IEnumerable<Pacjent> m = model.Where(x => x.Pesel.ToString().Contains(searchString));



            return View(m);
        }
      
        
        [HttpGet]
        public  IActionResult Wizyty(string searchString, int? id)
        {
            
            if (String.IsNullOrEmpty(searchString))
            {
                var mo = _db.Wizyty.Where(x => x.IdPacjent == id);

                return View(mo);
            }
            var model = _db.Wizyty.Where(x => x.IdPacjent == id);

            IEnumerable<Wizyty> m = model.Where(x => x.DataWizyty.ToShortDateString().Contains(searchString));
           
             
            return View(m);



        }
        [HttpGet]
        public IActionResult WizytyDodaj(int id)
        {
            Wizyty w = new Wizyty() { IdPacjent = id, DataWizyty = DateTime.Now };
            
            return View(w);




        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult WizytyDodaj(Wizyty wizyta)
        {
            _db.Wizyty.Add(wizyta);
            _db.SaveChanges();

            return View();

        

        }
        public IActionResult LekDodaj()
        {
            return View();
        }
    }
}