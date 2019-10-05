using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DiagramDesigner.AdventureWorld.Domain;

namespace DiagramDesigner.Converters
{
    public class ContainerToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.GetType() == typeof(Container))
            {
                return (System.Windows.Media.LinearGradientBrush) Application.Current.Resources["ItemBrushForContainer"];
            }

            if (value?.GetType() == typeof(Npc))
            {
                return (System.Windows.Media.LinearGradientBrush) Application.Current.Resources["ItemBrushForNpc"];
            }

            return (System.Windows.Media.LinearGradientBrush)Application.Current.Resources["ItemBrushForObject"];
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}