using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrashMaster
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
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
            Colli = 1, 
            Stk = 2, 
            Ton = 3,
            Kilogram = 4,
            Gram = 5,
            M3 = 6,
            Liter = 7,
            Hektoliter = 8
        }
        public enum affaldskategori
        {
            Batterier = 1,
            Biler = 2,
            Elektronikaffald = 3,
            ImprægneretTræ = 4,
            Inventar = 5,
            OrganiskAffald = 6,
            Papogpapir = 7,
            Plastemballager = 8,
            PVC = 9
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
