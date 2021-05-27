using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
            buttonsAvailable();
        }


        public void Tilføj_Valgte_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Trash item in Filhåndtering_GRID.SelectedItems)
                {
                    SQL_Handle.AddToDB(item, "Trash", true);
                }

                MessageBox.Show("De valgte rækker er blevet tilføjet til databasen");
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Tilføj_Alle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Filhåndtering_GRID.SelectAllCells();

                foreach (Trash item in Filhåndtering_GRID.SelectedItems)
                {
                    SQL_Handle.AddToDB(item, "Trash", true);
                }

                MessageBox.Show("Alle rækkerne er blevet tilføjet til databasen.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Dropzone_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"c:\Dropzone");
        }

        //Gør 'Tilføj valgte rækker' tilgængelig når mindst én række er valgt.
        private void IsItemSelected(object sender, MouseButtonEventArgs e)
        {
            if (Filhåndtering_GRID.SelectedItems.Count > 0)
            {
                Button_Tilføj_Valgte.IsEnabled = true;
            }
        }

        //Drag'n'drop
        private void DragAndDropCSV(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var file = files[0];
                DataContext = CSV_Handle.ReadCSVFile(file);

                buttonsAvailable();
            }
        }

        //Formater DateTime når kolonnen genereres.
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy:MM:dd HH:mm";

            if (e.PropertyType == typeof(Decimal))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:0.00}";
        }

        private void buttonsAvailable()
        {
            //gør 'tilføj alle rækker' og 'gem til fil' tilgængelig, hvis en fil er blevet indlæst til datagrid.
            if (Filhåndtering_GRID.Items.Count != 0)
            {
                Button_Tilføj_Alle.IsEnabled = true;

                menuitem_TilføjAlleRækker.IsEnabled = true;
                menuitem_TilføjValgteRækker.IsEnabled = true;
            }
        }
    }
}
