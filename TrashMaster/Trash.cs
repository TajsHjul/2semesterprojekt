using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrashMaster
{
    class Trash
    {
        public int Id { get; set; }
        public decimal Mængde { get; set; }
        public måleenhed Måleenhed { get; set; }
        public affaldskategori Affaldskategori { get; set; }
        public string Affaldsbeskrivelse { get; set; }
        public string Ansvarlig { get; set; }
        public int VirksomhedID { get; set; }
        public DateTime Dato { get; set; }

        public enum måleenhed
        {
            Colli, 
            Stk, 
            Ton,
            Kilogram,
            Gram,
            M3,
            Liter,
            Hektoliter
        }
        public enum affaldskategori
        {
            Batterier,
            Biler,
            Elektronikaffald,
            ImprægneretTræ,
            Inventar,
            OrganiskAffald,
            Papogpapir,
            Plastemballager,
            PVC
        }

        public Trash()
        {
            Mængde = 0.0M;
            Måleenhed = måleenhed.Kilogram;
            Affaldskategori = affaldskategori.Batterier;
            Affaldsbeskrivelse = "N/A";
            Ansvarlig = "N/A";
            VirksomhedID = 0;
            Dato = DateTime.UtcNow;
        }

        public Trash(decimal mængde, måleenhed måleenhed, affaldskategori affaldskategori, string affaldsbeskrivelse, string ansvarlig, int virksomhedid, DateTime dato)
        {
            Mængde = mængde;
            Måleenhed = måleenhed;
            Affaldskategori = affaldskategori;
            Affaldsbeskrivelse = affaldsbeskrivelse;
            Ansvarlig = ansvarlig;
            VirksomhedID = virksomhedid;
            Dato = dato;
        }
    }
}
