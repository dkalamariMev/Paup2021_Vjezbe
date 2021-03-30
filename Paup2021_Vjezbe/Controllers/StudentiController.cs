using Paup2021_Vjezbe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        //int? definiramo da parametar id može biti nullabilan
        //Ovo je HTTP GET metoda
        //Primjer pozivanja Student/Azuriraj/2
        public ActionResult Azuriraj(int? id)
        {
            //provjeravamo ako parametar id nema vrijednost, tj on nije definiran
            if (!id.HasValue)
            {
                //Vraćamo HTTP status 400
                //Lista HTTP statusnih kodova: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //ILI za preusmjeravanje korisnika na akciju Popis: return RedirectToAction("Popis");
            }

            //Instanciramo klasu StudentiDB koja sadržava listu studenata
            StudentiDB studentidb = new StudentiDB();

            /*
             * pomoću Lambda izraza FirstOrDefault(x => x.Id == id) dohvaćamo prvog elementa iz liste
             * kojemu se vrijednost propertya Id podudara sa vrijednošću parametra id
             */
            Student student = studentidb.VratiListu().FirstOrDefault(x => x.Id == id);

            //ako u listi nema studenta sa traženim Id-em onda je varijabla student null
            if (student == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                //Ili skraćeno
                return HttpNotFound();
            }

            //Objekt student klase Student prosljeđujemo u View kao njegov model
            return View(student);
        }

        //Ovo je HTTP POST metoda
        [HttpPost]
        [ValidateAntiForgeryToken] //mehanizam koji nas štiti od cross site request
                                   // forgery  (poziva post metode izvan naše aplikacije)
        public ActionResult Azuriraj(Student s)
        {
            if (!OIB.CheckOIB(s.Oib))
            {
                ModelState.AddModelError("Oib", "Neispravan OIB");
            }

            DateTime datumPrije18g = DateTime.Now.AddYears(-18);
            if(s.DatumRodjenja > datumPrije18g)
            {
                ModelState.AddModelError("DatumRodjenja", "Osoba mora biti starija od 18");
            }

            //ModelState.IsValid - provjera ispravnosti podataka
            //npr. ako je atribut int tipa a mi smo unijeli string u to polje na formi
            //neće proći validaciju i preusmjerit će korisnika na
            //stranicu za ažuriranje i ispisati grešku validacije
            //više o tome na narednim vježbama
            if (ModelState.IsValid)
            {
                //Ažuriranje liste podataka
                StudentiDB studentidb = new StudentiDB();
                studentidb.AzurirajStudenta(s);
                //Preusmjeravanje na metodu koja vraća popis studenata
                return RedirectToAction("Popis");
            }
            //Ako model nije ispravan vraćamo ga klijentu
            return View(s);
        }
    }
}