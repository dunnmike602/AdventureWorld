using System.Windows;
using AdventureLandCore.Domain;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace DiagramDesigner.Controls
{
    public partial class ConversationScriptEditor : ITypeEditor
    {
        PropertyItem _item;
        private Script _currentScript;

        public ConversationScriptEditor()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            _currentScript = (Script)_item.Value;

            ConversationDesigner.ConversationDesigner.Instance.ShowScriptDesigner(_currentScript);
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            _item = propertyItem;
            return this;
        }
  }
}
 