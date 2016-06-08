using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using System.Windows.Threading;

namespace wp1czerwca
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private static readonly string SettingsFileName = "settings2.bin";
        public static List<Uri> Tigers;
        public static List<Uri> Bulls;
        private Game Game { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            InitializeAnimalCollections();
            
            Game = new Game(this,LoadSettings());

            Loaded += DrawGrid;
            Game.InitGame();

           
        }

        private void InitializeAnimalCollections()
        {
            Tigers = new List<Uri>();
            Bulls = new List<Uri>();
            var requiredPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(
                                System.IO.Path.GetDirectoryName(
                                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)));
            foreach (var file in Directory.GetFiles(new Uri(requiredPath + @"\obrazki\Tigers").LocalPath))
                Tigers.Add(new Uri(file));
            foreach (var file in Directory.GetFiles(new Uri(requiredPath + @"\obrazki\Bulls").LocalPath))
                Bulls.Add(new Uri(file));

            // Tigers.Add(new Uri("/obrazki/Tigers/tygrys.png",UriKind.Relative));
           // Tigers.Add(new Uri("C/obrazki/Tigers/tygrys2.png",UriKind.Relative));
           // Bulls.Add(new Uri("/obrazki/Bulls/byk.png",UriKind.Relative));
           // Bulls.Add(new Uri("/obrazki/Bulls/byk2.png",UriKind.Relative));
        }

        private void DrawGrid(object sender, EventArgs e)
        {
           
            double CellHeight = GameGrid.ActualHeight/5;
            double CellWidth = GameGrid.ActualWidth/4;
            Cell.CellSize = new Size(CellWidth, CellHeight);
            Line line;
            for (int i = 1; i < 4; ++i)
            {
                line = new Line
                {
                    X1 = CellWidth*i,
                    X2 = CellWidth*i,
                    Y1 = 0,
                    Y2 = GameGrid.ActualHeight,
                    Stroke = Brushes.GreenYellow,
                    StrokeThickness = 3
                };
                Canvas.Children.Add(line);
            }
            for (int i = 1; i < 5; ++i)
            {
                line = new Line
                {
                    X1 = 0,
                    X2 = GameGrid.ActualWidth,
                    Y1 = CellHeight*i,
                    Y2 = CellHeight*i,
                    Stroke = Brushes.GreenYellow,
                    StrokeThickness = 3
                };
                Canvas.Children.Add(line);
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.Children.Clear();
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => DrawGrid(sender,e)));
        }

        public void ShowBoard()
        {
            for(int i = 0; i < 5; ++i)
                for (int j = 0; j < 4; ++j)
                    GameGrid.Children.Add(Game.Board[i, j].image);
        }

        private void NewGame_OnClick(object sender, RoutedEventArgs e)
        {
            Game.InitGame();
        }

        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow(Game.Settings);
            settings.ShowDialog();
            if(Game.Settings.Mode == GameMode.Computer && Game.Round==Animal.Bull)
                Game.MakeComputerMove();
            Game.SetAnimalImages();
            SaveSettings();
        }

        private void SaveSettings()
        {
            using (FileStream fs = new FileStream(SettingsFileName, FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs,Game.Settings);
            }
        }

        private Settings LoadSettings()
        {
            Settings result = null;
            try
            {
                using (FileStream fs = new FileStream(SettingsFileName, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    result = (Settings) bf.Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                result = new Settings();
            }
            catch (SerializationException)
            {
                result = new Settings();
            }
            return result;
        }

    }
}
