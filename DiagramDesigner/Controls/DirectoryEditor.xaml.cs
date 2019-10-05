using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Binding = System.Windows.Data.Binding;

namespace DiagramDesigner.Controls
{
    public partial class DirectoryEditor : ITypeEditor
    {
        public class OldWindow : IWin32Window
        {
            readonly IntPtr _handle;

            public OldWindow(IntPtr handle)
            {
                _handle = handle;
            }

            #region IWin32Window Members

            IntPtr IWin32Window.Handle => _handle;

            #endregion
        } 


        public static readonly DependencyProperty ValueProperty = DependencyProperty.
          Register("Value", typeof(string), typeof(FileEditor),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.
              BindsTwoWayByDefault));

        private PropertyItem _item;

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set { SetValue(ValueProperty, value); }
        }       

        public DirectoryEditor()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            using (var browseFolder = new FolderBrowserDialog())
            {
                browseFolder.RootFolder = Environment.SpecialFolder.Desktop;
                var path = _item.Value?.ToString();
                browseFolder.SelectedPath = path;

                 var result = browseFolder.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _item.Value = browseFolder.SelectedPath;
                }
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
