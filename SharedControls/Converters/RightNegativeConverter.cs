using System.Windows;
using System.Windows.Data;

namespace SharedControls.Converters
{
    public class RightNegativeConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness(0, 0, -System.Convert.ToDouble(value) / 2, 0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}