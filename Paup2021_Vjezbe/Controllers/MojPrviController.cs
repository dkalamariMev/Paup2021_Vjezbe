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
            ViewBag.Ime = "Sanja";
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

        //Merge 14.03.2021 10:55
        public string PericaHuten()
        {
            return "Perica Huten";
        }

        //Merge 14.03.2021 10:53
        public string CarKarlo()
        {
            return "Karlo Car";
        } 
        public string JovanovicMatija()
        {
            return "Jovanović Matija";
        }
        public string NovakLucija()
        {
            return "Novak Lucija";
        }
        public string BesenicErik()
        {
            return "Besenic Erik";
        }
    }
}