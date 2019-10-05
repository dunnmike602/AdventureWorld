﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DiagramDesigner.Symbols;
using DiagramDesigner.Symbols.Helpers;

namespace DiagramDesigner.Converters
{
    public class MakeVisibleWhenConversationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DesignerItem item && item.StencilObjectType == ObjectType.ConversationStart
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}