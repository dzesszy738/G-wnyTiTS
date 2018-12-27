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
            var pacjenci = from p in _db.Pacjenci
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewData["Message"] = searchString;
            }
            else
            {
                ViewData["Message"] = "@";
            }
           var model= await _db.Pacjenci.ToListAsync();
           


            return View(model);

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

        public async Task<IActionResult> Edit( int? id, string returnUrl, Pacjent model)
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