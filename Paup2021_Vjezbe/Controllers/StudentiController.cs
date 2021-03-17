using Paup2021_Vjezbe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paup2021_Vjezbe.Controllers
{
    public class StudentiController : Controller
    {
        // GET: Studenti
        public ActionResult Index()
        {
            ViewBag.Title = "Početna stranica o studentima";
            ViewBag.Fakultet = "Međimursko veleučilište";
            return View();
        }

        public ActionResult Popis()
        {
            //Instanciramo klasu StudentiDB koja sadržava listu studenata
            StudentiDB studenti = new StudentiDB();
            //Objekt studentidb klase StudentiDB prosljeđujemo u View kao njegov model
            return View(studenti);
        }

        //int? definiramo da parametar id može biti nullabilan, tj da nemora biti definirana njegova vrijednost
        public ActionResult Detalji(int? id)
        {
            //provjeravamo ako parametar id nema vrijednost, tj on nije definiran
            if (!id.HasValue)//ili if (id.HasValue == false)
            {
                //preusmjeravamo korisnika na akciju Popis
                return RedirectToAction("Popis");
            }

            //Instanciramo klasu StudentiDB koja sadržava listu studenata
            StudentiDB studenti = new StudentiDB();

            /*
               * sa objekta studentidb pozivamo metodu VratiListu() koja nam vraća listu studenata
               * pomoću Lambda izraza FirstOrDefault(x => x.Id == id) dohvaćamo prvog elementa iz liste
               * kojemu se vrijednost propertya Id podudara sa vrijednošću parametra id
               */
            Student student = studenti.VratiListu().FirstOrDefault(x => x.Id == id);

            //ako u listi nema studenta sa traženim Id-em onda je varijabla student null
            if (student == null)
            {
                //u tom slučaju preusmjeravamo korisnika na akciju Popis
                return RedirectToAction("Popis");
            }

            //Objekt student klase Student prosljeđujemo u View kao njegov model
            return View(student);
        }
    }
}