using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrashMaster.Handles
{
    class SQL_Handle : Trash
    {
        private static readonly string connectionString = @"Server = trashmaster.database.windows.net; Database = trashmaster1; User Id = extuser01; Password = GNUpluslinux!;";

        //sender SQL query til DB uden return
        public static void SqlQuery(string fullSQLquery)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(fullSQLquery, connection);
                using (SqlDataReader reader = command.ExecuteReader()) { }
                MessageBox.Show("Succesfull SqlQuery Executed.");
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

        // Tilføj 'Trash' til db med 'Trash' som parameter.

        public static void AddToDB(Trash trash, string tablename)
        {

            SqlConnection connection = new SqlConnection(connectionString);

            string fullSQLquery = String.Format("INSERT INTO " + tablename + " (Mængde, Måleenhed, Affaldskategori, Affaldsbeskrivelse, Ansvarlig, VirksomhedID, Dato) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", trash.Mængde, trash.Måleenhed, trash.Affaldskategori, trash.Affaldsbeskrivelse, trash.Ansvarlig, trash.VirksomhedID, trash.Dato);

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(fullSQLquery, connection);
                using (SqlDataReader reader = command.ExecuteReader()) { }
                MessageBox.Show("Affaldsregistreringen er nu tilføjet til databasen.");
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

        public static void EditDB(Trash trash, string tablename, int rowId)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            //string fullSQLquery = String.Format("UPDATE " + tablename + " SET Mængde = '{0}', Måleenhed = '{1}', Affaldskategori = '{2)', Affaldsbeskrivelse = '{3}', Ansvarlig = '{4}', VirksomhedID = '{5}' WHERE TrashId = '{7}'",
            //trash.Mængde, trash.Måleenhed, trash.Affaldskategori, trash.Affaldsbeskrivelse, trash.Ansvarlig, trash.VirksomhedID, rowId);

            string fullSQLquery = String.Format("UPDATE TRASH SET Mængde = '{0}', Måleenhed = '{1}', Affaldskategori = '{2}', Affaldsbeskrivelse = '{3}', Ansvarlig = '{4}', VirksomhedID = '{5}', Dato = CAST('{6}' AS smalldatetime) WHERE TrashId = '{7}'",
                trash.Mængde, trash.Måleenhed, trash.Affaldskategori, trash.Affaldsbeskrivelse, trash.Ansvarlig, trash.VirksomhedID, trash.Dato, rowId);


            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(fullSQLquery, connection);
                using (SqlDataReader reader = command.ExecuteReader()) { }
                MessageBox.Show("Row has been edited and saved to database.");
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

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
    }
}
