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


namespace TrashMaster.UserControls
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    /// Skrevet af Tajs Hjulmann Mølgård
    public partial class Graph : UserControl
    {
        string kategori;
        bool niceID ;
        // definerer max-værdier, margin og margin mellem punkterne
        const double margin = 10;
        double xmin = margin;
        double xmax;
        double ymax = margin;
        double ymin;
        // definerer step constant som bruges til at definere afstanden mellem punkter 
        const double step = 20;

        //ylabl og xlabl -- skal egenlig hentes fra Graphlogic-classen
        int xlabl = 0;
        int ylabl = 0;
        string NaughtyID = "";
        bool clickbool;

        public Graph()
        {
            bool clickbool = false;
            InitializeComponent();
            xmax = canGraph.Width - margin;
            ymin = canGraph.Height - margin;
        }
        private void Colli_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt32(VirksomhedsIDinput.Text);
                niceID = true;
            }
            catch
            {
                MessageBox.Show("Indtast venligst et gyldigt virksomhedsID i textboksen\ntil venstre for grafen.");
                niceID = false;
            }
            if (niceID != true)
            { MessageBox.Show(NaughtyID);
                affaldskategori1.IsEnabled = false;
                affaldskategori2.IsEnabled = false;
                affaldskategori3.IsEnabled = false;
                affaldskategori4.IsEnabled = false;
                affaldskategori5.IsEnabled = false;
                affaldskategori6.IsEnabled = false;
                affaldskategori7.IsEnabled = false;
                affaldskategori8.IsEnabled = false;
                affaldskategori9.IsEnabled = false;
            }
            else
            {
                
                canGraph.Children.Clear();
                affaldskategori1.IsEnabled = false;
                affaldskategori2.IsEnabled = false;
                affaldskategori3.IsEnabled = false;
                affaldskategori4.IsEnabled = false;
                affaldskategori5.IsEnabled = false;
                affaldskategori6.IsEnabled = false;
                affaldskategori7.IsEnabled = false;
                affaldskategori8.IsEnabled = false;
                affaldskategori9.IsEnabled = false;
                DrawYAxis("Colli ( ͡° ͜ʖ ͡°)", 0);
                ImageBrush hmm = new ImageBrush();
                
                canGraph.Background = hmm;
            }
            

        }
        private void stk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt32(VirksomhedsIDinput.Text);
                niceID = true;
            }
            catch
            {
                MessageBox.Show("Indtast venligst et gyldigt virksomhedsID i textboksen\ntil venstre for grafen.");
                niceID = false;
            }
            if (niceID != true)
            { MessageBox.Show(NaughtyID);
                affaldskategori1.IsEnabled = false;
                affaldskategori2.IsEnabled = false;
                affaldskategori3.IsEnabled = false;
                affaldskategori4.IsEnabled = false;
                affaldskategori5.IsEnabled = false;
                affaldskategori6.IsEnabled = false;
                affaldskategori7.IsEnabled = false;
                affaldskategori8.IsEnabled = false;
                affaldskategori9.IsEnabled = false;
            }
            else
            {
                
                canGraph.Children.Clear();
                affaldskategori1.IsEnabled = false;
                affaldskategori2.IsEnabled = false;
                affaldskategori3.IsEnabled = false;
                affaldskategori4.IsEnabled = false;
                affaldskategori5.IsEnabled = false;
                affaldskategori6.IsEnabled = false;
                affaldskategori7.IsEnabled = false;
                affaldskategori8.IsEnabled = false;
                affaldskategori9.IsEnabled = false;


                affaldskategori2.IsEnabled = true;
                DrawYAxis("Stk.", 1);
            }
            
        }


        private void Masse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt32(VirksomhedsIDinput.Text);
                niceID = true;
            }
            catch
            {
                
                MessageBox.Show("Indtast venligst et gyldigt virksomhedsID i textboksen\ntil venstre for grafen.");
                niceID = false;
            }
            if (niceID != true)
            { MessageBox.Show(NaughtyID);
                affaldskategori1.IsEnabled = false;
                affaldskategori2.IsEnabled = false;
                affaldskategori3.IsEnabled = false;
                affaldskategori4.IsEnabled = false;
                affaldskategori5.IsEnabled = false;
                affaldskategori6.IsEnabled = false;
                affaldskategori7.IsEnabled = false;
                affaldskategori8.IsEnabled = false;
                affaldskategori9.IsEnabled = false;
            }
            else
            {
                
                canGraph.Children.Clear();
                affaldskategori1.IsEnabled = false;
                affaldskategori2.IsEnabled = false;
                affaldskategori3.IsEnabled = false;
                affaldskategori4.IsEnabled = false;
                affaldskategori5.IsEnabled = false;
                affaldskategori6.IsEnabled = false;
                affaldskategori7.IsEnabled = false;
                affaldskategori8.IsEnabled = false;
                affaldskategori9.IsEnabled = false;

                affaldskategori1.IsEnabled = true;
                affaldskategori3.IsEnabled = true;
                affaldskategori4.IsEnabled = true;
                affaldskategori8.IsEnabled = true;
                affaldskategori9.IsEnabled = true;
                DrawYAxis("Kg.", 1);
            }
            
        }

        private void Volumen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt32(VirksomhedsIDinput.Text);
                niceID = true;
            }
            catch
            {
                MessageBox.Show("Indtast venligst et gyldigt virksomhedsID i textboksen\ntil venstre for grafen.");
                niceID = false;
            }
            if (niceID != true)
            { MessageBox.Show(NaughtyID);
                affaldskategori1.IsEnabled = false;
                affaldskategori2.IsEnabled = false;
                affaldskategori3.IsEnabled = false;
                affaldskategori4.IsEnabled = false;
                affaldskategori5.IsEnabled = false;
                affaldskategori6.IsEnabled = false;
                affaldskategori7.IsEnabled = false;
                affaldskategori8.IsEnabled = false;
                affaldskategori9.IsEnabled = false;
            }
            else
            {
                
                canGraph.Children.Clear();
                affaldskategori1.IsEnabled = false;
                affaldskategori2.IsEnabled = false;
                affaldskategori3.IsEnabled = false;
                affaldskategori4.IsEnabled = false;
                affaldskategori5.IsEnabled = false;
                affaldskategori6.IsEnabled = false;
                affaldskategori7.IsEnabled = false;
                affaldskategori8.IsEnabled = false;
                affaldskategori9.IsEnabled = false;

                affaldskategori5.IsEnabled = true;
                affaldskategori6.IsEnabled = true;
                affaldskategori7.IsEnabled = true;
                DrawYAxis("Kubikmeter", 1);
                

            }
            
        }
        private void visBatteri_Click(object sender, RoutedEventArgs e)
        {
            kategori = "Batterier";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Batteri = new GraphLogic();
            Batteri.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Batteri, 1);
            maaleenhed1.IsEnabled = true;
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.Green ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Green;
        }
        private void visBil_Click(object sender, RoutedEventArgs e)
        {
            kategori = "Biler";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Bil = new GraphLogic();
            Bil.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Bil, 2);
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.Blue ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Blue;

        }
        private void visElektronik_Click(object sender, RoutedEventArgs e)
        {
            kategori = "Elektronikaffald";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Elektro = new GraphLogic();
            Elektro.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Elektro, 3);
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.Bisque ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Bisque;
        }

        private void visImpreg_Click(object sender, RoutedEventArgs e)
        {
            kategori = "ImprægneretTræ";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Woody = new GraphLogic();
            Woody.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Woody, 4);
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.BlueViolet ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.BlueViolet;
        }

        private void visInventar_Click(object sender, RoutedEventArgs e)
        {
            kategori = "Inventar";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Inventa = new GraphLogic();
            Inventa.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Inventa, 5);
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.Brown ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Brown;
        }

        private void visOrganisk_Click(object sender, RoutedEventArgs e)
        {
            kategori = "OrganiskAffald";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Anima = new GraphLogic();
            Anima.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Anima, 6);
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.BurlyWood ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.BurlyWood;
        }

        private void visPapPapir_Click(object sender, RoutedEventArgs e)
        {
            kategori = "Papogpapir";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Pappapir = new GraphLogic();
            Pappapir.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Pappapir, 7);
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.Coral ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Coral;
        }

        private void visPlast_Click(object sender, RoutedEventArgs e)
        {
            kategori = "Plastemballager";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Plastique = new GraphLogic();
            Plastique.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Plastique, 8);
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.CornflowerBlue ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.CornflowerBlue;
        }

        private void visPVC_Click(object sender, RoutedEventArgs e)
        {
            kategori = "PVC";
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Pvc = new GraphLogic();
            Pvc.GenerateDatapoints(kategori, Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Pvc, 9);
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.HotPink ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.HotPink;
        }

        //Test-handler :/
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Bob = new GraphLogic();
            Bob.GenerateDatapoints("Biler", Convert.ToInt32(VirksomhedsIDinput.Text));
            DrawGraph(Bob, 0);


        }


        private void DrawGraph(GraphLogic graphlogic, int dataset)
        {
            // Laver x-axis
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymin), new Point(canGraph.Width, ymin)));
            for (int i = canGraph.Children.Count - 1; i >= 0; i += -1)
            {
                UIElement Child = canGraph.Children[i];
                if (Child is TextBlock)
                    canGraph.Children.Remove(Child);
            }
            //For-loop til generering af punkter til streger på x-aksen
            for (double x = xmin; x <= canGraph.Width - step; x += step)
            {
                Text(x, Convert.ToDouble(ymin + 10), graphlogic.HoAxisLabel(xlabl), Color.FromRgb(255, 255, 255), 70);
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymin - margin / 2),
                    new Point(x, ymin + margin / 2)));
                xlabl++;
            }
            xlabl = 0;
            //Til slut tegnes der en sort linie baseret på ovenstående punkter
            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);
            //Så genereres først liniefarverne, dernæst tilføjes punkterne i et loop hvori at der også tegnes tilhørende streger
            Brush[] brushes =
                { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Bisque, Brushes.BlueViolet, Brushes.Brown, Brushes.BurlyWood, Brushes.Coral, Brushes.CornflowerBlue , Brushes.HotPink};

            int count = 0;
            PointCollection points = new PointCollection();
            for (double x = xmin; x + 20 < 31 * 20; x += step)
            {


                //Her bliver datapunkterne genereret




                points.Add(new Point(x, ymin - 2* graphlogic.GivePointValue(dataset, count)));
                count++;

            }
            count = 0;
            Polyline dataline = new Polyline();
            dataline.StrokeThickness = 1;
            dataline.Stroke = brushes[dataset];
            dataline.Points = points;

            canGraph.Children.Add(dataline);
        }

        //Text-metode til generering af tekst på Canvas-objektet. Håndterer også tekst til Akse-labels
        private void Text(double x, double y, string text, Color color, double angle)
        {

            TextBlock textBlock = new TextBlock();

            textBlock.Text = text;

            textBlock.Foreground = new SolidColorBrush(color);

            Canvas.SetLeft(textBlock, x);

            Canvas.SetTop(textBlock, y);

            canGraph.Children.Add(textBlock);
            // Rotate if desired.
            if (angle != 0)
                textBlock.LayoutTransform = new RotateTransform(angle);
        }
        private void Labelle(double x, double y, string text, Color color, double angle)
        {

            Label labl = new Label();

            labl.Content = text;
            
            labl.Foreground = new SolidColorBrush(color);

            Canvas.SetLeft(labl, x);

            Canvas.SetTop(labl, y);

            canGraph.Children.Add(labl);
            // Rotate if desired.
            if (angle != 0)
                labl.LayoutTransform = new RotateTransform(angle);
        }

        private void DrawYAxis(string yheader, int yaxismultiplier)
        {
            
            
            // Laver y-axis
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, 0), new Point(xmin, canGraph.Height)));

            //For-loop til generering af punkter til streger på y-aksen
            for (double y = ymax; y <= canGraph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(new Point(xmin - margin / 2, y), new Point(xmin + margin / 2, y)));
                Labelle(xmin - 3.5 * margin, ymin - y, ylabl.ToString(), Color.FromRgb(255, 255, 255), 0);
                ylabl += 1 * yaxismultiplier;
            }
            Text(xmin, ymax - 30, yheader, Color.FromRgb(255, 255, 255), 0);
            //Til slut tegnes der en sort linie baseret på ovenstående punkter
            Path yaxis_path = new Path();
            
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);

            ylabl = 0;
        }

        private void VirksomhedsIDinput_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox.Text == "Indast VirksomhedsID her...")
                txtBox.Text = string.Empty;
        }
    }
}
