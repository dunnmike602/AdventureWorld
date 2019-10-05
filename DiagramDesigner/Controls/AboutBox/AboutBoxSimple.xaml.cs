using System.Windows;

namespace DiagramDesigner.Controls.AboutBox
{
    public partial class AboutBoxSimple : Window
    {
        public AboutBoxSimple()
        {
            InitializeComponent();
        }

        public AboutBoxSimple(Window parent)
            : this()
        {
            this.Owner = parent;
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
