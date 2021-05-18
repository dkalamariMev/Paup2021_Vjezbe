using Paup2021_Vjezbe.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Paup2021_Vjezbe
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Kod svakog korisničkog zahtjeva čitamo podatke o logiranom korisniku
        /// iz Cookiea, na način da dekriptiramo te podatke, deserijaliziramo ih,
        /// spremimo u objekt klase LogiraniKorisnik koji onda spremimo u
        /// HttpContext.Current.User koji nam je potreban za provjeru da li je korisnik autentificiran
        /// i provjeru da li korisnik ima prava pristupa po razini ovlasti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if(authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                LogiraniKorisnikSerializeModel serializeModel =
                    serializer.Deserialize<LogiraniKorisnikSerializeModel>(authTicket.UserData);

                LogiraniKorisnik korisnik = new LogiraniKorisnik(authTicket.Name);
                korisnik.PrezimeIme = serializeModel.PrezimeIme;
                korisnik.Ovlast = serializeModel.Ovlast;

                HttpContext.Current.User = korisnik;
            }
        }
    }
}
