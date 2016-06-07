using System;
using System.Configuration;
using System.Globalization;
using System.Windows.Data;

namespace wp1czerwca
{
    public class GameModeToRadioButtonConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            if ((SettingsWindow.StaticSettings.Mode == GameMode.Computer && parameter.ToString().Equals("pvcOption")) ||
                (SettingsWindow.StaticSettings.Mode == GameMode.Player && parameter.ToString().Equals("pvpOption")))
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return SettingsWindow.StaticSettings.Mode;

            if ((bool)value == false ||(SettingsWindow.StaticSettings.Mode == GameMode.Computer && parameter.ToString().Equals("pvcOption")) ||
                (SettingsWindow.StaticSettings.Mode == GameMode.Player && parameter.ToString().Equals("pvpOption")))
                return SettingsWindow.StaticSettings.Mode;
            

            return parameter.ToString().Equals("pvcOption") ? GameMode.Computer : GameMode.Player;
        }
    }
}