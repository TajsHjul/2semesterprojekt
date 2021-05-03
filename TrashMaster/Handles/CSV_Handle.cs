using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TrashMaster.Handles
{
    class CSV_Handle
    {
        public static string currentFile { get; set; }

        //Datastruktur for CSV import/eksport
        public int Id { get; set; }
        public decimal Mængde { get; set; }
        public måleenhed Måleenhed { get; set; }
        public affaldskategori Affaldskategori { get; set; }
        public string Affaldsbeskrivelse { get; set; }
        public string Ansvarlig { get; set; }
        public int VirksomhedID { get; set; }
        public string Dato { get; set; }

        public enum måleenhed
        {
            Kg = 1,
            Meter = 2,
            Colli = 3
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

        //Læser .CSV fil og håndterer dataintegritet - returnerer resultat som liste //unfin
        public static List<CSV_Handle> ReadCSVFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var data = from l in lines.Skip(1)
                       let split = l.Split(',')
                       select new CSV_Handle
                       {
                           Id = int.Parse(split[0]),
                           Mængde = decimal.Parse(split[1]),
                           Måleenhed = (måleenhed)Enum.Parse(typeof(måleenhed), split[2]),
                           Affaldskategori = (affaldskategori)Enum.Parse(typeof(affaldskategori), split[3]),
                           Affaldsbeskrivelse = split[4],
                           Ansvarlig = split[5],
                           VirksomhedID = int.Parse(split[6]),
                           Dato = split[7]
                       };

            return data.ToList();
        }

        //Importerer valgt .csv fil til datagrid.
        //Metoden returnerer et objekt, så DataContext (bundet til DataGrid/'Overblik_GRID' kan sættes til resultatet af denne metode.
        public static object ImportCSV()
        {
            try
            {
                //Instantier OpenFileDialog + parametre
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = ".csv";
                ofd.Filter = "CSV Files (*.csv)|*.csv";

                //Vis sfd og få bool på visning
                Nullable<bool> resultOFD = ofd.ShowDialog();

                //Få det valgte filepath + navn. DataContext sættes til dette return (tres), for at få resultatet vist i Datagrid.
                if (resultOFD == true)
                {
                    string filename = ofd.FileName;

                    //ReadCSVFile metode fra samme klasse (CSV_Handle)
                    object Tres = CSV_Handle.ReadCSVFile(filename);
                    return Tres;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + csvStructError());
                return "";
            }
        }

        //Eksporter datagrid til .csv fil
        public static void ExportCSV(DataGrid gridName)
        {
            try
            {
                //Instantier sfd + parametre
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".jpg";
                sfd.Filter = "CSV Files (*.csv)|*.csv";

                //Vis sfd og få bool på visning
                Nullable<bool> resultSFD = sfd.ShowDialog();

                //Marker alle celler og eksporter til valgte sfd path (sfd.FileName);
                if (resultSFD == true)
                {
                    string filename = sfd.FileName;

                    gridName.SelectAllCells();
                    gridName.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                    ApplicationCommands.Copy.Execute(null, gridName);
                    gridName.UnselectAllCells();
                    String resultX = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                    File.AppendAllText(filename, resultX, UnicodeEncoding.UTF8);



                    //MessageBox.Show("Din fil er blevet gemt.\n\n" + sfd.FileName.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //custom error som efterspørger dataintegritet hvis ikke den overholdes af .csv filen.
        public static string csvStructError()
        {
            string csvStructError =
                "\n\nDet valgte dokuments struktur stemmer ikke overens med denne applikations forventninger." +
                    "\n\nApplikationen forventer følgende hovedkolonner med tilhørende dataintegritet:" +
                    "\n\nId + int" +
                    "\nMængde + decimal" +
                    "\nMåleenhed + [Kg, Meter, Colli]" +
                    "\nAffaldskategori + [Batterier, Biler, Elektronikaffald, ImprægneretTræ, Inventar, OrganisskAffald, Papogpapir, PlastEmballager, PVC]" +
                    "\nAffaldsbeskrivelse + string" +
                    "\nAnsvarlig + string" +
                    "\nVirksomhedID + int" +
                    "\nDato + string";

            return csvStructError;
        }
    }
}
