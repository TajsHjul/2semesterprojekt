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
using TrashMaster.Frames;

namespace TrashMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            //window placement
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //start-side
            MainNavigationFrame.Content = new Login();
        }

        //Naviger to 'Overblik' siden.
        private void Overblik_Click(object sender, RoutedEventArgs e)
        {
            Overblik overblik = new Overblik();
            MainNavigationFrame.Content = overblik;
        }
    }
}
