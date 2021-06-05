using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using TrashMaster.Handles;

namespace TrashMaster.Frames
{

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

        //Skrevet af Edgar
        public editDB()
        {
            InitializeComponent();

            //sæt itemssource for comboboxes til enum(s) fra Trash klasse.
            cmbAffaldskategori.ItemsSource = Enum.GetValues(typeof(Trash.affaldskategori));
            cmbMåleenhed.ItemsSource = Enum.GetValues(typeof(Trash.måleenhed));
        }

        //Skrevet af Edgar
        private void Rediger_Click(object sender, RoutedEventArgs e)
        {
            //convert textbox_Dato to SQL accepted format.
            //string textboxDate = textbox_Dato.Text;
            //var textboxDateConv = DateTime.ParseExact(textboxDate, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture).ToString("M.dd.yyyy HH:mm:ss");

            try
            {
                //lav Trash object baseret på felterne i editDB page
                //Disse felter sættes fra 'Overblik' vinduet 
                Trash dbEdit = new Trash
                {
                    Mængde = Convert.ToDecimal(textbox_Mængde.Text),
                    Måleenhed = (Trash.måleenhed)Enum.Parse(typeof(Trash.måleenhed), cmbMåleenhed.Text),
                    Affaldskategori = (Trash.affaldskategori)Enum.Parse(typeof(Trash.affaldskategori), cmbAffaldskategori.Text),
                    Affaldsbeskrivelse = textbox_Affaldsbeskrivelse.Text,
                    Ansvarlig = textbox_Ansvarlig.Text,
                    VirksomhedID = Convert.ToInt32(textbox_VirksomhedID.Text)
            };

                //editDB metode, som gør brug af UPDATE SQL Query.
                SQL_Handle.EditDB(dbEdit, "Trash", Convert.ToInt32(textbox_Id.Text));
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Skrevet af Edgar
        //Kun tal og punktum i 'Mængde' box.
        private void textbox_Mængde_OnlyNumbersPlease(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9.]+");
        }

        //Skrevet af Edgar
        //Kun tal i 'VirksomhedID' box.
        private void textbox_VirksomhedID_OnlyNumbersPlease(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        //Skrevet af Tajs
        private void LinkValues()
        {
            switch (cmbAffaldskategori.SelectedItem)
            {
                case Trash.affaldskategori.Batterier:
                case Trash.affaldskategori.Elektronikaffald:
                case Trash.affaldskategori.ImprægneretTræ:
                case Trash.affaldskategori.Plastemballager:
                case Trash.affaldskategori.PVC:

                    //Tillad kun Ton, Kilogram og Gram - ved at ekskludere de andre måleenheder - når Batterier er valgt.
                    var vægt = Enum.GetValues(typeof(Trash.måleenhed)).Cast<Trash.måleenhed>()
                   .Except(new Trash.måleenhed[] { Trash.måleenhed.Colli, Trash.måleenhed.Stk, Trash.måleenhed.M3, Trash.måleenhed.Liter, Trash.måleenhed.Hektoliter });

                    cmbMåleenhed.ItemsSource = vægt;
                    break;
                case Trash.affaldskategori.Inventar:
                case Trash.affaldskategori.OrganiskAffald:
                case Trash.affaldskategori.Papogpapir:

                    //Tillad kun M3, Liter og Hektoliter - ved at ekskludere de andre måleenheder - når Inventar, Organisk Affald eller PapogPapir er valgt.
                    var volumen = Enum.GetValues(typeof(Trash.måleenhed)).Cast<Trash.måleenhed>()
                   .Except(new Trash.måleenhed[] { Trash.måleenhed.Colli, Trash.måleenhed.Stk, Trash.måleenhed.Ton, Trash.måleenhed.Kilogram, Trash.måleenhed.Gram });

                    cmbMåleenhed.ItemsSource = volumen;
                    break;
                case Trash.affaldskategori.Biler:


                    //Tillad kun Stk - ved at ekskludere de andre måleenheder - når Biler er valgt.
                    var antal = Enum.GetValues(typeof(Trash.måleenhed)).Cast<Trash.måleenhed>()
                   .Except(new Trash.måleenhed[] { Trash.måleenhed.Colli, Trash.måleenhed.M3, Trash.måleenhed.Liter, Trash.måleenhed.Hektoliter, Trash.måleenhed.Ton, Trash.måleenhed.Kilogram, Trash.måleenhed.Gram });

                    cmbMåleenhed.ItemsSource = antal;
                    break;

                default:
                    break;
            }
        }

        //Skrevet af Edgar
        private void cmbAffaldskategori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LinkValues();
        }
    }
}
