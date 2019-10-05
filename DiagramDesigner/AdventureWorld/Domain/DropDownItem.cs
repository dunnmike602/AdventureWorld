using AdventureLandCore.Domain;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public class DropDownItem : ReactiveObjectBase
    {
        private bool _isChecked;
        private string _label;

        private string _toolTip;

        public string ToolTip
        {
            get => _toolTip;
            set
            {
                _toolTip = value;
                OnPropertyChanged();
            }
        }
        
        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
    }
}