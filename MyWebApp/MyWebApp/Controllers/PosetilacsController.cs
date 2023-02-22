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
    [RoutePrefix("posetilac")]
    public class PosetilacsController : ApiController
    {
        public List<GrupniTrening> Get()
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null || user.Uloga != KorisnikTip.POSETILAC)
            {
                return null;
            }
            List<GrupniTrening> l = new List<GrupniTrening>(user.GrupniTreninzi.Where(x => x.Deleted == false));

            return l;
        }
        [HttpPost, Route("napraviKomentar")]
        public IHttpActionResult Post([FromBody]dynamic value)
        {
            if (value.id == null|| value.ocena == null || value.tekst == null)
            {
                return BadRequest();
            }
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user.Uloga != KorisnikTip.POSETILAC)
            {
                return BadRequest();
            }
            
            string id = value.id;
            int i = Int32.Parse(id);
            OcenaTip ocena = value.ocena;
            string tekst = value.tekst;          
            Komentar k = new Komentar(user, Baza.FitnesCentri[i], tekst, ocena,Baza.Komentari.Count, Baza.FitnesCentri[i].Id, true);
            Baza.Komentari.Add(k);
            Baza.IspisiKomentare();
            return Ok("uspeh");
        }
        [HttpGet, Route("poseceniFC")]
        public List<FitnesCentar> poseceniFC()
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null || user.Uloga != KorisnikTip.POSETILAC)
            {
                return null;
            }
            List<FitnesCentar> l = new List<FitnesCentar>(user.FitnesCentri.Where(x => x.Deleted == false));

            return l;
        }
        
    }
}
