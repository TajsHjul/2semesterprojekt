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
    public partial class Graph : UserControl
    {
        public Graph()
        {
            InitializeComponent();
        }
        private void stk_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void Colli_click(object sender, RoutedEventArgs e)
        {
            affaldskategori1.IsEnabled = false;
            affaldskategori2.IsEnabled = false;
            affaldskategori3.IsEnabled = false;
            affaldskategori4.IsEnabled = false;
            affaldskategori5.IsEnabled = false;
            affaldskategori6.IsEnabled = false;
            affaldskategori7.IsEnabled = false;
            affaldskategori8.IsEnabled = false;
            affaldskategori9.IsEnabled = false;

            ImageBrush hmm = new ImageBrush();
            hmm.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Icons/hmm.png", UriKind.Absolute));
            canGraph.Background = hmm;

        }
        private void Thicc_Click(object sender, RoutedEventArgs e)
        {
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

        private void Volumen_Click(object sender, RoutedEventArgs e)
        {
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

        private void visBil_Click(object sender, RoutedEventArgs e)
        {

        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            //Der instantieres et nyt GraphLogic object kaldet Bob, og der genereres datapoints
            GraphLogic Bob = new GraphLogic();
            Bob.GenerateDatapoints();

            // definerer max-værdier, margin og margin mellem punkterne
            const double margin = 10;
            double xmin = margin;
            double xmax = canGraph.Width - margin;
            double ymax = margin;
            double ymin = canGraph.Height - margin;
            // definerer step constant som bruges til at definere afstanden mellem punkter 
            const double step = 20;

            //ylabl og xlabl -- skal egenlig hentes fra Graphlogic-classen
            int xlabl = 0;
            int ylabl = 0;

            // Laver x-axis
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymin), new Point(canGraph.Width, ymin)));
            //For-loop til generering af punkter til streger på x-aksen
            for (double x = xmin; x <= canGraph.Width - step; x += step)
            {
                Text(x, Convert.ToDouble(ymin + 10), Bob.HoAxisLabel(xlabl), Color.FromRgb(255, 255, 255), 70);
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymin - margin / 2),
                    new Point(x, ymin + margin / 2)));
                xlabl++;
            }
            //Til slut tegnes der en sort linie baseret på ovenstående punkter
            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Laver y-axis
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, 0), new Point(xmin, canGraph.Height)));

            //For-loop til generering af punkter til streger på y-aksen
            for (double y = ymax; y <= canGraph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(new Point(xmin - margin / 2, y), new Point(xmin + margin / 2, y)));
                Text(xmin - 3.5 * margin, ymin - y, ylabl.ToString(), Color.FromRgb(255, 255, 255), 0);
                ylabl += 100;
            }
            //Til slut tegnes der en sort linie baseret på ovenstående punkter
            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);


            //Så genereres først liniefarverne, dernæst tilføjes punkterne i et loop hvori at der også tegnes tilhørende streger
            Brush[] brushes =
                { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Bisque, Brushes.BlueViolet, Brushes.Brown, Brushes.BurlyWood, Brushes.Coral, Brushes.CornflowerBlue };

            Random rand = new Random();
            for (int dataset = 8; dataset < 9; dataset++)
            {
               
                int start_y = (int)ymin;

                int last_y = rand.Next((int)ymax, (int)ymin);
                PointCollection points = new PointCollection();
                for (double x = xmin; x <= 200; x += step)
                {


                    //Her bliver datapunkterne genereret


                    last_y = rand.Next(last_y - 10, last_y + 10);

                    points.Add(new Point(x, Bob.GivePointValue(dataset, x/20)));


                }

                Polyline dataline = new Polyline();
                dataline.StrokeThickness = 1;
                dataline.Stroke = brushes[dataset];
                dataline.Points = points;

                canGraph.Children.Add(dataline);
            }
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

        
    }
}
