using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace TrashMaster.Handles
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    class CSV_Handle
    {
        public static List<Trash> ReadCSVFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            IEnumerable<Trash> data = from l in lines.Skip(1)

                                      //dabigsplit
                                      let split = l.TrimStart('"').TrimEnd('"').Split(new[] { "\",\"" } , StringSplitOptions.None)
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
            try
            {
                //Instantier sfd + parametre
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.DefaultExt = ".csv";
                sfd.Filter = ".CSV Files (*.csv)|*.csv";
                string csvPath = sfd.FileName;
                //Vis sfd og få bool på visning
                Nullable<bool> resultSFD = sfd.ShowDialog();

                if (resultSFD == true)
                {
                    
                    string Csvpath = sfd.FileName;
                    System.IO.StreamWriter csvFileWriter = new StreamWriter(Csvpath, false);

                    //Skriver alle rækker til fil

                    //loop antallet af rækker   
                    for (int i = 0; i <= gridName.Items.Count - 2; i++)
                    {
                        string dataFromGrid = "";

                 
                        //loop antallet af kolonner
                        for (int j = 0; j <= gridName.Columns.Count - 1; j++)
                        {
                            //
                            if (j == 0)
                            {
                                dataFromGrid = "\"" + ((DataRowView)gridName.Items[i]).Row.ItemArray[j].ToString() + "\"";
                            }

                            //De to næste else-if statements konverterer string værdien af de tilsvarende enums til den numeriske værdi i csv filen
                            else if (j == 2)
                            {
                                string enumStringValue = (string)((DataRowView)gridName.Items[i]).Row.ItemArray[j];
                                Trash.måleenhed enhedConv = (Trash.måleenhed)Enum.Parse(typeof(Trash.måleenhed), enumStringValue);
                                int bigCONVOenhed = (int)enhedConv;
                                dataFromGrid = dataFromGrid + ',' + "\"" + bigCONVOenhed + "\"";
                            }
                            else if (j == 3)
                            {
                                string enumStringValue = (string)((DataRowView)gridName.Items[i]).Row.ItemArray[j];
                                Trash.affaldskategori enhedConv = (Trash.affaldskategori)Enum.Parse(typeof(Trash.affaldskategori), enumStringValue);
                                int bigCONVOaffald = (int)enhedConv;
                                dataFromGrid = dataFromGrid + ',' + "\"" + bigCONVOaffald + "\"";
                            }
                            //default
                            else
                            {
                                dataFromGrid = dataFromGrid + ',' + "\"" + ((DataRowView)gridName.Items[i]).Row.ItemArray[j].ToString() + "\"";
                            }
                        }
                        csvFileWriter.WriteLine(dataFromGrid);
                    }
                    csvFileWriter.Flush();
                    csvFileWriter.Close();
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
                    "\nID + Int" +
                    "\nMængde + decimal" +
                    "\nMåleenhed + enum [Colli/1, Stk/2, Ton/3, Kilogram/4, Gram/5, M3/6, Liter/7, Hektoliter/8]" +
                    "\nAffaldskategori + enum [Batterier/1, Biler/2, Elektronikaffald/3, ImprægneretTræ/4, Inventar/5, OrganisskAffald/6, Papogpapir/7, PlastEmballager/8, PVC/9]" +
                    "\nAffaldsbeskrivelse + string" +
                    "\nAnsvarlig + string" +
                    "\nVirksomhedID + int" +
                    "\nDato + DateTime";

            return csvStructError;
        }
    }
}
