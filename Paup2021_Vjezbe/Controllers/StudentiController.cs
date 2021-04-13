﻿using Paup2021_Vjezbe.Models;
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
        BazaDbContext bazaPodataka = new BazaDbContext();

        // GET: Studenti
        public ActionResult Index()
        {
            ViewBag.Title = "Početna stranica o studentima";
            ViewBag.Fakultet = "Međimursko veleučilište";
            return View();
        }

        public ActionResult Popis(string naziv, string spol)
        {
            var studenti = bazaPodataka.PopisStudenata.ToList();

            if (!String.IsNullOrWhiteSpace(naziv))
            {
                studenti = studenti.Where(x => x.PrezimeIme.ToUpper().Contains(naziv.ToUpper())).ToList();
            }
            if (!String.IsNullOrWhiteSpace(spol))
            {
                studenti = studenti.Where(x => x.Spol == spol).ToList();
            }

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

            Student student = bazaPodataka.PopisStudenata.FirstOrDefault(x => x.Id == id);

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
            Student student;
            //provjeravamo ako parametar id nema vrijednost, tj on nije definiran
            if (!id.HasValue)
            {
                student = new Student();
                ViewBag.Title = "Kreiranje studenta";
                ViewBag.Novi = true;
            }
            else
            {
                student = bazaPodataka.PopisStudenata.FirstOrDefault(x => x.Id == id);

                //ako u listi nema studenta sa traženim Id-em onda je varijabla student null
                if (student == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    //Ili skraćeno
                    return HttpNotFound();
                }

                ViewBag.Title = "Ažuriranje podataka o studentu";
                ViewBag.Novi = false;

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
                if (s.Id != 0)
                {
                    bazaPodataka.Entry(s).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    bazaPodataka.PopisStudenata.Add(s);
                }
                bazaPodataka.SaveChanges();
                //Preusmjeravanje na metodu koja vraća popis studenata
                return RedirectToAction("Popis");
            }

            if(s.Id == 0)
            {
                ViewBag.Title = "Kreiranje studenta";
                ViewBag.Novi = true;
            }
            else
            {
                ViewBag.Title = "Ažuriranje podataka o studentu";
                ViewBag.Novi = false;
            }
            //Ako model nije ispravan vraćamo ga klijentu
            return View(s);
        }

        public ActionResult Brisi(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Popis");
            }

            Student s = bazaPodataka.PopisStudenata.FirstOrDefault(x => x.Id == id);

            if(s == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Potvrda brisanja studenta";
            return View(s);
        }

        [HttpPost]
        public ActionResult Brisi(int id)
        {
            Student s = bazaPodataka.PopisStudenata.FirstOrDefault(x => x.Id == id);
            if (s == null)
            {
                return HttpNotFound();
            }

            bazaPodataka.PopisStudenata.Remove(s);
            bazaPodataka.SaveChanges();

            return View("BrisiStatus");
        }
    }
}