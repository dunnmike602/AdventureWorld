using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SharedControls.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter == null)
            {
                return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value != null && parameter.ToString().ToUpper() == "INVERT")
            {
                return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new Exception("The method or operation is not implemented.");
        }
    }
}