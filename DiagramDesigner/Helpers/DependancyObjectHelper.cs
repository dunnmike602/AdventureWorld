using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using DiagramDesigner.Symbols;

namespace DiagramDesigner.Helpers
{
    internal static class DependancyObjectHelper
    {
        public static void GetConnectors(DependencyObject parent, ICollection<Connector> connectors)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Connector)
                {
                    connectors.Add(child as Connector);
                }
                else
                {
                    GetConnectors(child, connectors);
                }
            }
        }

    }
}