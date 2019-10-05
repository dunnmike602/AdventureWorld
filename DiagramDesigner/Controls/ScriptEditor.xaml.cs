using System.Windows;
using AdventureLandCore.Domain;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace DiagramDesigner.Controls
{
    public partial class ScriptEditor : ITypeEditor
    {
        PropertyItem _item;
        private Script _currentScript;

        public ScriptEditor()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            _currentScript = (Script)_item.Value;

            AdventureDesigner.Instance.ShowScriptDesigner(_currentScript);
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            _item = propertyItem;
            return this;
        }
  }
}
 