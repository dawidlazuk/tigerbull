using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace wp1czerwca
{
    public class AnimalToImageConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage result = new BitmapImage();
            Uri uri = null;
            result.BeginInit();
            switch ((Animal)value)
            {
                case Animal.Bull:
                    uri = Game.StaticSettings.Bull;
                        //new Uri(
                          //  @"C:\Users\Dawid\Documents\Visual Studio 2013\Projects\wp1czerwca\wp1czerwca\obrazki\byk.png");
                    break;
                case Animal.Tiger:
                    uri = Game.StaticSettings.Tiger;
                        //new Uri(
                          //  @"C:\Users\Dawid\Documents\Visual Studio 2013\Projects\wp1czerwca\wp1czerwca\obrazki\tygrys.png");
                    break;
            }
            result.UriSource = uri;
            result.EndInit();
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}