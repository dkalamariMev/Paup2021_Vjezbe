using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paup2021_Vjezbe.Controllers
{
    public class MojPrviController : Controller
    {
        // GET: MojPrvi
        public ActionResult Index()
        {
            ViewBag.Lokacija = "Međimursko veleučilište u Čakovcu";
            return View();
        }

        public ActionResult Druga()
        {
            ViewBag.Ustanova = "Međimursko veleučilište";
            ViewData["Lokacija"] = "Čakovec";
            return View();
        }

        public ActionResult Treca(string poruka, int broj=1)
        {
            ViewBag.Poruka = poruka;
            ViewBag.Broj = broj;
            return View();
        }

        public ActionResult Student()
        {
            ViewBag.Ime = "Miha";
            ViewBag.Prezime = "Perić";
            ViewBag.GodinaRodjenja = 1995;

            return View();
        }

        public string VratiVrijeme()
        {
            return DateTime.Now.ToString();
        }

        public string VratiVrijemeUtorak()
        {
            return DateTime.Now.ToString();
        }
        public string GalinaErik()
        {
            return "Galina Erik";
        }
    }
}