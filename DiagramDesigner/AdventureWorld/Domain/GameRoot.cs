using System.Collections.ObjectModel;
using AdventureLandCore.Domain;
using DiagramDesigner.AdventureWorld.CustomCollections;
using ReactiveUI;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public class GameRoot : ReactiveObjectBase
    {
        private ObservableItemCollection<Exit> _exits;
        private ObservableItemCollection<PlaceableObject> _placeableObjects;
        private ReactiveList<AdventureCommandMapping> _adventureCommandMappings;
        private ObservableItemCollection<Room> _rooms;
        private ObservableCollection<ScriptContainer> _scriptContainers;
        private Script _initialisationCode;
        private Script _gameLoopPreProcessCode;
        private Script _gameLoopPostProcessCode;
        private Script _commonCode;
        private ReactiveList<PlaceableObject> _placeableObjectsOnly;
        private ReactiveList<Container> _containersOnly;
        private ReactiveList<Npc> _npcsOnly;

        public ReactiveList<PlaceableObject> PlaceableObjectsOnly
        {
            get => _placeableObjectsOnly ?? (_placeableObjectsOnly = new ReactiveList<PlaceableObject>());
            set
            {
                _placeableObjectsOnly = value;
                OnPropertyChanged();
            }
        }

        public ReactiveList<Container> ContainersOnly
        {
            get => _containersOnly ?? (_containersOnly = new ReactiveList<Container>());
            set
            {
                _containersOnly = value;
                OnPropertyChanged();
            }
        }

        public ReactiveList<Npc> NpcsOnly
        {
            get => _npcsOnly ?? (_npcsOnly = new ReactiveList<Npc>());
            set
            {
                _npcsOnly = value;
                OnPropertyChanged();
            }
        }
        public ObservableItemCollection<Exit> Exits
        {
            get => _exits;
            set
            {
                _exits = value;
                OnPropertyChanged();
            }
        }

        public ObservableItemCollection<PlaceableObject> PlaceableObjects
        {
            get => _placeableObjects;
            set
            {
                _placeableObjects = value;
                OnPropertyChanged();
            }
        }

        public ReactiveList<AdventureCommandMapping> AdventureCommandMappings
        {
            get => _adventureCommandMappings;
            set
            {
                _adventureCommandMappings = value;
                OnPropertyChanged();
            }
        }

        public ObservableItemCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
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

        public Script InitialisationCode
        {
            get => _initialisationCode;
            set
            {
                _initialisationCode = value;
                OnPropertyChanged();
            }
        }

        public Script GameLoopPreProcessCode
        {
            get => _gameLoopPreProcessCode;
            set
            {
                _gameLoopPreProcessCode = value;
                OnPropertyChanged();
            }
        }

        public Script GameLoopPostProcessCode
        {
            get => _gameLoopPostProcessCode;
            set
            {
                _gameLoopPostProcessCode = value;
                OnPropertyChanged();
            }
        }

        public Script CommonCode
        {
            get => _commonCode;
            set
            {
                _commonCode = value;
                OnPropertyChanged();
            }
        }

        public string GameName => AdventureGameDesignerViewModel.Instance.GameName ?? "Adventure Game";
    }
}