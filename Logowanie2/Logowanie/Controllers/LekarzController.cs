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
        public IActionResult Wizyty(string searchString, int? id)
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
        public IActionResult WizytyDodaj(int id, int? idw)
        {
            if (idw.HasValue)
            {
                Wizyty wi = _db.Wizyty.Where(x => x.IdPacjent == id && x.IdWizyty==idw.Value).Last();

                return View(wi);
            }
            else
            {
                Wizyty w = new Wizyty() { IdPacjent = id, DataWizyty = DateTime.Now };
                _db.Wizyty.Add(w);
                _db.SaveChanges();

                Wizyty wi = _db.Wizyty.Where(x => x.IdPacjent == w.IdPacjent && x.DataWizyty == w.DataWizyty).Last();

                return View(wi);
            }



        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult WizytyDodaj(Wizyty wizyta)
        {
            if (wizyta.Opis == null && wizyta.Zalecenia == null)
            {
                _db.Wizyty.Remove(wizyta);
                _db.SaveChanges();
            }
            else
            {
                _db.Wizyty.Update(wizyta);
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "");



        }
        [HttpGet]
        [ValidateAntiForgeryToken]
       
      public IActionResult DodajLek(int? idp,int? idw)
        { 
           
            Leki w = new Leki() { IdPacjent = idp.Value, IdWizyty = idw.Value };

            return View(w);



        }

        [HttpPost]
        public IActionResult DodajLek(Leki lek)
        {

            if (lek.stcz.ToString() == "Staly")
            {
                lek.Staly = true;
                lek.Czasowy = false;

                _db.Leki.Add(lek);
                _db.SaveChanges();

                return RedirectToAction("WizytyDodaj", (lek.IdPacjent, lek.IdWizyty));

            }
            else
            {
                lek.Czasowy = true;
                lek.Staly = false;
                _db.Leki.Add(lek);
                _db.SaveChanges();
                return RedirectToAction("WizytyDodaj", (lek.IdPacjent, lek.IdWizyty));

            }

        }


        [HttpGet]
        public IActionResult Lek( int? id,int? idw)
        {

                var mo = _db.Leki.Where(x => x.IdPacjent == id && x.IdWizyty==idw);

                return View(mo);
          



        }
    }
}