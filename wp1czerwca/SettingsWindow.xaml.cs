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
using System.Windows.Shapes;

namespace wp1czerwca
{
    [Serializable]
    public class Settings
    {
        public GameMode Mode { get; set; }
        public Uri Tiger { get; set; }
        public Uri Bull { get; set; }

        public Settings() { }
        public Settings(GameMode mode,Uri tiger, Uri bull)
        {
            Mode = mode;
            Tiger = tiger;
            Bull = bull;
        }
    }
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        
        public static Settings StaticSettings { get; set; }

        public Settings Settings
        {
            get { return StaticSettings; }
            set { StaticSettings = value; }
        }
        public SettingsWindow(GameMode mode)
        {
            InitializeComponent();
            if (StaticSettings == null)
                StaticSettings = new Settings(mode,MainWindow.Tigers[0],MainWindow.Bulls[0]);

            this.DataContext = this;
            SettingsGrid.DataContext = Settings;
        }

        private void OK_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Previous_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Next_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
