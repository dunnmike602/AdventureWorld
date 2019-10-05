using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace SharedControls.Converters
{
    public class MultilineConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<string> lines)
            {
                return lines.First();
            }

            return string.Empty;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}