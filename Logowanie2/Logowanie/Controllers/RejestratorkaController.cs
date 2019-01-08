using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logowanie.Data;
using Logowanie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Korzh.EasyQuery.Linq;
using Logowanie.Models;
using Microsoft.AspNetCore.Authorization;
using Logowanie.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;

namespace Logowanie.Controllers
{
    public class RejestratorkaController : Controller
    {
        ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public RejestratorkaController(
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

        public async Task<IActionResult> IndexAsync(string searchString)
        {
          
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "@";

            }
         
            var model = await _db.Pacjenci.ToListAsync();

            IEnumerable<Pacjent> m = model.Where(x => x.Email.Contains(searchString));



            return View(m);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pacjent = await _db.Pacjenci.FindAsync(id);
            if(pacjent==null)
            {
                return NotFound();
            }

          
            return View(pacjent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public async Task<IActionResult> Edit( int? id, string returnUrl,Pacjent model)
        {
    
                returnUrl = null;
                ViewData["ReturnUrl"] = returnUrl;
            
            if (ModelState.IsValid)
            {
               _db.Update(model);
                await _db.SaveChangesAsync();


                _db.SaveChanges();
                return RedirectToAction("IndexAsync");

          }



        //    // If we got this far, something failed, redisplay form
         return View();
        }


    }
}