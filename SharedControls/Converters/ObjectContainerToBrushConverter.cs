using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SharedControls.Converters
{
    public class ObjectContainerToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool) value)
            {
                return (System.Windows.Media.LinearGradientBrush)Application.Current.Resources["ItemBrushForContainer"];
            }

            return (System.Windows.Media.LinearGradientBrush)Application.Current.Resources["ItemBrushForObject"];
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}