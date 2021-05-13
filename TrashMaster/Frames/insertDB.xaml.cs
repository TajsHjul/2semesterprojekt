using System;
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

                SQL_Handle.AddToDB(dbInsert, "Trash");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
