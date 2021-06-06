using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;

namespace TrashMaster.Handles
{
    /// <summary>
    
    /// </summary>
    class SQL_Handle : Trash
    {
        private static string connectionString = File.ReadAllLines(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                             + "/JETtm/connstring.txt").First();

        /// Skrevet af Edgar
        //Forsøg at logge ind med de givne parametre
        public static bool TryLogin(string username, string password)
        {
            //Test om connectionstring fra .txt er gyldig
            try
            {
                //check for opdatering af connectionstring ved loginforsøg 
                connectionString = File.ReadAllLines(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                             + "/JETtm/connstring.txt").First();

                //Test om connectionstring fra .txt er gyldig
                SqlConnection connection = new SqlConnection(connectionString);

                //Hvis connectionString er gyldig:
                try
                {
                    connection.Open();
                    string fullSQLquery = "SELECT * FROM Users WHERE hcUsername = '" + username + "' AND hcPassword = '" + password + "'";
                    SqlDataAdapter sda = new SqlDataAdapter(fullSQLquery, connectionString);
                    DataTable dtbl = new DataTable();
                    sda.Fill(dtbl);

                    if (dtbl.Rows.Count == 1)
                    {
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
            catch
            {
                MessageBox.Show("Den indtastede connectionstring er ikke gyldig.\nIndtast oplysningerne i det følgende format:" +
                    "\n\nServer = SERVERNAME; Database = DBNAME; User Id = USERNAME; Password = PASSWORD;");
                return false;
            }
        }

        // Tilføj til db med 'Trash' som parameter.
        /// Skrevet af Edgar
        public static void AddToDB(Trash trash, string tablename, bool multiple)
        {

            SqlConnection connection = new SqlConnection(connectionString);

            string fullSQLquery = String.Format("INSERT INTO " + tablename + " (Mængde, Måleenhed, Affaldskategori, Affaldsbeskrivelse, Ansvarlig, VirksomhedID) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", trash.Mængde, trash.Måleenhed, trash.Affaldskategori, trash.Affaldsbeskrivelse, trash.Ansvarlig, trash.VirksomhedID);
            if (SQL_Handle.CheckDetID(tablename, trash.VirksomhedID) == true)
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(fullSQLquery, connection);
                    using (SqlDataReader reader = command.ExecuteReader()) { }

                    if (multiple == false)
                    {
                        MessageBox.Show("Affaldsregistreringen er nu tilføjet til databasen.");
                    }
                    else
                    {
                        //Hvis multiple er sat til true, tilføj manuelt messagebox for godkendt affaldsregistrering i den eksterne metode - for at undgå prompt for hver eneste tilføjelse //fx persistering af csv rækker
                    }
                }
                catch (Exception splep)
                {
                    MessageBox.Show(Convert.ToString(splep));
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                }
            }
            else
                MessageBox.Show("VirksomhedsID er ikke gyldigt.");

        }

        /// Skrevet af Edgar
        public static void EditDB(Trash trash, string tablename, int rowId)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string fullSQLquery = String.Format("UPDATE TRASH SET Mængde = '{0}', Måleenhed = '{1}', Affaldskategori = '{2}', Affaldsbeskrivelse = '{3}', Ansvarlig = '{4}', VirksomhedID = '{5}' WHERE TrashId = '{6}'",
                trash.Mængde, trash.Måleenhed, trash.Affaldskategori, trash.Affaldsbeskrivelse, trash.Ansvarlig, trash.VirksomhedID, rowId);


            if (SQL_Handle.CheckDetID(tablename, trash.VirksomhedID) == true)

            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(fullSQLquery, connection);
                    using (SqlDataReader reader = command.ExecuteReader()) { }
                    MessageBox.Show("Dataen er nu redigeret og gemt til databasen.");
                }
                catch (Exception splep)
                {
                    MessageBox.Show(Convert.ToString(splep));
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                }
            }
            else
                MessageBox.Show("Indtast gyldigt virksomhedsID");


        }

        /// Skrevet af Edgar
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

            catch (Exception splep)
            {
                MessageBox.Show(Convert.ToString(splep));
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }



        }

        /// Skrevet af Edgar
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
