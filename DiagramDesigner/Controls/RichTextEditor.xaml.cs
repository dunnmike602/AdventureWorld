using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace DiagramDesigner.Controls
{
    public partial class RichTextEditor
    {
        private readonly PropertyItem _item;
        public string Text { get; set; }

        public RichTextEditor()
        {
            InitializeComponent();
        }

        public RichTextEditor(string text) : this()
        {
            Text = text;

            MainText.Text = Text;
            MainText.IsReadOnly = false;

            Title = "Edit Text";
        }

        public RichTextEditor(PropertyItem item) : this()
        {
            _item = item;
            MainText.Text = (string) _item.Value;
            MainText.IsReadOnly = _item.IsReadOnly;

            if (_item.IsReadOnly)
            {
                CancelButton.Visibility = Visibility.Collapsed;
                Title = "Browse Text";
            }
            else
            {
                Title = "Edit Text";
            }
        }
      
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if (_item != null)
            {
                _item.Value = MainText.Text;
            }

            Text = MainText.Text;
  
            Close();
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            MainText.Focus();
            MainText.CaretIndex = MainText.Text.Length;
        }
    }
}
