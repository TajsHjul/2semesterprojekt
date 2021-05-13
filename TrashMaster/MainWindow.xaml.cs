using System.Windows;
using TrashMaster.Frames;

namespace TrashMaster
{
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
            MainNavigationFrame.Content = new Overblik();
        }

        //Naviger til 'Overblik' siden.
        private void Overblik_Click(object sender, RoutedEventArgs e)
        {
            Overblik overblik = new Overblik();
            MainNavigationFrame.Content = overblik;
        }

        //Naviger til 'Filhåndtering' siden.
        private void Filhåndtering_Click(object sender, RoutedEventArgs e)
        {
            Filhåndtering filhåndtering = new Filhåndtering();
            MainNavigationFrame.Content = filhåndtering;
        }
    }
}
