using AdventureLandCore.Domain;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public class ScriptContainer : ReactiveObjectBase
    {
        private string _name;
        private Script _script;
        private ScriptTypes _type;
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public ScriptTypes Type        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public Script Script
        {
            get => _script;
            set
            {
                _script = value;
                OnPropertyChanged();
            }
        }
    }
}