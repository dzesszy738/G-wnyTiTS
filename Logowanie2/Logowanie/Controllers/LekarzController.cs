﻿using System;
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
        public IActionResult Edit(int? id)
        {

            Pacjent P = new Pacjent();

            if (id == null)
            {
                return RedirectToAction("About","Home");
            }
            var pacjent = _db.Pacjenci.Find(id);
            if (pacjent == null)
            {
                return NotFound();
            }
            return View(pacjent);
        }
    }
    }
