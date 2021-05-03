using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrashMaster.Handles;

namespace TrashMaster.Frames
{
    /// <summary>
    /// Interaction logic for removeDB.xaml
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
