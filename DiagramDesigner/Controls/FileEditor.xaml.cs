using System.IO;
using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Binding = System.Windows.Data.Binding;
using Microsoft.Win32;

namespace DiagramDesigner.Controls
{
    public partial class FileEditor : ITypeEditor
    {
      public static readonly DependencyProperty ValueProperty = DependencyProperty.
          Register("Value", typeof(string), typeof(FileEditor),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.
              BindsTwoWayByDefault));

        private PropertyItem _item;

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }       

        public FileEditor()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog
            {
                Filter = @"Executable Files (*.exe)|*.exe",
            };
          
            var path = _item.Value?.ToString();

            if (!string.IsNullOrWhiteSpace(path))
            {
                openFile.InitialDirectory = Path.GetDirectoryName(path);
            }

            if (openFile.ShowDialog() == true)
            {
                _item.Value = openFile.FileName;
            }
        }

        public FrameworkElement ResolveEditor(PropertyItem 
            propertyItem)
        {
            _item = propertyItem;

            var binding = new Binding("Value")
                {
                    Source = propertyItem,
                    Mode = BindingMode.OneWay
                };

            BindingOperations.SetBinding(this, ValueProperty, binding);

            return this;
        }
    }
}
