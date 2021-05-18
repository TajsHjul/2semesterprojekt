using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrashMaster.Handles;

namespace TrashMaster.Frames
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    /// 
    public partial class Overblik : Page
    {
        public Overblik()
        {
            InitializeComponent();

            //Opdater grid på loadup.
            UpdateGrid(Overblik_GRID, "Trash");
        }

        //Sætter insertDB som frame content.
        private void Tilføj_Click(object sender, RoutedEventArgs e)
        {
            insertDB idb = new insertDB();
            ((MainWindow)Application.Current.MainWindow).MainNavigationFrame.Content = idb; 
        }

        //Sætter editDB som frame content.
        private void Rediger_Click(object sender, RoutedEventArgs e)
        {
            editDB edb = new editDB();
            ((MainWindow)Application.Current.MainWindow).MainNavigationFrame.Content = edb;

            try
            {
                //gem valgte række fra Grid som DataRowView. Elementerne er nu tilgængelige vha. datarow index.
                DataRowView dataRow = (DataRowView)Overblik_GRID.SelectedItems[0];

                //indsæt værdierne fra dataRow (kopi af valgte element fra mainGrid) i 'editDBwindow' felterne til redigering.
                int cellValueId = Convert.ToInt32(dataRow.Row.ItemArray[0]);
                edb.get_Textbox_Id.Text = cellValueId.ToString();
                edb.textbox_Id.IsReadOnly = true;

                //Decimal Formatting
                decimal cellValueMængde = Convert.ToDecimal(dataRow.Row.ItemArray[1]);
                string cellValueMængdeCONV = String.Format("{0:0.00}", cellValueMængde.ToString(System.Globalization.CultureInfo.InvariantCulture));
                edb.get_Textbox_Mængde.Text = cellValueMængdeCONV;

                //selectedItem value
                Trash.måleenhed cellValueMåleenhed = (Trash.måleenhed)Enum.Parse(typeof(Trash.måleenhed), (string)dataRow.Row.ItemArray[2]);
                edb.cmbMåleenhed.SelectedItem = cellValueMåleenhed;

                //selectedItem value
                Trash.affaldskategori cellValueAffaldskategori = (Trash.affaldskategori)Enum.Parse(typeof(Trash.affaldskategori), (string)dataRow.Row.ItemArray[3]);
                edb.cmbAffaldskategori.SelectedItem = cellValueAffaldskategori;

                string cellValueAffaldsbeskrivelse = Convert.ToString(dataRow.Row.ItemArray[4]);
                edb.get_Textbox_Affaldsbeskrivelse.Text = cellValueAffaldsbeskrivelse;

                string cellValueAnsvarlig = Convert.ToString(dataRow.Row.ItemArray[5]);
                edb.get_Textbox_Ansvarlig.Text = cellValueAnsvarlig;

                int cellValueVirksomhedID = Convert.ToInt32(dataRow.Row.ItemArray[6]);
                edb.get_Textbox_VirksomhedID.Text = cellValueVirksomhedID.ToString();

                string cellValueDato = Convert.ToString(dataRow.Row.ItemArray[7]);
                edb.get_Textbox_Dato.Text = cellValueDato.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Slet valgte række
        private void Slet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //gem valgte række fra mainGrid som DataRowView. Elementerne er nu tilgængelige vha. datarow index.
                DataRowView dataRow = (DataRowView)Overblik_GRID.SelectedItems[0];
                int cellValueId = Convert.ToInt32(dataRow.Row.ItemArray[0]);

                //prompt bruger om sletning af valgte række - hvis ja, kør removefromdb metode.

                MessageBoxResult result = MessageBox.Show("Slet valgte affaldsdata tilhørende Id: " + cellValueId.ToString() + " ?", "Slet Affaldsdata", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SQL_Handle.RemoveFromDB(cellValueId, "Trash");
                        UpdateGrid(Overblik_GRID, "Trash");
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vælg en række.\n " + ex.Message);
            }
        }

        //Eksporter alt i datagrid til .csv fil med CSV_Handle.ExportCSV(tablename) metode.
        private void Gem_Fil_Click(object sender, RoutedEventArgs e)
        {
            CSV_Handle.ExportCSV(Overblik_GRID);
        }

        //Åben .csv fil
        private void Åben_Fil_Click(object sender, RoutedEventArgs e)
        {
            DataContext = CSV_Handle.ImportCSV();
        }

        //Threading for opdatering af grid.
        //Vis LoadingCircle 
        private async void UpdateGrid(DataGrid gridName, string tableName)
        {
            DataContext = await RTU_Get_UpTime();
            Task<object> RTU_Get_UpTime() { return Task.Run(() => {

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                //Threading for opdatering af GUI element
                this.Dispatcher.Invoke(() =>
                {
                    LoadingCircle.Visibility = Visibility.Visible;
                });

                Thread.Sleep(1000);

                //Threading for opdatering af GUI element
                this.Dispatcher.Invoke(() =>
                {
                    LoadingCircle.Visibility = Visibility.Collapsed;
                });

                return SQL_Handle.QueryToSource("SELECT * FROM dbo." + tableName); 

            });

            }
        }

        //Gør rediger+slet knapperne utilgængelige hvis en række ikke er valgt.
        private void IsItemSelected(object sender, MouseButtonEventArgs e)
        {
            if (Overblik_GRID.SelectedItems.Count > 0)
            {
                Button_Rediger.IsEnabled = true;
                Button_Slet.IsEnabled = true;
            }
        }

        //Formater DateTime når kolonnen genereres.
        //Formater Decimal til sepperering med punktum og to decimaler
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy-MM-dd HH:mm:ss";

            if (e.PropertyType == typeof(Decimal))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:0.00}";
        }
    }
}
