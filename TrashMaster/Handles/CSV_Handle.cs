using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace TrashMaster.Handles
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    class CSV_Handle
    {
        public static string currentFile { get; set; }

        //Læser .CSV fil og håndterer dataintegritet - returnerer resultat som liste //unfin
        public static List<Trash> ReadCSVFile(string filePath)
        {
            string [] lines = File.ReadAllLines(filePath);

            IEnumerable<Trash> data = from l in lines.Skip(1)
                       let split = l.Split(',')
                       select new Trash
                       {
                           Mængde = decimal.Parse(split[0]),
                           Måleenhed = (Trash.måleenhed)Enum.Parse(typeof(Trash.måleenhed), split[1]),
                           Affaldskategori = (Trash.affaldskategori)Enum.Parse(typeof(Trash.affaldskategori), split[2]),
                           Affaldsbeskrivelse = split[3],
                           Ansvarlig = split[4],
                           VirksomhedID = int.Parse(split[5]),
                           Dato = Convert.ToDateTime(split[6])
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
                    return ReadCSVFile(filename);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + csvStructError());
                return null;
            }
        }

        //Eksporter datagrid til .csv fil
        public static void ExportCSV(DataGrid gridName)
        {
            try
            {
                //Instantier sfd + parametre
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".csv";
                sfd.Filter = ".CSV Files (*.csv)|*.csv";

                //Vis sfd og få bool på visning
                Nullable<bool> resultSFD = sfd.ShowDialog();

                //Marker alle celler og eksporter til valgte sfd path (sfd.FileName);
                if (resultSFD == true)
                {

                    //(A)Hvis selectionmode er sat til single, set til Extended så 'SelectAllCells' metoden bliver tilgængelige. Sæt derefter tilbage til Single.
                    if( gridName.SelectionMode != (DataGridSelectionMode)SelectionMode.Extended)
                    {
                        gridName.SelectionMode = (DataGridSelectionMode)SelectionMode.Extended;
                    }

                    string filename = sfd.FileName;
                    gridName.SelectAllCells();
                    gridName.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                    ApplicationCommands.Copy.Execute(null, gridName);
                    gridName.UnselectAllCells();
                    String resultX = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                    File.AppendAllText(filename, resultX.Replace(';',','), UnicodeEncoding.UTF8);

                    //(A)Sat tilbage til Single
                    gridName.SelectionMode = (DataGridSelectionMode)SelectionMode.Single;

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
                    "\nMængde + decimal" +
                    "\nMåleenhed + [Colli, Stk, Ton, Kilogram, Gram, M3, Liter, Hektoliter]" +
                    "\nAffaldskategori + [Batterier, Biler, Elektronikaffald, ImprægneretTræ, Inventar, OrganisskAffald, Papogpapir, PlastEmballager, PVC]" +
                    "\nAffaldsbeskrivelse + string" +
                    "\nAnsvarlig + string" +
                    "\nVirksomhedID + int" +
                    "\nDato + string";

            return csvStructError;
        }


    }
}
