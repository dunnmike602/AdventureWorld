using System;
using System.Globalization;
using System.Windows.Data;
using DiagramDesigner.AdventureWorld.Domain;

namespace DiagramDesigner.Converters
{
    public class ObjectToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var adventureObject = value as AdventureObjectBase;

            return adventureObject?.ObjectBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}