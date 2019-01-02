using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logowanie.Data;
using Logowanie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logowanie.Controllers
{
    public class LekarzController : Controller
    {
        ApplicationDbContext _db;

        public LekarzController(
                    ApplicationDbContext db)
        {
            _db = db;

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