using System;
using System.Windows;
using System.Windows.Controls;
using TrashMaster.Handles;

namespace TrashMaster.Frames
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    public partial class removeDB : Page
    {
        public removeDB()
        {
            InitializeComponent();
        }

        //slet valgte bruger.
        private void removeFromDB_Click(object sender, RoutedEventArgs e)
        {
            //remove row by userinput ID
            try
            {
                SQL_Handle.RemoveFromDB(Convert.ToInt32(textbox_Id.Text), "Trash");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
