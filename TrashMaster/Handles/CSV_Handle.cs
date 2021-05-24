using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
                           Id = int.Parse(split[0]),
                           Mængde = decimal.Parse(split[1]),
                           Måleenhed = (Trash.måleenhed)Enum.Parse(typeof(Trash.måleenhed), split[2]),
                           Affaldskategori = (Trash.affaldskategori)Enum.Parse(typeof(Trash.affaldskategori), split[3]),
                           Affaldsbeskrivelse = split[4],
                           Ansvarlig = split[5],
                           VirksomhedID = int.Parse(split[6]),
                           Dato = DateTime.Parse(split[7])
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
            ////Sæt list seperator til at være ";"
            //System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator = ";";


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

                    
                    string CsvFpath = sfd.FileName;
                    System.IO.StreamWriter csvFileWriter = new StreamWriter(CsvFpath, false);
                    string columnHeaderText = "";
                    int countColumn = gridName.Columns.Count - 1;
                    if (countColumn >= 0)
                    {
                        columnHeaderText = (gridName.Columns[0].Header).ToString();
                    }

                    //// kolonne headers
                    //for (int i = 1; i <= countColumn - 1; i++)
                    //{
                    //    columnHeaderText = "\"" + (gridName.Columns[i].Header).ToString() + "\",";
                    //    csvFileWriter.Write(columnHeaderText);
                    //}


                    // rækker
                    for (int i = 0; i <= gridName.Items.Count - 2; i++)
                    {
                        string dataFromGrid = "";


                        for (int j = 0; j <= gridName.Columns.Count - 1; j++)
                        {
                            if (j == 0)
                            {
                                dataFromGrid = "\"" + ((DataRowView)gridName.Items[i]).Row.ItemArray[j].ToString() + "\"";
                            }
                            else
                            {
                                dataFromGrid = dataFromGrid + ',' + "\"" + ((DataRowView)gridName.Items[i]).Row.ItemArray[j].ToString() + "\"";
                            }
                        }
                        csvFileWriter.WriteLine(dataFromGrid);
                    }



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
