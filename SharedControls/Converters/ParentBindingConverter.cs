using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AdventureLandCore.Domain;

namespace SharedControls.Converters
{
    public class ParentBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
          return value is Room ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}