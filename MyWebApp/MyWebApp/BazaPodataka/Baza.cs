using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MyWebApp.BazaPodataka
{
    public class Baza
    {
        public static List<FitnesCentar> FitnesCentri = new List<FitnesCentar>();
        public static Dictionary<string,Korisnik> Korisnici=new Dictionary<string, Korisnik>();
        public static List<Komentar> Komentari = new List<Komentar>();

        public static bool UcitajKomentare(string path)
        {
            bool uspeo = false;
            try
            {
                path = HostingEnvironment.MapPath(path);
                string jsonString = System.IO.File.ReadAllText(path);
                Komentari = (List<Komentar>)Upis.DeSerialize<List<Komentar>>(jsonString);
                uspeo = true;
            }
            catch (Exception e)
            {
                Komentari = new List<Komentar>();
            }
            return uspeo;
        }
        public static bool UcitajKorisnike(string path)
        {
            bool uspeo = false;
            try
            {
                path = HostingEnvironment.MapPath(path);
                string jsonString = System.IO.File.ReadAllText(path);
                Korisnici = (Dictionary<string, Korisnik>)Upis.DeSerialize<Dictionary<string, Korisnik>>(jsonString);
                uspeo = true;
            }
            catch (Exception e)
            {
                Korisnici = new Dictionary<string, Korisnik>();
            }
            return uspeo;
        }
        public static bool UcitajFitnesCentre(string path)
        {
            bool uspeo = false;
            try
            {
                path = HostingEnvironment.MapPath(path);
                string jsonString = System.IO.File.ReadAllText(path);
                FitnesCentri = (List<FitnesCentar>)Upis.DeSerialize<List<FitnesCentar>>(jsonString);
                uspeo = true;
            }
            catch (Exception e)
            {
                FitnesCentri = new List<FitnesCentar>();
            }
            return uspeo;
        }
        public static bool UcitajSvePodatke()
        {
            bool uspeo = false;
            if (UcitajKorisnike("~/App_Data/Korisnici.json") && UcitajKomentare("~/App_Data/Komentari.json"))             
            {
                uspeo = true;
            }
            return uspeo;
        }
        public static bool IspisiKomentare()
        {
            return IspisiKomentare("~/App_Data/Komentari.json");
        }
        public static bool IspisiKomentare(string path)
        {
            bool uspeo = false;
            try
            {
                path = HostingEnvironment.MapPath(path);
                Ispis.PrintSerialize(Komentari, path);
                uspeo = true;
            }
            catch (Exception e)
            {

            }
            return uspeo;
        }
        public static bool IspisiKorisnike()
        {
            return IspisiKorisnike("~/App_Data/Korisnici.json");
        }
        public static bool IspisiKorisnike(string path)
        {
            bool uspeo = false;
            try
            {
                path = HostingEnvironment.MapPath(path);
                Ispis.PrintSerialize(Korisnici, path);
                uspeo = true;
            }
            catch (Exception e)
            {

            }
            return uspeo;
        }
        public static bool IspisiFitnesCentre()
        {
            return IspisiFitnesCentre("~/App_Data/FitnesCentri.json");
        }
        public static bool IspisiFitnesCentre(string path)
        {
            bool uspeo = false;
            try
            {
                path = HostingEnvironment.MapPath(path);
                Ispis.PrintSerialize(FitnesCentri, path);
                uspeo = true;
            }
            catch (Exception e)
            {

            }
            return uspeo;
        }
        public static int ProveraDatuma(string datum,int mode)
        {
            string[] d = datum.Split('/');
            string[] p;
            if (mode == 0) {
                 d = datum.Split('/');
                
            }            
            else
            {
                p = datum.Split(' ');
                d = p[0].Split('/');
            }
            int dan = Int32.Parse(d[0]);
            int mesec = Int32.Parse(d[1]);
            int godina = Int32.Parse(d[2]);
            int danSada = DateTime.Now.Day;
            int mesecSada = DateTime.Now.Month;
            int godinaSada = DateTime.Now.Year;
            if (godina < godinaSada) { return -1; }
            else if (godina > godinaSada) { return 10; }
            else 
            {
                if (mesec < mesecSada) { return -1; }
                else if (mesec > mesecSada) { return 10; }
                else 
                {
                    if (dan < danSada) { return -1; }
                    else if (dan > danSada) { return dan - danSada; }
                    else { return 0; }
                }
            }
        }
    }
}