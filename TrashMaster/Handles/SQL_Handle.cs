using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using TrashMaster.Frames;

namespace TrashMaster.Handles
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    class SQL_Handle : Trash
    {
        private static readonly string connectionString = File.ReadAllLines(System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData

                             )
                             +
                             "/JETtm/connstring.txt").First();


        //Forsøg at logge ind med de givne parametre
        public static bool TryLogin(string username, string password)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string fullSQLquery = "SELECT * FROM Users WHERE hcUsername = '" + username + "' AND hcPassword = '" + password + "'";
            try
            {
                connection.Open();
                SqlDataAdapter sda = new SqlDataAdapter(fullSQLquery, connectionString);
                DataTable dtbl = new DataTable();
                sda.Fill(dtbl);

                if (dtbl.Rows.Count == 1)
                {
                    ////Hvis login godkendes, gør MenuHeader synlig og naviger bruger til Overblik siden.
                    //((MainWindow)Application.Current.MainWindow).MenuHeader.Visibility = Visibility.Visible;
                    //((MainWindow)Application.Current.MainWindow).MainNavigationFrame.Content = new Overblik();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        // Tilføj til db med 'Trash' som parameter.

        public static void AddToDB(Trash trash, string tablename, bool multiple)
        {

            SqlConnection connection = new SqlConnection(connectionString);

            string fullSQLquery = String.Format("INSERT INTO " + tablename + " (Mængde, Måleenhed, Affaldskategori, Affaldsbeskrivelse, Ansvarlig, VirksomhedID) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", trash.Mængde, trash.Måleenhed, trash.Affaldskategori, trash.Affaldsbeskrivelse, trash.Ansvarlig, trash.VirksomhedID);
            if (SQL_Handle.CheckDetID(tablename, trash.VirksomhedID) == true)

            {
                connection.Open();

                SqlCommand command = new SqlCommand(fullSQLquery, connection);
                using (SqlDataReader reader = command.ExecuteReader()) { }

                
                    MessageBox.Show("Affaldsregistreringen er nu tilføjet til databasen.");
                
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            else
                MessageBox.Show("Hey yo, klaphat.\n Kun godkendte VirksomhedsID!!!");
            
        }


        public static void EditDB(Trash trash, string tablename, int rowId)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string fullSQLquery = String.Format("UPDATE TRASH SET Mængde = '{0}', Måleenhed = '{1}', Affaldskategori = '{2}', Affaldsbeskrivelse = '{3}', Ansvarlig = '{4}', VirksomhedID = '{5}' WHERE TrashId = '{6}'",
                trash.Mængde, trash.Måleenhed, trash.Affaldskategori, trash.Affaldsbeskrivelse, trash.Ansvarlig, trash.VirksomhedID, rowId);


            if (SQL_Handle.CheckDetID(tablename, trash.VirksomhedID) == true)

            {
                connection.Open();

                SqlCommand command = new SqlCommand(fullSQLquery, connection);
                using (SqlDataReader reader = command.ExecuteReader()) { }
                MessageBox.Show("Dataen er nu redigeret og gemt til databasen.");
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            else
                MessageBox.Show("Hey yo, klaphat.\n Kun godkendte VirksomhedsID!!!");


        }

        //Fjern fra DB med ID (unik) parameter
        public static void RemoveFromDB(int id, string tablename)
        {
            SqlConnection connection = new SqlConnection(connectionString);


            //"id" på datagrid hedder "TrashID" i databasen.
            string fullSQLquery = String.Format("DELETE FROM dbo." + tablename + " WHERE TrashID = {0}", id);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(fullSQLquery, connection);
                using (SqlDataReader reader = command.ExecuteReader()) { }

                MessageBox.Show("Affaldsdata med id: " + id + " er nu slettet fra databasen.");
            }
                
            catch(Exception splep)
            {
                MessageBox.Show(Convert.ToString(splep));
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
            
            

        }

        //Tager SQL query som første parameter og returnerer resultatet som DataTable.DefaultView (kan bruges som Datacontext7Itemssource etc).
        public static object QueryToSource(string fullSQLquery)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                //Create DataTable and populate it with sqlQuery results
                DataTable dt = new DataTable();
                using (connection)
                {
                    using (SqlCommand cmd = new SqlCommand(fullSQLquery, connection))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
                
                return dt.DefaultView;
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }

        }

        //Skrevet af Tajs Hjulmann
        private static bool? CheckDetID(string tablename, int vID)
        {
            SqlConnection connection = new SqlConnection(connectionString);


            //"id" på datagrid hedder "TrashID" i databasen.
            string fullSQLquery = String.Format("SELECT * FROM Virksomheder WHERE VirksomhedID= {0}", vID);

            
                connection.Open();

                SqlCommand command = new SqlCommand(fullSQLquery, connection);
                SqlDataReader reader = command.ExecuteReader();

                
                if (reader.HasRows == false)
                {
                    
                         return false;
                }
                else
                {

                    return true;
                }
                
           
            
        }
    }
}
