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
    [RoutePrefix("vlasnik")]
    public class VlasniksController : ApiController
    {
        public IHttpActionResult Post(FitnesCentar fc)
        {
            if (fc == null)
            {
                return BadRequest();
            }
            if (fc.Adresa == null || fc.CenaGrupnogTreninga == 0
                || fc.CenaTreninga == 0 || fc.CenaTreningaSaTrenerom == 0
                || fc.GodinaOtvaranja == 0 || fc.MesecnaClanarina == 0 || fc.Naziv == null)
            {
                return BadRequest();
            }
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user.Uloga != KorisnikTip.VLASNIK)
            {
                return BadRequest();
            }
            if (HttpContext.Current.Session["fc"] == null)
            {
                fc.Id = Baza.FitnesCentri.Count;
                user.FitnesCentri.Add(fc);
            }
            else
            {
                foreach(FitnesCentar fCentar in user.FitnesCentri)
                {
                    if (fCentar.Id == (int)HttpContext.Current.Session["fc"])
                    {
                        fCentar.Izmeni(fc);
                        HttpContext.Current.Session["fc"] = null;
                        Baza.IspisiKorisnike();
                        return Ok("uspeh"); 
                    }
                }
            }
            Baza.FitnesCentri.Add(fc);
            Baza.IspisiKorisnike();
            return Ok("uspeh");
        }
        public List<FitnesCentar> Get()
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null || user.Uloga!=KorisnikTip.VLASNIK)
            {
                return null;
            }
            List<FitnesCentar> l = new List<FitnesCentar>(user.FitnesCentri.Where(x => x.Deleted == false));

            return l;
        }
        public void Delete(int id)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            foreach (FitnesCentar fCentar in user.FitnesCentri)
            {
                if (fCentar.Id == id)
                {
                    fCentar.Deleted = true;
                    foreach (string s in fCentar.TrenerKorisnickoIme)
                    {
                        Baza.Korisnici[s].Deleted = true;
                        foreach(GrupniTrening gt in Baza.Korisnici[s].GrupniTreninzi)
                        {
                            gt.Deleted = true;
                        }

                    }
                    Baza.IspisiKorisnike();
                    break;
                }
            }
        }
        [HttpPost, Route("postaviFC")]
        public IHttpActionResult postaviFC([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];

            if (user.Uloga != KorisnikTip.VLASNIK || value.id == null)
            {
                return BadRequest();
            }
            else
            {
                int fc = (int)value.id;
                HttpContext.Current.Session["fc"] = fc;
            }

            return Ok("uspeh");
        }
        [HttpPost, Route("blokirajTrenera")]
        public IHttpActionResult blokirajTrenera([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];

            if (user.Uloga != KorisnikTip.VLASNIK || value.korisnickoIme == null)
            {
                return BadRequest();
            }
            else
            {
                List<Korisnik> treneri = vratiTrenere();
                foreach(Korisnik k  in treneri)
                {
                    string korisnickoIme = value.korisnickoIme;
                    if (k.KorisnickoIme.Equals(korisnickoIme))
                    {
                        k.Deleted = true;
                        foreach (GrupniTrening gt in k.GrupniTreninzi)
                        {
                            gt.Deleted = true;
                        }
                        Baza.IspisiKorisnike();
                        return Ok("uspeh");
                    }
                }
            }

            return BadRequest();
        }
        [HttpGet, Route("vratiTrenere")]
        public List<Korisnik> vratiTrenere()
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];

            if (user.Uloga != KorisnikTip.VLASNIK)
            {
                return null;
            }
            List<Korisnik> treneri = new List<Korisnik>();
            foreach(FitnesCentar fc in user.FitnesCentri)
            {
                foreach(string s in fc.TrenerKorisnickoIme)
                {
                    if (Baza.Korisnici[s].Deleted == false)
                    {
                        treneri.Add(Baza.Korisnici[s]);
                    }
                }
            }
            return treneri;
        }
    }
}
