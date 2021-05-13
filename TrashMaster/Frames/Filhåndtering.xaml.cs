using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrashMaster.Handles;

namespace TrashMaster.Frames
{
    /// <summary>
    /// Skrevet af: Edgar
    /// </summary>
    public partial class Filhåndtering : Page
    {
        public Filhåndtering()
        {
            InitializeComponent();
        }

        //Åben .csv fil
        private void Åben_Fil_Click(object sender, RoutedEventArgs e)
        {
            DataContext = CSV_Handle.ImportCSV();
        }

        //Eksporter alt i datagrid til .csv fil med CSV_Handle.ExportCSV(tablename) metode.
        private void Gem_Fil_Click(object sender, RoutedEventArgs e)
        {
            CSV_Handle.ExportCSV(Filhåndtering_GRID);
        }

        private void CSVTEST(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataRow = (DataRowView)Filhåndtering_GRID.SelectedItems[0];

                var selected = (Trash)Filhåndtering_GRID.SelectedItems;

                MessageBox.Show(selected.Mængde.ToString());




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }




            //try
            //{
            //Ammount of Rows
            //for (int i = 0; i < Filhåndtering_GRID.SelectedItems.Count; i++)
            //{
            //    DataRowView dataRow = (DataRowView)Filhåndtering_GRID.SelectedItems[i];

                //Each cell in row
                //for (int j = 0; j < dataRow.Row.ItemArray.Length; j++)
                //{
                //    Trash trash = new Trash
                //    {
                //        Mængde = Convert.ToDecimal(dataRow.Row.ItemArray[0]),
                //        Måleenhed = (Trash.måleenhed)Enum.Parse(typeof(Trash.måleenhed), (string)dataRow.Row.ItemArray[1]),
                //        Affaldskategori = (Trash.affaldskategori)Enum.Parse(typeof(Trash.affaldskategori), (string)dataRow.Row.ItemArray[2]),
                //        Affaldsbeskrivelse = Convert.ToString(dataRow.Row.ItemArray[3]),
                //        Ansvarlig = Convert.ToString(dataRow.Row.ItemArray[4]),
                //        VirksomhedID = Convert.ToInt32(dataRow.Row.ItemArray[5]),
                //        Dato = Convert.ToDateTime(dataRow.Row.ItemArray[6])
                //    };

                //    //SQL_Handle.AddToDB(trash, "Trash");
                //}
            //}

            //MessageBox.Show("legit sick");
            //}

            //catch(Exception ex)
            //{
            //    MessageBox.Show("k");
            //}
        }
    }
}
