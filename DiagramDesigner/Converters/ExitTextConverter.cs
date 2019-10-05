﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DiagramDesigner.AdventureWorld.Domain;

namespace DiagramDesigner.Converters
{
    public class ExitTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var exits = (IEnumerable<Exit>) value;
            
            // ReSharper disable once PossibleNullReferenceException
            var direction = System.Convert.ToInt32(parameter);

            var directionMapping = AdventureGameDesignerViewModel.Instance.DirectionMappings[direction];

            var roomText = $" to {exits.FirstOrDefault(exit => exit.Direction == directionMapping.Text)?.ToRoom.BaseName}";

            return directionMapping.Abbreviation + roomText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}