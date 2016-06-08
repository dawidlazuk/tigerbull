using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace wp1czerwca
{
    public class UriToBitmapConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage result = new BitmapImage();
            result.BeginInit();
            result.UriSource = (Uri) value;
            result.EndInit();
            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}