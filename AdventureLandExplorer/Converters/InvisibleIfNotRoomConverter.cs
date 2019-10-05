using System;
using System.Globalization;
using System.Windows.Data;
using AdventureLandCore.Domain;

namespace AdventureLandExplorer.Converters
{
    public class InvisibleIfNotRoomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Room;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}