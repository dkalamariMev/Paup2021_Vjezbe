using Paup2021_Vjezbe.Misc;
using Paup2021_Vjezbe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Paup2021_Vjezbe.Controllers
{
    public class KorisniciController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();

        // GET: Korisnici
        public ActionResult Index()
        {
            var listaKorisnika = bazaPodataka.PopisKorisnika
                .OrderBy(x => x.SifraOvlasti).ThenBy(x => x.Prezime).ToList();
            return View(listaKorisnika);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Prijava(string returnUrl)
        {
            KorisnikPrijava model = new KorisnikPrijava();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        public ActionResult Prijava(KorisnikPrijava model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //dohvaćamo podatke o korisniku po korisničkom imenu
                var korisnikBaza = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.KorisnickoIme == model.KorisnickoIme);
                //provjeravamo hash lozinke iz baze i izračunati hash na temelju upisane lozinke na login formi
                bool passwordOK = korisnikBaza.Lozinka == Misc.PasswordHelper.IzracunajHash(model.Lozinka);

                if (passwordOK)
                {
                    LogiraniKorisnik prijavljeniKorisnik = new LogiraniKorisnik(korisnikBaza);
                    LogiraniKorisnikSerializeModel serializeModel = new LogiraniKorisnikSerializeModel();
                    serializeModel.CopyFromUser(prijavljeniKorisnik);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string korisnickiPodaci = serializer.Serialize(serializeModel);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                        1,
                        prijavljeniKorisnik.Identity.Name,
                        DateTime.Now,
                        DateTime.Now.AddDays(1),
                        false,
                        korisnickiPodaci);

                    string ticketEncrpyted = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                        ticketEncrpyted);
                    Response.Cookies.Add(cookie);

                    //ako postoji url kojem je korisnik prvotno pristupao tada preusmjeravamo na taj url
                    if(!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka");
            return View(model);
        }
    }
}