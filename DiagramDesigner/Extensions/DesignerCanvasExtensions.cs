using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using DiagramDesigner.Helpers;
using DiagramDesigner.Symbols;
using DiagramDesigner.ToolboxImpl;

namespace DiagramDesigner.Extensions
{
    public static class DesignerCanvasExtensions
    {
        private static object GetXamlFromStencil(string gridName)
        {
            var stencils = (Toolbox)Application.Current.FindResource("FlowChartStencils");

            var roomStencil = stencils.Items.Cast<Grid>().First(grid => grid.Name == gridName);

            return XamlReader.Load(XmlReader.Create(new StringReader(XamlWriter.Save(roomStencil))));
        }

        public static DesignerItem AddStencilItem(this DesignerCanvas source, string gridName)
        {
            var newItem = new DesignerItem
            {
                Content = GetXamlFromStencil(gridName),
                Width = GlobalUiConstants.TemplateItemWidth,
                Height = GlobalUiConstants.TemplateItemHeight
            };

            Canvas.SetLeft(newItem, 0);
            Canvas.SetTop(newItem, 0);

            Panel.SetZIndex(newItem, 0);
            source.Children.Add(newItem);
            DesignerCanvas.SetConnectorDecoratorTemplate(newItem);

            return newItem;
        }
    }
}