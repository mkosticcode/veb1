using MyWebApp.BazaPodataka;
using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace MyWebApp.Controllers
{
    [RoutePrefix("logins")]
    public class LoginsController : ApiController
    {
        [HttpGet, Route("")]
        public RedirectResult Index()
        {

            var requestUri = Request.RequestUri.Authority;
            var redir = "http://" + requestUri + "/Html/Login.html";
            return Redirect(redir);
        }
        public Korisnik Get()
        {
            Korisnik ulogovan = (Korisnik)HttpContext.Current.Session["user"];

            return ulogovan;

        }
        [HttpPost, Route("login")]
        public IHttpActionResult Post([FromBody]dynamic value)
        {
            string korisnickoIme = value.korisnickoIme.ToString();
            string sifra = value.sifra.ToString();
            if (korisnickoIme.Equals("") || sifra.Equals("") || loggedIn() )
            {
                return BadRequest();
            }
            if (!Baza.Korisnici.ContainsKey(korisnickoIme))
            {
                return BadRequest();
            }
            if (!Baza.Korisnici[korisnickoIme].Lozinka.Equals(sifra))
            {
                return BadRequest();
            }
            if (Baza.Korisnici[korisnickoIme].Deleted == true)
            {
                return BadRequest();
            }
          
            Korisnik user = Baza.Korisnici[korisnickoIme];

            HttpContext.Current.Session["user"] = (Korisnik)user;
            
            return Ok("uspeh");
        }
        [HttpGet, Route("logoff")]
        public IHttpActionResult logoff()
        {           

            HttpContext.Current.Session["user"] = null;

            return Ok("uspeh");
        }
        [HttpGet, Route("LoggedIn")]
        public Korisnik LoggedIn()
        {
            if (loggedIn())
            {
                Korisnik user = (Korisnik)HttpContext.Current.Session["user"];

                return user;
            }

            return null;
        }
        public static bool loggedIn()
        {
            Korisnik user = null;
            user = (Korisnik)HttpContext.Current.Session["user"];
            if (user == null)
            {
                return false;
            }
            return true;
        }
        #region redirect
        [HttpGet, Route("HomeRedirect")]
        public string DetaljRedirect()
        {

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            return (baseUrl + "/Html/Home.html");
        }
        #endregion redirect
    }
}
