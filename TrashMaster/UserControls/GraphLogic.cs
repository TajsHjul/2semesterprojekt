using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TrashMaster.UserControls
{
    //Skrevet af Tajs Hjulmann Mølgård
    class GraphLogic
    {
        //GraphLogic-klassen. Tænkes at skulle stå for den "logiske" del af graf-arbejdet.
        
        List<double> snupData = new List<double>();
        List<DateTime> snupDato = new List<DateTime>();
        List<string> snupEnhed = new List<string>();
        //connectionstring kan evt håndteres af SQL_Handle
        private static readonly string connectionString = File.ReadAllLines(System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData

                             )
                             +
                             "/JETtm/connstring.txt").First();
        public void GenerateDatapoints(string kategori, int vid)
        {
            //Skal måske bruge til at sortere indkommende data til brug i GivePointValue()

            string sql = null;
            SqlCommand command;

            SqlConnection connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);

            try
            {
                connection.Open();
                command.Connection = connection;
                DateTime thirtytoday = DateTime.Now.AddDays(-30);
                command.CommandText = String.Format("SELECT Mængde,Dato, Måleenhed FROM Trash Where Affaldskategori='" + kategori + "' AND VirksomhedID ='" + vid + "' AND DATEDIFF(day,Dato,GETDATE()) between 0 and 31;");




                using (SqlDataReader reader = command.ExecuteReader())

                {
                    while (reader.Read())

                    {
                        snupData.Add(Convert.ToDouble(reader[0]) *10);
                        snupDato.Add(Convert.ToDateTime(reader[1]));
                        snupEnhed.Add(reader[2].ToString());
                    }

                    //Her kan der evt. tilføjes en removerange



                    reader.Close();

                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }
        public string HoAxisLabel(int xnotch)
        {
            if (snupDato.Count() != 0)
            {
                if (xnotch < snupDato.Count)
                {
                    return snupDato[xnotch].ToString().Remove(12);
                }
                else
                {
                    return snupDato[snupDato.Count() - 1].AddDays(xnotch-snupData.Count()+1).ToString().Remove(12);
                }
            }
            else
            {
                return "_nodata_";
            }
            
            //Metode til generering af X-akse labels. Ved ikke helt om den er nødvendig
            
        }

        public string VertAxisLabel(int ynotch)
        {
            //Metode til generering af Y-akse labels. Ved ikke helt om den er nødvendig
            return null;
        }

        public double GivePointValue(int dataset, double xvalue)
        {

            if (snupData.Count > xvalue)
            {
                switch (snupEnhed[Convert.ToInt32(xvalue)])
                {
                    
                    case "Ton":
                        return snupData[Convert.ToInt32(xvalue)] * 1000;
                    case "Gram":
                        return snupData[Convert.ToInt32(xvalue)]/1000;
                    case "Kilogram":
                        return snupData[Convert.ToInt32(xvalue)];
                    case "M3":
                        return snupData[Convert.ToInt32(xvalue)];
                    case "Hektoliter":
                        return snupData[Convert.ToInt32(xvalue)]*10;
                    case "Liter":
                        return snupData[Convert.ToInt32(xvalue)]*1000;

                    default:
                        return snupData[Convert.ToInt32(xvalue)];



                }
                


            }
            else
            {
                return 0.0;
            }


        }

        public int SnupdataLength()
        {
            return snupData.Count();
        }
    }
}
