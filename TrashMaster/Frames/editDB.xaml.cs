using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using TrashMaster.Handles;

namespace TrashMaster.Frames
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    public partial class editDB : Page
    {
        //Gør felterne (textbox, combobox) i editDB tilgængelige med {get;). 
        //Når en række i DataGrid er valgt på 'Overblik' page og brugeren trykker 'Rediger', kopieres oplysningerne fra Overblik_GRID ind i 'editDB' page felterne.
        //For at oplysningerne kan indtatastes/'kopieres' i felterne, skal de som vist her gøres tilgængelige vha. get { return *felt*}. 
        //Da datatypen er af 'Textbox' klassen, kan man fra 'Overblik' page bruge syntax som tilhøreres Textbox klassen (fx. textbox.text = "").
        //Dette benyttes i 'Overblik' vinduet til at sætte textbox.text og combobox.SelectedItem tilsvarende den valgte række i Overblik_GRID.

        public TextBox get_Textbox_Id { get { return textbox_Id; } }
        public TextBox get_Textbox_Mængde { get { return textbox_Mængde; } }
        public Object get_cmb_Måleenhed { get { return cmbMåleenhed.SelectedItem; } }
        public Object get_cmb_Affaldskategori { get { return cmbAffaldskategori.SelectedItem; } }
        public TextBox get_Textbox_Affaldsbeskrivelse { get { return textbox_Affaldsbeskrivelse; } }
        public TextBox get_Textbox_Ansvarlig { get { return textbox_Ansvarlig; } }
        public TextBox get_Textbox_VirksomhedID { get { return textbox_VirksomhedID; } }
        public TextBox get_Textbox_Dato { get { return textbox_Dato; } }

        public editDB()
        {
            InitializeComponent();

            //sæt itemssource for comboboxes til enum(s) fra Trash klasse.
            cmbAffaldskategori.ItemsSource = Enum.GetValues(typeof(Trash.affaldskategori));
            cmbMåleenhed.ItemsSource = Enum.GetValues(typeof(Trash.måleenhed));
        }

        private void Rediger_Click(object sender, RoutedEventArgs e)
        {
            //convert to SQL
            string msStartDt = textbox_Dato.Text;
            var str = DateTime.ParseExact(msStartDt, "M/dd/yyyy hh:mm:ss tt",
                                          CultureInfo.InvariantCulture).ToString("M.dd.yyyy HH:mm:ss");


            try
            {
                //lav Trash object baseret på felterne i editDB page.
                Trash dbEdit = new Trash
                {
                    Mængde = Convert.ToDecimal(textbox_Mængde.Text),
                    Måleenhed = (Trash.måleenhed)Enum.Parse(typeof(Trash.måleenhed), cmbMåleenhed.Text),
                    Affaldskategori = (Trash.affaldskategori)Enum.Parse(typeof(Trash.affaldskategori), cmbAffaldskategori.Text),
                    Affaldsbeskrivelse = textbox_Affaldsbeskrivelse.Text,
                    Ansvarlig = textbox_Ansvarlig.Text,
                    VirksomhedID = Convert.ToInt32(textbox_VirksomhedID.Text),
                    Dato = Convert.ToDateTime(str)
            };

                //editDB metode, som gør brug af UPDATE SQL Query.
                SQL_Handle.EditDB(dbEdit, "Trash", Convert.ToInt32(textbox_Id.Text));
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Kun tal og punktum i 'Mængde' box.
        private void textbox_Mængde_OnlyNumbersPlease(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9.]+");
        }

        //Kun tal i 'VirksomhedID' box.
        private void textbox_VirksomhedID_OnlyNumbersPlease(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
    }
}
