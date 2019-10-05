using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DiagramDesigner.Symbols;

namespace DiagramDesigner.Extensions
{
    internal static class DesignerItemExtensions
    {
        public static Rect GetBoundingRectangle(this IEnumerable<DesignerItem> items)
        {
            var x1 = double.MaxValue;
            var y1 = double.MaxValue;
            var x2 = double.MinValue;
            var y2 = double.MinValue;

            foreach (var item in items)
            {
                x1 = Math.Min(Canvas.GetLeft(item), x1);
                y1 = Math.Min(Canvas.GetTop(item), y1);

                x2 = Math.Max(Canvas.GetLeft(item) + item.Width, x2);
                y2 = Math.Max(Canvas.GetTop(item) + item.Height, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }

    }
}