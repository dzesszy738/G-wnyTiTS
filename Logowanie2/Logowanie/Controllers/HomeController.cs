using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Logowanie.Models;
using System.Security.Claims;
using Logowanie.Data;

namespace Logowanie.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db;

        public HomeController(
                    ApplicationDbContext db)
        {
            _db = db;

        }

        public IActionResult Index()
        {


            if(User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                return RedirectToAction("Index","Admin");
            }
            if(User.HasClaim(ClaimTypes.Role,"Recepcjonistka"))
            {
                return RedirectToAction("IndexAsync", "Rejestratorka");
            }
            if (User.HasClaim(ClaimTypes.Role, "Lekarz"))
            {
                return RedirectToAction("Index", "Lekarz");
            }

            return View();
        }

        public async Task<IActionResult> About(Pacjent model)
        {
            
            if (User.HasClaim(ClaimTypes.Role, "Pacjent"))
            {
                int k =_db.Pacjenci.Where(x => x.Email == User.Identity.Name).Select(m => m.IdPacjent).First();
                
                 model = await _db.Pacjenci.FindAsync(k);
            }

            return View(model);
        }

        [HttpGet]
        public  IActionResult Contact(string searchString)
        {
            if (User.HasClaim(ClaimTypes.Role, "Pacjent"))
            {
                if (String.IsNullOrEmpty(searchString))
                {
                    searchString = ".";

                }
                var model = _db.Wizyty.ToList();

                IEnumerable<Wizyty> m = model.Where(x => x.DataWizyty.ToShortDateString().Contains(searchString));
                return View(m);

            }

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
