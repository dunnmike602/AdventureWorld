using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Syncfusion.Windows.Tools.Controls;

namespace SharedControls
{
    public partial class MultiTextEditor
    {
        public  ObservableCollection<string> TextValues { get; set; }

        public bool OkPressed { get; set; }

        public ObservableCollection<UserControl> DescriptionView { get; set; } = new ObservableCollection
            <UserControl>();

        public MultiTextEditor()
        {
            InitializeComponent();

            OkPressed = false;
        }

        public MultiTextEditor(IEnumerable<string> textValues, bool isReadOnly, string propertyName) : this()
        {
            TextValues = new ObservableCollection<string>(textValues);

            DescriptionView.Clear();

            if (TextValues.Count == 0)
            {
                LoadData(string.Empty);
            }
            else
            {
                foreach (var textItem in TextValues)
                {
                    LoadData(textItem);
                }
            }

            TextTabControl.DataContext = DescriptionView;
            TextTabControl.SelectedIndex = 0;

            if (isReadOnly)
            {
                CancelButton.Visibility = Visibility.Collapsed;
                AddButton.Visibility = Visibility.Collapsed;
                Title = $"Browse {propertyName}";
            }
            else
            {
                Title = $"Edit {propertyName}";
            }
        }

        private void LoadData(string text)
        {
            var newView = new TextView
            {
                ShowCloseButton = true,
                Text = text,
            };

            DescriptionView.Add(newView);
        }

        private void UpdateHeaders()
        {
            for (var index = 0; index < DescriptionView.Count; index++)
            {
                var view = (TextView)DescriptionView[index];
                view.LabelText = "Description " + index;
            }

            TextTabControl.DataContext = DescriptionView;
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            TextTabControl.SelectedIndex = 0;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            LoadData(string.Empty);
            UpdateHeaders();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            TextValues.Clear();

            foreach (var textView in DescriptionView.Cast<TextView>())
            {
                TextValues.Add(textView.Text);
            }

            OkPressed = true;

            Close();
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateHeaders();
        }

        private void TextTabControlOnTabClosed(object sender, CloseTabEventArgs e)
        {
            DescriptionView.Remove((UserControl)e.TargetTabItem.DataContext);

            UpdateHeaders();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            DescriptionView.RemoveAt(TextTabControl.SelectedIndex);

            UpdateHeaders();
        }
    }
}
