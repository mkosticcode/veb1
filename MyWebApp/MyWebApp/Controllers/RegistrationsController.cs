using MyWebApp.BazaPodataka;
using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MyWebApp.Controllers
{
    [RoutePrefix("register")]
    public class RegistrationsController : ApiController
    {
        public IHttpActionResult Post(Korisnik korisnik)
        {
            if (korisnik == null)
            {
                return BadRequest();
            }
            if (korisnik.Ime == null || korisnik.Lozinka == null
                || korisnik.Prezime == null || korisnik.KorisnickoIme == null
                || korisnik.Email == null || korisnik.Uloga == KorisnikTip.VLASNIK)
            {
                return BadRequest();
            }
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (Baza.Korisnici.ContainsKey(korisnik.KorisnickoIme))
            {
                return BadRequest();
            }
            if (korisnik.Uloga == KorisnikTip.TRENER)
            {
                if (HttpContext.Current.Session["fc"] == null)
                {
                    return BadRequest();
                }
                if (user == null)
                {
                    return BadRequest();

                }
                else if (user.Uloga != KorisnikTip.VLASNIK)
                {
                    return BadRequest();
                }
                if (HttpContext.Current.Session["fc"] == null)
                {
                    return BadRequest();
                }
                 int id = (int)HttpContext.Current.Session["fc"];
                foreach(FitnesCentar fc in user.FitnesCentri)
                {
                    if (fc.Id == id)
                    {
                        if (korisnik.FitnesCentri.Count > 0)
                        {
                            return BadRequest();
                        }
                        fc.TrenerKorisnickoIme.Add(korisnik.KorisnickoIme);
                        korisnik.FitnesCentri.Add(fc);
                        HttpContext.Current.Session["fc"] = null;
                        break;
                    }
                }
            }
            Baza.Korisnici.Add(korisnik.KorisnickoIme, korisnik);
            korisnik.PretvoriDatumUTrazenuFormu();

            Baza.IspisiKorisnike();
            return Ok("uspeh");
        }
        public IHttpActionResult Put(Korisnik korisnik)
        {
            Korisnik ulogovan = (Korisnik)HttpContext.Current.Session["user"];
            if (korisnik == null || ulogovan == null)
            {
                return BadRequest();
            }
            if (korisnik.Ime == null || korisnik.Lozinka == null
                || korisnik.Prezime == null 
                || korisnik.Email == null )
            {
                return BadRequest();
            }
            korisnik.KorisnickoIme = ulogovan.KorisnickoIme;
            korisnik.Uloga = ulogovan.Uloga;
            korisnik.GrupniTreninzi = ulogovan.GrupniTreninzi;
            korisnik.FitnesCentri = ulogovan.FitnesCentri;
            korisnik.PretvoriDatumUTrazenuFormu();
            Baza.Korisnici[ulogovan.KorisnickoIme] = korisnik;
            HttpContext.Current.Session["user"] = korisnik;
            Baza.IspisiKorisnike();
            return Ok("uspeh");
        }
    }
}
