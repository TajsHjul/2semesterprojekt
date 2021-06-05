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
                    if (constraintOK(item) == true)
                    {
                        SQL_Handle.AddToDB(item, "Trash", true);
                    }
                    else
                    {
                        MessageBox.Show("Affaldspostering "+item.Id+" overholder ikke dette program's\nrestriktioner på måleenheder for pågældende\naffaldskategori...");
                    }
                    
                }
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
                    if (constraintOK(item) == true)
                    {
                        SQL_Handle.AddToDB(item, "Trash", true);
                    }
                    else
                    {
                        MessageBox.Show("Affaldspostering " + item.Id + " overholder ikke dette program's\nrestriktioner på måleenheder for pågældende\naffaldskategori...");
                    }

                }
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
            try
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
            catch (Exception)
            {
                MessageBox.Show("Applikationen understøtter ikke denne filtype. Indlæs venligt csv fil.\n" + CSV_Handle.csvStructError());
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

        private static bool constraintOK(Trash junk)
        {
            switch(junk.Affaldskategori)
            {
                case Trash.affaldskategori.Batterier:
                case Trash.affaldskategori.Elektronikaffald:
                case Trash.affaldskategori.ImprægneretTræ:
                case Trash.affaldskategori.Plastemballager:
                case Trash.affaldskategori.PVC:
                    {
                        switch(junk.Måleenhed)
                        {
                            case Trash.måleenhed.Gram:
                            case Trash.måleenhed.Kilogram:
                            case Trash.måleenhed.Ton:
                                {
                                    return true;
                                }
                            default:
                                {
                                    return false;
                                }
                        }
                        
                    }
                

                case Trash.affaldskategori.Inventar:
                case Trash.affaldskategori.OrganiskAffald:
                case Trash.affaldskategori.Papogpapir:
                    {
                        switch (junk.Måleenhed)
                        {
                            case Trash.måleenhed.Liter:
                            case Trash.måleenhed.M3:
                            case Trash.måleenhed.Hektoliter:
                                {
                                    return true;
                                }
                            default:
                                {
                                    return false;
                                }
                        }

                    }
                case Trash.affaldskategori.Biler:
                    {
                        switch (junk.Måleenhed)
                        {
                            case Trash.måleenhed.Stk:
                            case Trash.måleenhed.Colli:
                                {
                                    return true;
                                }
                            default:
                                {
                                    return false;
                                }
                        }

                    }
                default:
                    return false;
            }
        }
    }
}
