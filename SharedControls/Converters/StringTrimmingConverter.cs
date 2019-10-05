using System;
using System.Globalization;
using System.Windows.Data;

namespace SharedControls.Converters
{
    [ValueConversion(typeof(string), typeof(string))] 
    public class StringTrimmingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rawValue = value;

            if (rawValue is string)
            {
                rawValue = rawValue.ToString().Replace("\r", " ").Replace("\n", string.Empty);
            }

            return rawValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}