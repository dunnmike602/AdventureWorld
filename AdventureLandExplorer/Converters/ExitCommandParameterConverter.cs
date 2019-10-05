using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using AdventureLandCore.Domain;
using AdventureLandExplorer.AdventureLandWpf;

namespace AdventureLandExplorer.Converters
{
    public class ExitCommandParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var exits = (IEnumerable<Exit>)value;

            // ReSharper disable once PossibleNullReferenceException
            var direction = System.Convert.ToInt32(parameter);

            var directionMapping = AdventureLandWpfProgram.GameConfiguration.DirectionMappings[direction];

            return exits.FirstOrDefault(exit => exit.Direction == directionMapping.Text);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}