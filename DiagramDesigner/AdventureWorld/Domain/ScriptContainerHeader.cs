using System.Collections.ObjectModel;
using AdventureLandCore.Domain;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public class ScriptContainerHeader : ReactiveObjectBase
    {
        private string _image;
        private string _header;
        private ScriptTypes _type;
        private ObservableCollection<ScriptContainer> _scriptContainers = new ObservableCollection<ScriptContainer>();

        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }

        public ScriptTypes Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ScriptContainer> ScriptContainers
        {
            get => _scriptContainers;
            set
            {
                _scriptContainers = value;
                OnPropertyChanged();
            }
        }
    }
}