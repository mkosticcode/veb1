using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public enum OcenaTip
    {
        JEDAN = 1,
        DVA,
        TRI,
        CETRI,
        PET
    }
    public class Komentar
    {
        private Korisnik posetilac;
        private FitnesCentar fitnesCentar;
        private string tekst;
        private OcenaTip ocena;
        private bool deleted;
        private int idKomentara;
        private int idFC;

       

        public Korisnik Posetilac { get => posetilac; set => posetilac = value; }
        public FitnesCentar FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public string Tekst { get => tekst; set => tekst = value; }
        public OcenaTip Ocena { get => ocena; set => ocena = value; }
        public bool Deleted { get => deleted; set => deleted = value; }
        public int IdKomentara { get => idKomentara; set => idKomentara = value; }
        public int IdFC { get => idFC; set => idFC = value; }

        public Komentar()
        {
        }

        public Komentar(Korisnik posetilac, FitnesCentar fitnesCentar, string tekst, OcenaTip ocena, int idKomentara, int idFC, bool deleted)
        {
            Posetilac = posetilac;
            FitnesCentar = fitnesCentar;
            Tekst = tekst;
            Ocena = ocena;
            IdKomentara = idKomentara;
            IdFC = idFC;
            Deleted = deleted;
        }
    }
}