using System;
using System.Globalization;
using System.IO;
using System.Windows;
using TrashMaster.Frames;
using TrashMaster.Handles;
using TrashMaster.Misc;
using System.Threading;

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

            //FileWatcher
            FSWatcher();

            //test123

            //window placement
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //start-side
            MainNavigationFrame.Content = new Overblik();
            textblock_Overblik.TextDecorations = TextDecorations.Underline;

            ////overkill, but one works
            //CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            //CultureInfo.CurrentUICulture = new CultureInfo("en-US", false);
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US", false);
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US", false);


            //allDemCultureSettings
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "";
            newCulture.DateTimeFormat.LongTimePattern = "yyyy:MM:dd HH:mm";
            newCulture.DateTimeFormat.DateSeparator = ":";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }

        //Naviger til 'Overblik' siden, understreg menupunkt.
        private void Overblik_Click(object sender, RoutedEventArgs e)
        {
            MainNavigationFrame.Content = new Overblik();
            textblock_Overblik.TextDecorations = TextDecorations.Underline;
            textblock_Filhåndtering.TextDecorations = null;
        }

        //Naviger til 'Filhåndtering' siden, understreg menupunkt.
        private void Filhåndtering_Click(object sender, RoutedEventArgs e)
        {
            MainNavigationFrame.Content = new Filhåndtering();
            textblock_Filhåndtering.TextDecorations = TextDecorations.Underline;
            textblock_Overblik.TextDecorations = null;
        }

        //Log ud
        private void LogUd_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Log ud?\nHåndtering af affaldsdata vil blive utilgængeligt.", "Bekræft log ud", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    //log use
                    Login.LogUse(DateTime.Now.ToString());

                    //naviger to login page
                    MainNavigationFrame.Content = new Login();
                    break;

                //blankt pt
                case MessageBoxResult.No:
                    break;
            }
        }


        private void Graf_Click(object sender, RoutedEventArgs e)

        {
            MainNavigationFrame.Content = new showGraph();
            textblock_Graf.TextDecorations = TextDecorations.Underline;
            textblock_Overblik.TextDecorations = null;
        }
        public void FSWatcher() //Lavet af JBR
        {
            // Definerer vores mappe som der skal watches af appen
            string dropzoneFolder = @"C:\Dropzone";

            //Lav dropzone mappe hvis den ikke allerede findes.
            if (Directory.Exists(dropzoneFolder) == false)
            {
                Directory.CreateDirectory(dropzoneFolder);
            }

            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = dropzoneFolder;

            // Vi holder øje med et nyt filename i mappen Dropzone, der kunne i realiteten addes andre parametre
            watcher.NotifyFilter = NotifyFilters.FileName;

            // Kigger kun efter .csv-filer.
            watcher.Filter = "*.csv";

            //Tilfoejer event handlers.
            //Specificerer hvad der goeres naar en fil aendres, skabes eller slettes.
            watcher.Created += OnCreated;

            //Starter overvågningen
            watcher.EnableRaisingEvents = true;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            //Prompt bruger 'OnCreated' event
            MessageBoxResult result = MessageBox.Show("Ny .CSV fil: " + e.FullPath + " er blevet registreret i Dropzone.\nVil du åbne denne fil?", "Dropzone", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    //Threading for opdatering af GUI element
                    this.Dispatcher.Invoke(() =>
                    {
                        //Naviger til filhåndtering
                        Filhåndtering filhåndtering = new Filhåndtering();
                        MainNavigationFrame.Content = filhåndtering;

                        //Sæt dataContext (Grid ItemsSource til return af CSV.Handle.ReadCSVFile() - returnerer en List<Trash>).
                        DataContext = CSV_Handle.ReadCSVFile(e.FullPath);
                    });

                    break;

                    //blankt pt
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
