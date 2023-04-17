using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WPF_LoginForm.Converters;
namespace WPF_LoginForm.Converters
{

    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var imagePath = "";
            if (value is Uri uri)
            {
                imagePath = uri.ToString();
            }
            else if (value is string stringValue)
            {
                imagePath = stringValue;
            }

            var bitmapImage = new BitmapImage();
            // 设置BaseUri属性以帮助解析相对路径
            bitmapImage.BaseUri = new Uri("pack://application:,,,/");

            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imagePath, UriKind.Relative);//RelativeOrAbsolute
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class StringToImageConverter : IValueConverter
    //{
    //     // public string StringToImageConverterX;
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null) return null;

    //        var imagePath = (string)value;
    //        var bitmapImage = new BitmapImage();

    //        bitmapImage.BeginInit();
    //        bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
    //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
    //        bitmapImage.EndInit();

    //        return bitmapImage;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}

}
