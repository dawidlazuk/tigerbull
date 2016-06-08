using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using wp1czerwca.Annotations;

namespace wp1czerwca
{
    [Serializable]
    public class Settings
    {
        private Uri tiger;
        private Uri bull;
        public GameMode Mode { get; set; }

        public Uri Tiger { get; set; }

        public Uri Bull { get; set; }

        public Settings(GameMode mode = GameMode.Computer,int tiger = 0, int bull = 0)
        {
            Mode = mode;
            this.Tiger = MainWindow.Tigers[tiger];
            this.Bull = MainWindow.Bulls[bull];
        }
    }
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        
        public static Settings StaticSettings { get; set; }
        public static int tigerInd;
        public static int bullInd;
        public Settings Settings
        {
            get { return StaticSettings; }
            set { StaticSettings = value; }
        }
        public SettingsWindow(Settings gameSettings)
        {
            InitializeComponent();
            if (StaticSettings == null)
            {
                tigerInd = MainWindow.Tigers.FindIndex(u => gameSettings.Tiger == u);
                bullInd = MainWindow.Bulls.FindIndex(u => gameSettings.Bull == u);
                StaticSettings = gameSettings;
            }
            this.DataContext = this;
            SettingsGrid.DataContext = Settings;
        }

        private void OK_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PreviousTiger_OnClick(object sender, RoutedEventArgs e)
        {
            tigerInd = (tigerInd - 1)%MainWindow.Tigers.Count;
            if (tigerInd < 0)
                tigerInd += MainWindow.Tigers.Count;
            Settings.Tiger = MainWindow.Tigers[tigerInd];
            ResetSettingsContext();
        }

        private void NextTiger_OnClick(object sender, RoutedEventArgs e)
        {
            tigerInd = (tigerInd + 1) % MainWindow.Tigers.Count;
            Settings.Tiger = MainWindow.Tigers[tigerInd];
            ResetSettingsContext();
        }


        private void PreviousBull_OnClick(object sender, RoutedEventArgs e)
        {
            bullInd = (bullInd - 1) % MainWindow.Bulls.Count;
            if (bullInd < 0)
                bullInd += MainWindow.Bulls.Count;
            Settings.Bull = MainWindow.Bulls[bullInd];
            ResetSettingsContext();
        }

        private void NextBull_OnClick(object sender, RoutedEventArgs e)
        {
            bullInd = (bullInd + 1) % MainWindow.Bulls.Count;
            Settings.Bull = MainWindow.Bulls[bullInd];
            ResetSettingsContext();
        }

        private void ResetSettingsContext()
        {
            SettingsGrid.DataContext = null;
            SettingsGrid.DataContext = Settings;
        }
    }
}
