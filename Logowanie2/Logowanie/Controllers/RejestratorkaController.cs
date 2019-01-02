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

namespace Logowanie.Controllers
{
    public class RejestratorkaController : Controller
    {
        ApplicationDbContext _db;

        public RejestratorkaController(
                    ApplicationDbContext db)
        {
            _db = db;
            
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