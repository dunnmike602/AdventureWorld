using System.ComponentModel;
using System.Windows;

namespace SharedControls
{
    /// <summary>
    /// Interaction logic for TextView.xaml
    /// </summary>
    public partial class TextView :  INotifyPropertyChanged

    {
        public TextView()
        {
            InitializeComponent();
        }

        public bool ShowCloseButton { get; set; }
        private string _labelText;

        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                OnPropertyChanged("LabelText");
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        public string Text
        {
            get => TextInputData.Text;
            set { TextInputData.Text = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void TextViewOnLoaded(object sender, RoutedEventArgs e)
        {
            TextInputData.Focus();
        }
    }
}
