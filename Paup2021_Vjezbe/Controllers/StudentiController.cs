using PagedList;
using Paup2021_Vjezbe.Models;
using Paup2021_Vjezbe.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Paup2021_Vjezbe.Controllers
{
    [Authorize]
    public class StudentiController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();

        // GET: Studenti
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Početna stranica o studentima";
            ViewBag.Fakultet = "Međimursko veleučilište";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Popis()
        {
            var smjeroviList = bazaPodataka.PopisSmjerova.OrderBy(x => x.Naziv).ToList();
            ViewBag.Smjerovi = smjeroviList;
            return View();
        }

        [AllowAnonymous]
        public ActionResult PopisPartial(string naziv, string spol, string smjer, string sort, int? page)
        {
            //System.Threading.Thread.Sleep(200); //simulacija duže obrade zahtjeva

            ViewBag.Sortiranje = sort;
            ViewBag.NazivSort = String.IsNullOrEmpty(sort) ? "naziv_desc" : "";
            ViewBag.SmjerSort = sort == "smjer" ? "smjer_desc" : "smjer";
            ViewBag.Smjer = smjer;
            ViewBag.Naziv = naziv;
            ViewBag.Spol = spol;

            var studenti = bazaPodataka.PopisStudenata.ToList();

            //filtriranje
            if (!String.IsNullOrWhiteSpace(naziv))
            {
                studenti = studenti.Where(x => x.PrezimeIme.ToUpper().Contains(naziv.ToUpper())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(spol))
            {
                studenti = studenti.Where(x => x.Spol == spol).ToList();
            }

            if (!String.IsNullOrWhiteSpace(smjer))
            {
                studenti = studenti.Where(x => x.SifraSmjera == smjer).ToList();
            }

            switch (sort)
            {
                case "naziv_desc":
                    studenti = studenti.OrderByDescending(s => s.PrezimeIme).ToList();
                    break;
                case "smjer":
                    studenti = studenti.OrderBy(s => s.SifraSmjera).ToList();
                    break;
                case "smjer_desc":
                    studenti = studenti.OrderByDescending(s => s.SifraSmjera).ToList();
                    break;
                default:
                    studenti = studenti.OrderBy(s => s.PrezimeIme).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return PartialView("_PartialPopis", studenti.ToPagedList(pageNumber, pageSize));
        }

        [AllowAnonymous]
        public ActionResult IspisStudenata(string naziv, string spol, string smjer, string sort, int? page)
        {
            //System.Threading.Thread.Sleep(200); //simulacija duže obrade zahtjeva

            ViewBag.Sortiranje = sort;
            ViewBag.NazivSort = String.IsNullOrEmpty(sort) ? "naziv_desc" : "";
            ViewBag.SmjerSort = sort == "smjer" ? "smjer_desc" : "smjer";
            ViewBag.Smjer = smjer;
            ViewBag.Naziv = naziv;
            ViewBag.Spol = spol;

            var studenti = bazaPodataka.PopisStudenata.ToList();

            //filtriranje
            if (!String.IsNullOrWhiteSpace(naziv))
            {
                studenti = studenti.Where(x => x.PrezimeIme.ToUpper().Contains(naziv.ToUpper())).ToList();
            }

            if (!String.IsNullOrWhiteSpace(spol))
            {
                studenti = studenti.Where(x => x.Spol == spol).ToList();
            }

            if (!String.IsNullOrWhiteSpace(smjer))
            {
                studenti = studenti.Where(x => x.SifraSmjera == smjer).ToList();
            }

            switch (sort)
            {
                case "naziv_desc":
                    studenti = studenti.OrderByDescending(s => s.PrezimeIme).ToList();
                    break;
                case "smjer":
                    studenti = studenti.OrderBy(s => s.SifraSmjera).ToList();
                    break;
                case "smjer_desc":
                    studenti = studenti.OrderByDescending(s => s.SifraSmjera).ToList();
                    break;
                default:
                    studenti = studenti.OrderBy(s => s.PrezimeIme).ToList();
                    break;
            }

            StudentiReport studentiReport = new StudentiReport();
            studentiReport.ListaStudenata(studenti);

            return File(studentiReport.Podaci, System.Net.Mime.MediaTypeNames.Application.Pdf,
                "PopisStudenata.pdf");
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
            //onda kreiramo novi objekt klase Student
            if (!id.HasValue)
            {
                student = new Student();
                ViewBag.Title = "Kreiranje studenta";
                ViewBag.Novi = true;
            }
            //ako id postoji onda provjeravamo ako taj student postoji u bazi podataka
            else
            {
                student = bazaPodataka.PopisStudenata.FirstOrDefault(x => x.Id == id);

                //ako u listi nema studenta sa traženim Id-em onda je objekt student null
                if (student == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    //Ili skraćeno
                    return HttpNotFound();
                }

                ViewBag.Title = "Ažuriranje podataka o studentu";
                ViewBag.Novi = false;

            }

            var smjerovi = bazaPodataka.PopisSmjerova.OrderBy(x => x.Naziv).ToList();
            smjerovi.Insert(0, new Smjer { Sifra = "", Naziv = "Nedefinirano" });
            ViewBag.Smjerovi = smjerovi;

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
            if (s.DatumRodjenja > datumPrije18g)
            {
                ModelState.AddModelError("DatumRodjenja", "Osoba mora biti starija od 18");
            }

            //Ako je korisnik odabrao sliku spremamo je u direktorij Images
            if (s.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                string extension = Path.GetExtension(s.ImageFile.FileName);

                //kontroliramo ekstenziju datoteke
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    fileName = fileName + "_" + DateTime.Now.Ticks + extension; //umjesto DateTime.Now.Ticks možemo koristiti i Guid.NewGuid()
                    string folderSlike = "~/Images/";
                    //u model spremamo putanju do slike
                    s.SlikaPutanja = folderSlike + fileName;
                    //generiramo putanju do slike na disku gdje je želimo spremiti
                    fileName = Path.Combine(Server.MapPath(folderSlike), fileName);
                    s.ImageFile.SaveAs(fileName);
                }
                else
                {
                    ModelState.AddModelError("SlikaPutanja", "Nepodržana ekstenzija");
                }
            }

            //ModelState.IsValid - provjera ispravnosti podataka
            //npr. ako je atribut int tipa a mi smo unijeli string u to polje na formi
            //neće proći validaciju i preusmjerit će korisnika na
            //stranicu za ažuriranje i ispisati grešku validacije
            //više o tome na narednim vježbama
            if (ModelState.IsValid)
            {
                //ako model ima vrijednost parametra Id različito od 0 tada znamo da korisnik ažurira podatke o studentu
                if (s.Id != 0)
                {
                    bazaPodataka.Entry(s).State = System.Data.Entity.EntityState.Modified;
                }
                //ako model ima vrijednost parametra Id jednak 0 tada se radi o dodavanju novog studenta
                else
                {
                    bazaPodataka.PopisStudenata.Add(s);
                }
                bazaPodataka.SaveChanges();

                //Preusmjeravanje na metodu koja vraća popis studenata
                return RedirectToAction("Popis");
            }

            //ukoliko je došlo do greške validacije potrebno je ponovno prikazati formu za unos s unešenim podacima
            //i ovisno dal se kreira (Id == 0) ili ažurira student modificiramo naslov stranice
            if (s.Id == 0)
            {
                ViewBag.Title = "Kreiranje studenta";
                ViewBag.Novi = true;
            }
            else
            {
                ViewBag.Title = "Ažuriranje podataka o studentu";
                ViewBag.Novi = false;
            }

            var smjerovi = bazaPodataka.PopisSmjerova.OrderBy(x => x.Naziv).ToList();
            smjerovi.Insert(0, new Smjer { Sifra = "", Naziv = "Nedefinirano" });
            ViewBag.Smjerovi = smjerovi;

            return View(s);
        }

        //Potvrda akcije brisanja studenta
        //GET metoda
        public ActionResult Brisi(int? id)
        {
            //ako id nije defiran preusmjeravamo korisnika na popis studenata
            if (id == null)
            {
                return RedirectToAction("Popis");
            }

            //dohvaćamo studenta iz baze podataka na temelju id
            Student s = bazaPodataka.PopisStudenata.FirstOrDefault(x => x.Id == id);

            //ako ne postoji student s tim id-em vraćamo HTTP status Not found
            if (s == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Potvrda brisanja studenta";
            return View(s);
        }

        //Metoda za brisanje studenta
        //Poziva je metoda za potvrdu brisanja
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int id)
        {
            Student s = bazaPodataka.PopisStudenata.FirstOrDefault(x => x.Id == id);
            if (s == null)
            {
                return HttpNotFound();
            }

            bazaPodataka.PopisStudenata.Remove(s);
            bazaPodataka.SaveChanges();

            //Ovdje smo definirali kao parametar naziv viewa koji metoda vraća
            //Pošto GET metoda Brisi vraća view Brisi ovdje putem parametra definiramo koji view vraća ova metoda
            //U tom viewu će biti prikazan status brisanja
            return View("BrisiStatus");
        }
    }
}