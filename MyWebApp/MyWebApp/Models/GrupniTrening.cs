using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public enum TreningTip
    {
        YOGA = 1,
        LES_MILLS_TONE,
        BODY_PUMP
    }
    public class GrupniTrening
    {
        private string naziv;
        private TreningTip tipTreninga;
        private FitnesCentar mestoTreninga;
        private int trajanjeTreninga;
        private string datumIVremeTreninga;
        private int maksimalanBrojPosetilaca;
        private bool deleted;
        private string originalniDatumVreme;
        List<string> posetioci;

        public string Naziv { get => naziv; set => naziv = value; }
        public TreningTip TipTreninga { get => tipTreninga; set => tipTreninga = value; }
        public FitnesCentar MestoTreninga { get => mestoTreninga; set => mestoTreninga = value; }
        public int TrajanjeTreninga { get => trajanjeTreninga; set => trajanjeTreninga = value; }
        public string DatumIVremeTreninga { get => datumIVremeTreninga; set => datumIVremeTreninga = value; }
        public int MaksimalanBrojPosetilaca { get => maksimalanBrojPosetilaca; set => maksimalanBrojPosetilaca = value; }
        public List<string> Posetioci { get => posetioci; set => posetioci = value; }

        public bool Deleted { get => deleted; set => deleted = value; }
        public string OriginalniDatumVreme { get => originalniDatumVreme; set => originalniDatumVreme = value; }

        public GrupniTrening()
        {
            Posetioci = new List<string>();
        }

        public GrupniTrening(string naziv, TreningTip tipTreninga, FitnesCentar mestoTreninga, int trajanjeTreninga, string datumIVremeTreninga, int maksimalanBrojPosetilaca)
        {
            Naziv = naziv;
            TipTreninga = tipTreninga;
            MestoTreninga = mestoTreninga;
            TrajanjeTreninga = trajanjeTreninga;
            DatumIVremeTreninga = datumIVremeTreninga;
            MaksimalanBrojPosetilaca = maksimalanBrojPosetilaca;
            Posetioci = new List<string>();
        }

        public GrupniTrening(string naziv, TreningTip tipTreninga, FitnesCentar mestoTreninga, int trajanjeTreninga, string datumIVremeTreninga, int maksimalanBrojPosetilaca, List<string> posetioci, bool deleted) : this(naziv, tipTreninga, mestoTreninga, trajanjeTreninga, datumIVremeTreninga, maksimalanBrojPosetilaca)
        {
            Posetioci = posetioci;
            Deleted = deleted;
        }
        public void PretvoriDatumUTrazenuFormu()
        {
            this.originalniDatumVreme = this.datumIVremeTreninga;
            string[] split = this.datumIVremeTreninga.Split('T');
            string[] split2 = split[0].Split('-');
            this.datumIVremeTreninga = split2[2] + '/' + split2[1] + '/' + split2[0]+ ' '+ split[1];
        }
        public void Izmeni(GrupniTrening gt)
        {
            TipTreninga = gt.TipTreninga;
            TrajanjeTreninga = gt.TrajanjeTreninga;
            DatumIVremeTreninga = gt.DatumIVremeTreninga;
            MaksimalanBrojPosetilaca = gt.MaksimalanBrojPosetilaca;
            OriginalniDatumVreme = gt.OriginalniDatumVreme;
        }
    }
}