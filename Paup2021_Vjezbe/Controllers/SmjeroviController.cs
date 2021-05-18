using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Paup2021_Vjezbe.Misc;
using Paup2021_Vjezbe.Models;

namespace Paup2021_Vjezbe.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.Administrator + ", " + OvlastiKorisnik.Moderator)]
    public class SmjeroviController : Controller
    {
        private BazaDbContext db = new BazaDbContext();

        // GET: Smjerovi
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.PopisSmjerova.ToList());
        }

        // GET: Smjerovi/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Smjer smjer = db.PopisSmjerova.Find(id);
            if (smjer == null)
            {
                return HttpNotFound();
            }
            return View(smjer);
        }

        // GET: Smjerovi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Smjerovi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sifra,Naziv,Aktivnost")] Smjer smjer)
        {
            if (ModelState.IsValid)
            {
                db.PopisSmjerova.Add(smjer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(smjer);
        }

        // GET: Smjerovi/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Smjer smjer = db.PopisSmjerova.Find(id);
            if (smjer == null)
            {
                return HttpNotFound();
            }
            return View(smjer);
        }

        // POST: Smjerovi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sifra,Naziv,Aktivnost")] Smjer smjer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smjer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(smjer);
        }

        // GET: Smjerovi/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Smjer smjer = db.PopisSmjerova.Find(id);
            if (smjer == null)
            {
                return HttpNotFound();
            }
            return View(smjer);
        }

        // POST: Smjerovi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Smjer smjer = db.PopisSmjerova.Find(id);
            db.PopisSmjerova.Remove(smjer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
