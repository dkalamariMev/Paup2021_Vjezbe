using Paup2021_Vjezbe.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paup2021_Vjezbe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Primjer kako doći do podataka o logiranom korisniku
            if (User != null)
            {                  
                LogiraniKorisnik logKor = User as LogiraniKorisnik;
                if (logKor != null)
                {
                    ViewBag.Logirani = logKor.KorisnickoIme;
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}