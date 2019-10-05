using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiagramDesigner.Controls
{
    public class  TextOnlyTextBox : TextBox
    {
        public TextOnlyTextBox()
        {
            this.PreviewKeyDown += TextOnlyTextBoxPreviewKeyDown;
        }

        private void TextOnlyTextBoxPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           var allowableKeys = Enumerable.Range(44, 26).Select(i => (Key) i).Union(new List<Key>{Key.Delete, Key.Back, Key.Tab, Key.Left, Key.Right}).ToList();

            if (!allowableKeys.Contains(e.Key))
            {
                e.Handled = true;
            }
        }
    }
}