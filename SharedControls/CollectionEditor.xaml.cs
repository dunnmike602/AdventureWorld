using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace SharedControls
{
    public partial class CollectionEditor : ITypeEditor
    {
        PropertyItem _item;
        
        public IEnumerable<string> Values
        {
            get { return (IEnumerable<string>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(IEnumerable<string>), typeof(CollectionEditor), new PropertyMetadata(null));

        public CollectionEditor()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<string> value;
            bool isReadOnly;
            string propertyName;

            // Dialog can be triggered 2 different ways by the property grid.
            if (_item == null)
            {
                value = Values;
                isReadOnly = false;
                propertyName = "Descriptions";
            }
            else
            {
                value = (IEnumerable<string>) _item.Value;
                isReadOnly = _item.IsReadOnly;
                propertyName = _item.PropertyName;
            }

            var editor = new MultiTextEditor(value, isReadOnly, propertyName)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

            editor.ShowDialog();

            if (!editor.OkPressed)
            {
                return;
            }

            if (_item == null)
            {
                Values = Enumerable.ToArray<string>(editor.TextValues);
            }
            else
            {
                _item.Value = editor.TextValues;
            }
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            _item = propertyItem;
            return this;
        }
    }
}
