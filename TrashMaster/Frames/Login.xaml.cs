﻿using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using TrashMaster.Handles;
using TrashMaster.Misc;

namespace TrashMaster.Frames
{
    /// <summary>
    /// Skrevet af Edgar
    /// </summary>
    /// 

    public partial class Login : Page
    {

        public static string loginTime;

        private static string Username { get; set; }
        private string Password { get; set; }

        public Login()
        {
            InitializeComponent();

            //MenuHeader er utilgængelig/usynlig ved opstart af program, og kræver et gyldigt login at synliggøre.
            ((MainWindow)Application.Current.MainWindow).MenuHeader.Visibility = Visibility.Hidden;

        }


        private void button_Login_Click(object sender, RoutedEventArgs e)
        {

            //Kør login i ny thread
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    LoadingCircle.Visibility = Visibility.Visible;
                });

                this.textbox_Username.Dispatcher.Invoke(DispatcherPriority.Normal,
                 new Action(() => { Username = this.textbox_Username.Text; }));

                this.passwordbox_Password.Dispatcher.Invoke(DispatcherPriority.Normal,
                 new Action(() => { Password = this.passwordbox_Password.Password; }));

                if (SQL_Handle.TryLogin(Username, Password) == true)
                {
                    //log tidspunkt for login
                    loginTime = DateTime.Now.ToString();

                    this.Dispatcher.Invoke(() =>
                    {
                        //Hvis login godkendes, gør MenuHeader synlig og naviger bruger til Overblik siden.
                        ((MainWindow)Application.Current.MainWindow).MenuHeader.Visibility = Visibility.Visible;
                        ((MainWindow)Application.Current.MainWindow).MainNavigationFrame.Content = new Overblik();
                    });
                }
                else
                {
                    MessageBox.Show("Forkert brugernavn eller adgangskode");
                }

                this.Dispatcher.Invoke(() =>
                {
                    LoadingCircle.Visibility = Visibility.Hidden;
                });

            });

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
                    outputFile.WriteLine("Login\t\t\t" + "  Logud\t\t" + "\tBruger\n");
                }
            }

            //brug streamwriter til at skrive login tidspunkt, logud tidspunkt + brugernavn
            //true for appendline (ny linje i samme fil)
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(userLogFolder, fileName), true))
            {
                outputFile.WriteLine(loginTime + "  -  " + logouttime + "\t\t- " + Username);
            }
        }
    }
}
