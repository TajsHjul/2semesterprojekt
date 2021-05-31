using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TrashMaster.Handles;

namespace TrashMaster.Frames
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    public partial class insertDB : Page
    {
        public insertDB()
        {

            InitializeComponent();
            cmbAffaldskategori.ItemsSource = Enum.GetValues(typeof(Trash.affaldskategori));
            cmbMåleenhed.ItemsSource = Enum.GetValues(typeof(Trash.måleenhed));

        }

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

        private void cmbAffaldskategori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LinkValues();
        }

        private void Tilføj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Id tilføjes automatisk i DB - Dato har en standardvalue på DateTime.Now (sat i Trash klasse).
                Trash dbInsert = new Trash
                {
                    Mængde = Convert.ToDecimal(textbox_Mængde.Text),
                    Måleenhed = (Trash.måleenhed)Enum.Parse(typeof(Trash.måleenhed), cmbMåleenhed.Text),
                    Affaldskategori = (Trash.affaldskategori)Enum.Parse(typeof(Trash.affaldskategori), cmbAffaldskategori.Text),
                    Affaldsbeskrivelse = textbox_Affaldsbeskrivelse.Text,
                    Ansvarlig = textbox_Ansvarlig.Text,
                    VirksomhedID = Convert.ToInt32(textbox_VirksomhedID.Text),
                };

                SQL_Handle.AddToDB(dbInsert, "Trash", false);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Udfyld venligst alle registreringskolonner.\n" + ex.Message);
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
