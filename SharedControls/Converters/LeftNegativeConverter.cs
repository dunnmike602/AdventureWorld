using System.Windows;
using System.Windows.Data;

namespace SharedControls.Converters
{
    public class LeftNegativeConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness(-System.Convert.ToDouble(value) / 2, 0, 0, 0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}