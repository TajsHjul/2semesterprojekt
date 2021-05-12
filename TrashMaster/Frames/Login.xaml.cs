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

namespace TrashMaster.Frames
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();

            //MenuHeader er utilgængelig/usynlig ved opstart af program, og kræver et gyldigt login at initiere.
            ((MainWindow)Application.Current.MainWindow).MenuHeader.Visibility = Visibility.Hidden;
        }

        private void AttemptLogin(string username, string password)
        {
            //hardcoded login
            string hcUsername = "4ma";
            string hcPassword = "random";

            //hvis parametre passer, gør interface tilgængeligt og naviger til 'Oversigt' siden. Ellers giv fejlbesked.
            if (username == hcUsername && password == hcPassword)
            {
                MessageBox.Show("Login godkendt.");
                ((MainWindow)Application.Current.MainWindow).MenuHeader.Visibility = Visibility.Visible;
                ((MainWindow)Application.Current.MainWindow).MainNavigationFrame.Content = new Overblik();
            }
            else
            {
                textblock_Login.Foreground = Brushes.Red;
                textblock_Login.Text = "Ugyldigt Login.";
            }
        }

        private void button_Login_Click(object sender, RoutedEventArgs e)
        {
            //Forsøg login ved klik af 'Login' knap.
            //Brug textbox+passwordbox som parametre
            AttemptLogin(textbox_Username.Text, passwordbox_Password.Password.ToString());
        }
    }
}
