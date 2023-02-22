using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class FitnesCentar
    {
        private string naziv;
        private string adresa;
        private int godinaOtvaranja;
        private string vlasnikKorisnickoIme;
        private double mesecnaClanarina;
        private double godisnjaClanarina;
        private double cenaTreninga;
        private double cenaGrupnogTreninga;
        private double cenaTreningaSaTrenerom;
        private int id;
        private bool deleted;
        private List<string> trenerKorisnickoIme;

        public string Naziv { get => naziv; set => naziv = value; }
        public string Adresa { get => adresa; set => adresa = value; }
        public int GodinaOtvaranja { get => godinaOtvaranja; set => godinaOtvaranja = value; }
        public string VlasnikKorisnickoIme { get => vlasnikKorisnickoIme; set => vlasnikKorisnickoIme = value; }
        public double MesecnaClanarina { get => mesecnaClanarina; set => mesecnaClanarina = value; }
        public double GodisnjaClanarina { get => godisnjaClanarina; set => godisnjaClanarina = value; }
        public double CenaTreninga { get => cenaTreninga; set => cenaTreninga = value; }
        public double CenaGrupnogTreninga { get => cenaGrupnogTreninga; set => cenaGrupnogTreninga = value; }
        public double CenaTreningaSaTrenerom { get => cenaTreningaSaTrenerom; set => cenaTreningaSaTrenerom = value; }

        public int Id { get => id; set => id = value; }
        public List<string> TrenerKorisnickoIme { get => trenerKorisnickoIme; set => trenerKorisnickoIme = value; }
        public bool Deleted { get => deleted; set => deleted = value; }

        public FitnesCentar()
        {
            TrenerKorisnickoIme = new List<string>();
        }

        public FitnesCentar(string naziv, string adresa, int godinaOtvaranja, string vlasnikKorisnickoIme, double mesecnaClanarina, double godisnjaClanarina, double cenaTreninga, double cenaGrupnogTreninga, double cenaTreningaSaTrenerom)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            VlasnikKorisnickoIme = vlasnikKorisnickoIme;
            MesecnaClanarina = mesecnaClanarina;
            GodisnjaClanarina = godisnjaClanarina;
            CenaTreninga = cenaTreninga;
            CenaGrupnogTreninga = cenaGrupnogTreninga;
            CenaTreningaSaTrenerom = cenaTreningaSaTrenerom;
            TrenerKorisnickoIme = new List<string>();
        }

        public FitnesCentar(string naziv, string adresa, int godinaOtvaranja, string vlasnikKorisnickoIme, double mesecnaClanarina, double godisnjaClanarina, double cenaTreninga, double cenaGrupnogTreninga, double cenaTreningaSaTrenerom, int id, List<string> trenerKorisnickoIme, bool deleted) : this(naziv, adresa, godinaOtvaranja, vlasnikKorisnickoIme, mesecnaClanarina, godisnjaClanarina, cenaTreninga, cenaGrupnogTreninga, cenaTreningaSaTrenerom)
        {
            TrenerKorisnickoIme = trenerKorisnickoIme;
            Deleted = deleted;
        }
        public void Izmeni(FitnesCentar fc)
        {
            Naziv = fc.naziv;
            Adresa = fc.adresa;
            GodinaOtvaranja = fc.godinaOtvaranja;
            VlasnikKorisnickoIme = fc.vlasnikKorisnickoIme;
            MesecnaClanarina = fc.mesecnaClanarina;
            GodisnjaClanarina = fc.godisnjaClanarina;
            CenaTreninga = fc.cenaTreninga;
            CenaGrupnogTreninga = fc.cenaGrupnogTreninga;
            CenaTreningaSaTrenerom = fc.cenaTreningaSaTrenerom;
        }
    }
}