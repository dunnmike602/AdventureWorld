using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace DiagramDesigner.Controls
{
    public partial class CompassEditor : ITypeEditor
    {
        private PropertyItem _item;

        public CompassEditor()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var editor = new CompassWindow(_item)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            editor.ShowDialog();
        }

        public FrameworkElement ResolveEditor(PropertyItem 
            propertyItem)
        {
            _item = propertyItem;

            return this;
        }
    }
}
