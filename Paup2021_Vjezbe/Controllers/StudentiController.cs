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
            StudentiDB studenti = new StudentiDB();

            return View(studenti);
        }

        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            StudentiDB studenti = new StudentiDB();
            Student student = studenti.VratiListu().FirstOrDefault(x => x.Id == id);

            if(student == null)
            {
                return RedirectToAction("Popis");
            }

            return View(student);
        }
    }
}