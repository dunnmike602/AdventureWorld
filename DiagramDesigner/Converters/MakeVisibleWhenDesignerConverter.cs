using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AdventureLandCore.Extensions;

namespace DiagramDesigner.Converters
{
    public class MakeVisibleWhenDesignerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = value?.ToString();

            return name.IsEqualToAny("RoomDesigner", "ObjectDesigner") ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}