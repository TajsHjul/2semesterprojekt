using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TrashMaster.UserControls
{
    class GraphLogic
    {
        //GraphLogic-klassen. Tænkes at skulle stå for den "logiske" del af graf-arbejdet.
        int xlabl = 0;
        int holabl = 39;
        List<double> snupData = new List<double>();
        private static readonly string connectionString = @"Server = trashmaster.database.windows.net; Database = trashmaster1; User Id = extuser01; Password = GNUpluslinux!;";
        public void GenerateDatapoints()
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
                DateTime today = new DateTime();
                command.CommandText = String.Format("SELECT Mængde FROM Trash Where Affaldskategori='Biler' ;", DateTime.Today.AddDays(-39), today);

                
                

                using (SqlDataReader reader = command.ExecuteReader())

                {
                    while (reader.Read())

                    {
                        snupData.Add(Convert.ToDouble(reader[0])*50);
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
            
            //Metode til generering af X-akse labels. Ved ikke helt om den er nødvendig
            return DateTime.Today.AddDays(xnotch - holabl).ToString().Remove(10);
        }

        public string VertAxisLabel(int ynotch)
        {
            //Metode til generering af Y-akse labels. Ved ikke helt om den er nødvendig
            return null;
        }
        public double GivePointValue(int dataset, double xvalue)
        {
            GenerateDatapoints();
            //Genererer lige pt tilfældige tal via Random-class. Tanken var at denne metode skulle give det ønskede yvalue baseret på et dataset og en xvalue
            int Min = 0;
            int Max = 500;

            Random randNum = new Random();

            //Sleep() er nødvendig med Random-class, da den ellers bare genererer de samme tal igen og igen :(
            Thread.Sleep(60);
            double yvalue = randNum.Next(Min, Max);
            return snupData[Convert.ToInt32(xvalue)];
        }
    }
}
