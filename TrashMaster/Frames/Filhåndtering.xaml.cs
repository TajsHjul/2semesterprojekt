using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

            DataContext = CSV_Handle.ReadCSVFile(@"D:\BAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\TestMappe\YAAP.csv");
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

        public void Tilføj_Valgte(object sender, RoutedEventArgs e)
        {
            foreach (Trash item in Filhåndtering_GRID.SelectedItems)
            {
                SQL_Handle.AddToDB(item, "Trash", true);
            }

            MessageBox.Show("De valgte rækker er blevet tilføjet til databasen");
        }
        public void Tilføj_Alle(object sender, RoutedEventArgs e)
        {
            //Tilføj valgte rækker til db
            Filhåndtering_GRID.SelectAllCells();

            foreach (Trash item in Filhåndtering_GRID.SelectedItems)
            {
                SQL_Handle.AddToDB(item, "Trash", true);
            }

            MessageBox.Show("Alle rækkerne er blevet tilføjet til databasen.");
        }

        //Gør tilføj knapperne utilgængelige hvis en række ikke er valgt.
        private void IsItemSelected(object sender, MouseButtonEventArgs e)
        {
            if (Filhåndtering_GRID.SelectedItems.Count > 0)
            {
                Button_Tilføj_Valgte.IsEnabled = true;
            }
        }
    }
}
