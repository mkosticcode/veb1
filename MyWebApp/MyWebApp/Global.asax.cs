using MyWebApp.BazaPodataka;
using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace MyWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Baza.UcitajSvePodatke();
            foreach (Korisnik k in Baza.Korisnici.Values)
            {
                if (k.Uloga == KorisnikTip.VLASNIK)
                {
                    foreach (FitnesCentar fc in k.FitnesCentri)
                    {
                        Baza.FitnesCentri.Add(fc);
                    }
                }
            }
            //Korisnik vlasnik = new Korisnik("vlasnik", "sifra", "Pera", "Peric", PolTip.MUSKI, "mail@gmail.com", "01/01/1971", KorisnikTip.VLASNIK);
            /*FitnesCentar f1 = new FitnesCentar("Fitnes centar 1", "Adresa 1", 2005, vlasnik.KorisnickoIme, 1000, 11000, 100, 100, 200);
            FitnesCentar f2 = new FitnesCentar("Fitnes centar 2", "Adresa 2", 2020, vlasnik.KorisnickoIme, 1100, 12000, 150, 150, 250);
            FitnesCentar f3 = new FitnesCentar("Fitnes centar 3", "Adresa 3", 2017, vlasnik.KorisnickoIme, 1200, 13000, 200, 200, 300);
           f3.Deleted = true;
           Korisnik posetilac = new Korisnik("posetilac", "sifra", "Marko", "Markovic", PolTip.MUSKI, "mm@gmail.com", "01/01/1991", KorisnikTip.POSETILAC);
           Korisnik trener = new Korisnik("trener", "sifra", "Jovan", "Jovanovic", PolTip.MUSKI, "jj@gmail.com", "01/01/1981", KorisnikTip.TRENER);
           trener.FitnesCentri.Add(f1);
           f1.TrenerKorisnickoIme.Add(trener.KorisnickoIme);
           Baza.Korisnici.Add(vlasnik.KorisnickoIme, vlasnik);
           Baza.Korisnici.Add(trener.KorisnickoIme, trener);
           Baza.Korisnici.Add(posetilac.KorisnickoIme, posetilac);
           vlasnik.FitnesCentri.Add(f1);
           vlasnik.FitnesCentri.Add(f2);
           vlasnik.FitnesCentri.Add(f3);
           GrupniTrening gt = new GrupniTrening("yoga", TreningTip.YOGA, f1, 30, "01/01/2023", 2);
           trener.GrupniTreninzi.Add(gt);
           posetilac.GrupniTreninzi.Add(gt);
           posetilac.FitnesCentri.Add(f1);
           gt.Posetioci.Add(posetilac.KorisnickoIme);
           int id = 0;
           foreach(Korisnik k in Baza.Korisnici.Values)
           {
               foreach(FitnesCentar fc in k.FitnesCentri)
               {
                   fc.Id = id;
                   id++;
                   Baza.FitnesCentri.Add(fc);
               }
           }
           Baza.IspisiKorisnike();*/
            /**/
            /*
             Baza.FitnesCentri.Add(f1);
             Baza.FitnesCentri.Add(f2);
             Baza.FitnesCentri.Add(f3);
             Baza.IspisiFitnesCentre();
             int i=0;
             foreach(FitnesCentar f in Baza.FitnesCentri)
             {
                 f.Id = i;
                 i++;
             }
             Baza.IspisiFitnesCentre();*/
            /* Korisnik vlasnik = new Korisnik("vlasnik", "sifra", "Pera", "Peric", PolTip.MUSKI, "mail@gmail.com", "01/01/1971", KorisnikTip.VLASNIK);
             Korisnik trener = new Korisnik("trener", "sifra", "Jovan", "Jovanovic", PolTip.MUSKI, "jj@gmail.com", "01/01/1981", KorisnikTip.TRENER);
             Korisnik posetilac = new Korisnik("posetilac", "sifra", "Marko", "Markovic", PolTip.MUSKI, "mm@gmail.com", "01/01/1991", KorisnikTip.POSETILAC);
             Baza.Korisnici = new Dictionary<string, Korisnik>();
             Baza.Korisnici.Add(vlasnik.KorisnickoIme, vlasnik);
             Baza.Korisnici.Add(trener.KorisnickoIme, trener);
             Baza.Korisnici.Add(posetilac.KorisnickoIme, posetilac);
             Baza.IspisiKorisnike();*/
             /*
            Korisnik vlasnik = new Korisnik("vlasnik", "sifra", "Pera", "Peric", PolTip.MUSKI, "mail@gmail.com", "01/01/1971", KorisnikTip.VLASNIK);
            Baza.Korisnici.Add(vlasnik.KorisnickoIme, vlasnik);
            Baza.IspisiKorisnike();*/
        }
        public override void Init()
        {
            this.PostAuthenticateRequest += MyPostAuthenticateRequest;
            base.Init();
        }

        void MyPostAuthenticateRequest(object sender, EventArgs e)
        {
           if (HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/Home/")|| HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/")
                || HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/api/") || HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/logins/")
                || HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/App_Data/") || HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/vlasnik/")
                || HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/trener/"))
            {
                System.Web.HttpContext.Current.SetSessionStateBehavior(
                SessionStateBehavior.Required);
            }
        }
    }
}
