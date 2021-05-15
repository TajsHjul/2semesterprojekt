using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TrashMaster.Frames
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    /// 

    public partial class Login : Page
    {
        //hardcoded login
        private static string hcUsername = "4ma";
        private string hcPassword = "random";
        public static string loginTime;

        public Login()
        {
            InitializeComponent();

            //MenuHeader er utilgængelig/usynlig ved opstart af program, og kræver et gyldigt login at initiere.
            ((MainWindow)Application.Current.MainWindow).MenuHeader.Visibility = Visibility.Hidden;
        }

        private void AttemptLogin(string username, string password)
        {
            //hvis parametre passer, gør interface tilgængeligt og naviger til 'Oversigt' siden. Ellers giv fejlbesked.
            if (username == hcUsername && password == hcPassword)
            {
                //gem login-tidspunkt
                loginTime = DateTime.Now.ToString();

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

        //log brug af appen, metode kaldes ved logud
        public static void LogUse(string logouttime)
        {
            //laver \userLog mappe til at gemme login/logud timestamp. 
            string userLogFolder = @"C:\userLog";
            string fileName = "UserLog.txt";

            //Lav userlog mappe hvis den ikke allerede findes.
            if (Directory.Exists(userLogFolder) == false)
            {
                Directory.CreateDirectory(userLogFolder);
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(userLogFolder, fileName)))
                {
                    outputFile.WriteLine("Login\t\t\t" + "  Logud\t\t\t" + "\tBruger\n");
                }
            }

            //brug streamwriter til at skrive login tidspunkt, logud tidspunkt + brugernavn
            //true for appendline (ny linje i samme fil)
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(userLogFolder, fileName), true))
            {
                outputFile.WriteLine(loginTime + "  -  " + logouttime + "\t\t " + hcUsername);
            }
        }
    }
}
