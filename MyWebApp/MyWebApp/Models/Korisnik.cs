using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public enum KorisnikTip
    {
        POSETILAC = 1,
        TRENER,
        VLASNIK
    }
    public enum PolTip
    {
        MUSKI = 1,
        ZENSKI
    }
    public class Korisnik
    {
        private string korisnickoIme;
        private string lozinka;
        private string ime;
        private string prezime;
        private PolTip pol;
        private string email;
        private string datumRodjenja;
        private KorisnikTip uloga;
        private bool deleted;

        List<GrupniTrening> grupniTreninzi;
        List<FitnesCentar> fitnesCentri;

        public string KorisnickoIme { get => korisnickoIme; set => korisnickoIme = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public PolTip Pol { get => pol; set => pol = value; }
        public string Email { get => email; set => email = value; }
        public string DatumRodjenja { get => datumRodjenja; set => datumRodjenja = value; }
        public KorisnikTip Uloga { get => uloga; set => uloga = value; }
        public List<GrupniTrening> GrupniTreninzi { get => grupniTreninzi; set => grupniTreninzi = value; }
        public List<FitnesCentar> FitnesCentri { get => fitnesCentri; set => fitnesCentri = value; }
        public bool Deleted { get => deleted; set => deleted = value; }

        public Korisnik()
        {
            GrupniTreninzi = new List<GrupniTrening>();
            FitnesCentri = new List<FitnesCentar>();
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, PolTip pol, string email, string datumRodjenja, KorisnikTip uloga)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            GrupniTreninzi = new List<GrupniTrening>();
            FitnesCentri = new List<FitnesCentar>();
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, PolTip pol, string email, string datumRodjenja, KorisnikTip uloga, List<GrupniTrening> grupniTreninzi, List<FitnesCentar> fitnesCentri) : this(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja, uloga)
        {
            GrupniTreninzi = grupniTreninzi;
            FitnesCentri = fitnesCentri;
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, PolTip pol, string email, string datumRodjenja, KorisnikTip uloga, List<GrupniTrening> grupniTreninzi, List<FitnesCentar> fitnesCentri, bool deleted) : this(korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja, uloga, grupniTreninzi, fitnesCentri)
        {
            Deleted = deleted;
        }

        public void PretvoriDatumUTrazenuFormu()
        {
            string[] split = this.DatumRodjenja.Split('-');
            this.DatumRodjenja = split[2] + '/' + split[1] + '/' + split[0];
        }

    }
}