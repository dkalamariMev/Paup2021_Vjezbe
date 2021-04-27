using Paup2021_Vjezbe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paup2021_Vjezbe.Controllers
{
    public class KolegijiController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();

        public ActionResult Popis()
        {
            var kolegiji = bazaPodataka.PopisKolegija.OrderBy(x => x.Semestar).ThenBy(x => x.Naziv).ToList();
            return View(kolegiji);
        }
    }
}