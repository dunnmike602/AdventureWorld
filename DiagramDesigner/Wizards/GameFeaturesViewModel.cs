using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public class GameFeaturesViewModel : ValidatableViewModelBase
    {
        private bool _enableScore;
        private bool _enableShowItemsInRoom;
        private bool _enableInventorySize;
        private bool _enableExitDescriptions;
        private bool _enablePlayerLost;
        private bool _enableTitles;

        public bool EnableScore
        {
            get => _enableScore;
            set => this.RaiseAndSetIfChanged(ref _enableScore, value);
        }

        public bool EnableShowItemsInRoom
        {
            get => _enableShowItemsInRoom;
            set => this.RaiseAndSetIfChanged(ref _enableShowItemsInRoom, value);
        }

        public bool EnableInventorySize
        {
            get => _enableInventorySize;
            set => this.RaiseAndSetIfChanged(ref _enableInventorySize, value);
        }

        public bool EnableExitDescriptions
        {
            get => _enableExitDescriptions;
            set => this.RaiseAndSetIfChanged(ref _enableExitDescriptions, value);
        }

        public bool EnablePlayerLost
        {
            get => _enablePlayerLost;
            set => this.RaiseAndSetIfChanged(ref _enablePlayerLost, value);
        }

        public bool EnableTitles
        {
            get => _enableTitles;
            set => this.RaiseAndSetIfChanged(ref _enableTitles, value);
        }

        public GameFeaturesViewModel()
        {
            EnableScore = true;
            EnableShowItemsInRoom = true;
            EnableInventorySize = true;
            EnableExitDescriptions = false;
            EnablePlayerLost = true;
            EnableTitles = true;
        }
    }
}