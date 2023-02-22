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
    [RoutePrefix("trener")]
    public class TrenersController : ApiController
    {
        [HttpPost, Route("DeleteGT")]
        public void DeleteGT([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null || user.Uloga != KorisnikTip.TRENER)
            {
                return ;
            }
            foreach (GrupniTrening gtt in user.GrupniTreninzi)
            {
                string v = value.naziv;
                if (gtt.Naziv.Equals(v))
                {
                    if (Baza.ProveraDatuma(gtt.DatumIVremeTreninga, 2) < 0) { break; }
                    if (gtt.Posetioci.Count > 0) { break; }
                    HttpContext.Current.Session["gp"] = null;
                    gtt.Deleted = true;
                    Baza.IspisiKorisnike();
                    break;
                }
            }
        }
        public List<GrupniTrening> Get()
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null || user.Uloga != KorisnikTip.TRENER)
            {
                return null;
            }
            List<GrupniTrening> l = new List<GrupniTrening>(user.GrupniTreninzi.Where(x => x.Deleted == false));

            return l;
        }
        
        public IHttpActionResult Post(GrupniTrening gt)
        {
            if (gt == null)
            {
                return BadRequest();
            }
            if (gt.DatumIVremeTreninga == null || gt.MaksimalanBrojPosetilaca == 0
                 || gt.Naziv == null )
            {
                return BadRequest();
            }
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user.Uloga != KorisnikTip.TRENER)
            {
                return BadRequest();
            }
            if (HttpContext.Current.Session["gp"]!=null)
            {
                gt.PretvoriDatumUTrazenuFormu();
                if(Baza.ProveraDatuma(gt.DatumIVremeTreninga, 2) < 3) { return BadRequest(); }
                foreach (GrupniTrening gtt in user.GrupniTreninzi)
                {
                    if (gtt.Naziv.Equals((string)HttpContext.Current.Session["gp"]))
                    {
                        HttpContext.Current.Session["gp"] = null;
                        gtt.Izmeni(gt);
                        Baza.IspisiKorisnike();
                        return Ok("uspeh");
                    }
                }
                
                HttpContext.Current.Session["gp"] = null;
            }
            foreach(GrupniTrening gtUser in user.GrupniTreninzi)
            {
                if (gtUser.Naziv.Equals(gt.Naziv)) { return BadRequest(); }
            }
            gt.MestoTreninga = user.FitnesCentri[0];
            gt.PretvoriDatumUTrazenuFormu();
            if (Baza.ProveraDatuma(gt.DatumIVremeTreninga, 2) < 3) { return BadRequest(); }
            user.GrupniTreninzi.Add(gt);
            Baza.IspisiKorisnike();
            return Ok("uspeh");
        }
        [HttpPost, Route("posetiociGT")]
        public List<Korisnik> posetiociGT([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null || user.Uloga != KorisnikTip.TRENER || value.naziv == null)
            {
                return null;
            }
            List<Korisnik> posetioci = new List<Korisnik>();
            foreach (GrupniTrening gt in user.GrupniTreninzi)
            {
                if (gt.Naziv.Equals((string)value.naziv))
                {
                    foreach (string s in gt.Posetioci)
                    {
                        posetioci.Add(Baza.Korisnici[s]);
                    }
                    return posetioci;
                }
            }
            return posetioci;
        }
        [HttpPost, Route("postaviGT")]
        public GrupniTrening postaviGT([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];

            if (user.Uloga != KorisnikTip.TRENER )
            {
                return null;
            }
            else
            {
                string gp = (string)value.naziv;
                foreach (GrupniTrening gtt in user.GrupniTreninzi)
                {

                    if (gtt.Naziv.Equals(gp))
                    {
                        HttpContext.Current.Session["gp"] = gp;
                        return gtt;
                    }
                }
              
            }

            return null;
        }
        #region SearchSort
        [HttpPost, Route("pretraga")]
        public List<GrupniTrening> pretraga([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];

            if (user ==null)
            {
                return null;
            }
            List<GrupniTrening> nadjeno = new List<GrupniTrening>();
            string nazivGT = value.nazivGT;
            string tipGT = value.tipGT; if (tipGT == null) { tipGT = ""; }
            string vremeDS = value.vremeDS; if (vremeDS == null) { vremeDS = ""; }
            string vremeGS = value.vremeGS; if (vremeGS == null) { vremeGS = ""; }

            int godinaOtvaranjaD, godinaOtvaranjaG;
            Int32.TryParse(vremeDS, out godinaOtvaranjaD);
            Int32.TryParse(vremeGS, out godinaOtvaranjaG);
            List<GrupniTrening> l = new List<GrupniTrening>(user.GrupniTreninzi.Where(x => x.Deleted == false));

            foreach (GrupniTrening g in l)
            {
                string[] s = g.DatumIVremeTreninga.Split('/');
                string[] s2 = s[2].Split(' ');
                int godina = Int32.Parse(s2[0]);
                if (!g.Naziv.Contains(nazivGT)) { continue; }
                if (!g.TipTreninga.ToString().Contains(tipGT)) { continue; }
                if (godinaOtvaranjaD != 0 && godinaOtvaranjaD > godina) { continue; }
                if (godinaOtvaranjaG != 0 && godinaOtvaranjaG < godina) { continue; }
                nadjeno.Add(g);
            }

            return nadjeno;

        }
        [HttpPost, Route("sortiranje")]
        public List<GrupniTrening> sortiranje([FromBody]dynamic value)
        {
            Korisnik user = (Korisnik)HttpContext.Current.Session["user"];

            if (user == null)
            {
                return null;
            }
            List<GrupniTrening> nadjeno = new List<GrupniTrening>(user.GrupniTreninzi.Where(x => x.Deleted == false));

           
            string uslov = value.Uslov;
            string smer = value.Smer;

            switch (uslov)
            {
                case "nazivGTSort":
                    if (smer.Equals("vece"))
                    {
                        nadjeno.Sort(delegate (GrupniTrening g, GrupniTrening p)
                        {
                            return g.Naziv.CompareTo(p.Naziv);
                        });
                    }
                    else
                    {
                        nadjeno.Sort(delegate (GrupniTrening g, GrupniTrening p)
                        {
                            return p.Naziv.CompareTo(g.Naziv);
                        });
                    }
                    break;
                case "tipGTSort":
                    if (smer.Equals("vece"))
                    {
                        nadjeno.Sort(delegate (GrupniTrening g, GrupniTrening p)
                        {
                            return g.TipTreninga.CompareTo(p.TipTreninga);
                        });
                    }
                    else
                    {
                        nadjeno.Sort(delegate (GrupniTrening g, GrupniTrening p)
                        {
                            return p.TipTreninga.CompareTo(g.TipTreninga);
                        });
                    }
                    break;
                case "godinaGTSort":
                    if (smer.Equals("vece"))
                    {
                        nadjeno.Sort(delegate (GrupniTrening g, GrupniTrening p)
                        {
                            return uporediVremena(g.DatumIVremeTreninga, p.DatumIVremeTreninga);
                        });
                    }
                    else
                    {
                        nadjeno.Sort(delegate (GrupniTrening g, GrupniTrening p)
                        {
                            return uporediVremena(p.DatumIVremeTreninga, g.DatumIVremeTreninga);
                        });
                    }
                    break;

            }
            return nadjeno;

        }
        public int uporediVremena(string prvi, string drugi)
        {

            string[] s = prvi.Split('/');
            string[] sf = s[2].Split(' ');
            int godina = Int32.Parse(sf[0]);
            string[] s2 = drugi.Split('/');
            string[] sf2 = s2[2].Split(' ');
            int godina2 = Int32.Parse(sf2[0]);          
            if (godina > godina2) { return 1; }
            if (godina2 < godina) { return -1; }
            
            return 1;
        }
        #endregion SearchSort
    }
}
