using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using MyWebApp.Models;
using MyWebApp.BazaPodataka;

namespace MyWebApp.Controllers
{
   // [RoutePrefix("Home")]
    public class HomeController : ApiController
    {
        [HttpGet, Route("")]
        public RedirectResult Index()
        {
            var requestUri = Request.RequestUri.Authority;
            var redir = "http://" + requestUri + "/Html/Home.html";
            return Redirect(redir);
        }
        public List<FitnesCentar> Get()
        {
            List<FitnesCentar> l =new List<FitnesCentar>( BazaPodataka.Baza.FitnesCentri.Where(x => x.Deleted == false));
            return l;
        }
        public FitnesCentar Get(int id)
        {
            List<FitnesCentar> l = new List<FitnesCentar>(BazaPodataka.Baza.FitnesCentri.Where(x => x.Deleted == false));

            foreach (FitnesCentar f in l)
            {
                if (f.Id == id)
                {
                    return f;
                }
            }
            return null; ;

        }
        [HttpPost, Route("sacuvajFC")]
        public IHttpActionResult sacuvajFC([FromBody]dynamic value)
        {
            HttpContext.Current.Session["detalj"] = value.id;

            return Ok(value.id);
        }
        [HttpGet, Route("ucitajFC")]
        public IHttpActionResult ucitajFC()
        {
            var id = HttpContext.Current.Session["detalj"];
            if (id == null)
            {
                return BadRequest();
            }
            return Ok(id);
        }
        [HttpGet, Route("getDetailGT")]
        public List<GrupniTrening> getDetailGT()
        {
            if (HttpContext.Current.Session["detalj"] == null)
            {
                return null;
            }
            var d = HttpContext.Current.Session["detalj"];
            int id =Int32.Parse(d.ToString());
            List<GrupniTrening> gt = new List<GrupniTrening>();
            foreach (FitnesCentar f in Baza.FitnesCentri)
            {
                if (f.Id == id)
                {
                  foreach(string s in f.TrenerKorisnickoIme)
                    {
                        
                        gt.AddRange(Baza.Korisnici[s].GrupniTreninzi.Where(x => x.Deleted == false && Baza.ProveraDatuma(x.DatumIVremeTreninga,1)>0) );
                    }
                    break;
                }
            }
            return gt;
        }
        [HttpPost, Route("poseti")]
        public IHttpActionResult poseti([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null || user.Uloga != KorisnikTip.POSETILAC)
            {
                return BadRequest();
            }
            if (HttpContext.Current.Session["detalj"] == null || value.naziv==null)
            {
                return BadRequest();
            }
            var k= HttpContext.Current.Session["detalj"];
            int id =Int32.Parse( k.ToString());
            string naziv = value.naziv;
            foreach (FitnesCentar f in Baza.FitnesCentri)
            {
                if (f.Id == id)
                {
                    foreach (string s in f.TrenerKorisnickoIme)
                    {
                        foreach(GrupniTrening g in Baza.Korisnici[s].GrupniTreninzi)
                        {
                            if (g.Naziv.Equals(naziv))
                            {
                                if (g.MaksimalanBrojPosetilaca > g.Posetioci.Count)
                                {
                                    foreach(string pose in g.Posetioci)
                                    {
                                        if (pose.Equals(user.KorisnickoIme))
                                        {
                                            return BadRequest();
                                        }
                                    }
                                    user.GrupniTreninzi.Add(g);
                                    if(!user.FitnesCentri.Any(x => x.Id == f.Id))
                                    {
                                        user.FitnesCentri.Add(f);
                                    }
                                    g.Posetioci.Add(user.KorisnickoIme);
                                    Baza.IspisiKorisnike();
                                    return Ok("uspeh");
                                }

                            }
                        }
                    }
                }
            }
            return Ok("uspeh");
        }
        [HttpPost, Route("odobri")]
        public IHttpActionResult odobri([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null || user.Uloga != KorisnikTip.VLASNIK)
            {
                return BadRequest();
            }
            if (HttpContext.Current.Session["detalj"] == null )
            {
                return BadRequest();
            }
            string idS = value.id;
            int id = Int32.Parse(idS);
            List<Komentar> k= new List<Komentar>(Baza.Komentari.Where(x => x.IdKomentara == id));
            if (k.Count == 1)
            {
                k[0].Deleted = false;
                Baza.IspisiKomentare();

                return Ok("uspeh");
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet, Route("komentari")]
        public List<Komentar> komentari()
        {
            var k = HttpContext.Current.Session["detalj"];
            int id = Int32.Parse(k.ToString());

            List<Komentar> komentari = new List<Komentar>();
            foreach(Komentar i in Baza.Komentari)
            {
                if (i.IdFC== id)
                {
                    komentari.Add(i);
                }
            }
            return komentari;
        }
        #region SearchSort
        [HttpPost, Route("pretraga")]
        public List<FitnesCentar> pretraga([FromBody]dynamic value)
        {
            List<FitnesCentar> nadjeno = new List<FitnesCentar>();
            string nazivFC = value.nazivFC;
            string adresaFC = value.adresaFC; if (adresaFC == null) { adresaFC = ""; }
            string godinaOtvaranjaDS = value.godinaOtvaranjaD; if (godinaOtvaranjaDS == null) { godinaOtvaranjaDS = ""; }
            string godinaOtvaranjaGS = value.godinaOtvaranjaG; if (godinaOtvaranjaGS == null) { godinaOtvaranjaGS = ""; }

            int godinaOtvaranjaD, godinaOtvaranjaG;
            Int32.TryParse(godinaOtvaranjaDS, out godinaOtvaranjaD);
            Int32.TryParse(godinaOtvaranjaGS, out godinaOtvaranjaG);
            List<FitnesCentar> l = new List<FitnesCentar>(BazaPodataka.Baza.FitnesCentri.Where(x => x.Deleted == false));

            foreach (FitnesCentar f in l)
            {
                if (!f.Naziv.Contains(nazivFC)) { continue;}
                if (!f.Adresa.Contains(adresaFC)) { continue; }
                if(godinaOtvaranjaD != 0 && godinaOtvaranjaD > f.GodinaOtvaranja) { continue; }
                if(godinaOtvaranjaG != 0 && godinaOtvaranjaG < f.GodinaOtvaranja) { continue; }
                nadjeno.Add(f);
            }

            return nadjeno;

        }
        [HttpPost, Route("sortiranje")]
        public List<FitnesCentar> sortiranje([FromBody]dynamic value)
        {
            List<FitnesCentar> nadjeno = new List<FitnesCentar>(BazaPodataka.Baza.FitnesCentri.Where(x => x.Deleted == false));
           
            string uslov = value.Uslov;
            string smer = value.Smer;
            switch (uslov)
            {
                case "nazivFCSort":
                    if (smer.Equals("vece"))
                    {
                        nadjeno.Sort(delegate (FitnesCentar g, FitnesCentar p)
                        {
                            return g.Naziv.CompareTo(p.Naziv);
                        });
                    }
                    else
                    {
                        nadjeno.Sort(delegate (FitnesCentar g, FitnesCentar p)
                        {
                            return p.Naziv.CompareTo(g.Naziv);
                        });
                    }
                    break;
                case "adresaFCSort":
                    if (smer.Equals("vece"))
                    {
                        nadjeno.Sort(delegate (FitnesCentar g, FitnesCentar p)
                        {
                            return g.Adresa.CompareTo(p.Adresa);
                        });
                    }
                    else
                    {
                        nadjeno.Sort(delegate (FitnesCentar g, FitnesCentar p)
                        {
                            return p.Adresa.CompareTo(g.Adresa);
                        });
                    }
                    break;
                case "godinaOtvaranjaFCSort":
                    if (smer.Equals("vece"))
                    {
                        nadjeno.Sort(delegate (FitnesCentar g, FitnesCentar p)
                        {
                            return g.GodinaOtvaranja.CompareTo(p.GodinaOtvaranja);
                        });
                    }
                    else
                    {
                        nadjeno.Sort(delegate (FitnesCentar g, FitnesCentar p)
                        {
                            return p.GodinaOtvaranja.CompareTo(g.GodinaOtvaranja);
                        });
                    }
                    break;

            }
            return nadjeno;

        }
        #endregion SearchSort
        #region redirect
        [HttpGet, Route("DetaljRedirect")]
        public string DetaljRedirect()
        {

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return (baseUrl + "/Html/Detalj.html");
        }
        [HttpGet, Route("LoginRedirect")]
        public string LoginRedirect()
        {

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return (baseUrl + "/Html/Login.html");
        }
        [HttpGet, Route("RegisterRedirect")]
        public string RegisterRedirect()
        {

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return (baseUrl + "/Html/Register.html");
        }
        [HttpGet, Route("ModifyRedirect")]
        public string ModifyRedirect()
        {

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return (baseUrl + "/Html/Modify.html");
        }
        [HttpGet, Route("VlasnikRedirect")]
        public string VlasnikRedirect()
        {

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return (baseUrl + "/Html/Vlasnik.html");
        }
        [HttpGet, Route("TrenerRedirect")]
        public string TrenerRedirect()
        {

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return (baseUrl + "/Html/Trener.html");
        }
        [HttpGet, Route("PosetilacRedirect")]
        public string PosetilacRedirect()
        {

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return (baseUrl + "/Html/Posetilac.html");
        }
        #endregion redirect

    }
}