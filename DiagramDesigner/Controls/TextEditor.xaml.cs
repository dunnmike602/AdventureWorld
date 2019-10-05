using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace DiagramDesigner.Controls
{
    /// <summary>
    /// Interaction logic for CollectionEditor.xaml
    /// </summary>
    public partial class TextEditor : ITypeEditor
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.
          Register("Value", typeof(string), typeof(TextEditor),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.
              BindsTwoWayByDefault));

        PropertyItem _item;

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set { SetValue(ValueProperty, value); }
        }       

        public TextEditor()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var editor = new RichTextEditor(_item)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

            editor.ShowDialog();
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            var binding = new Binding("Value")
            {
                Source = propertyItem,
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(this, ValueProperty, binding);

            _item = propertyItem;
            return this;
        }
    }
}
