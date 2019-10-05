using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AdventureLandCore.Extensions;
using DiagramDesigner.AdventureLandWpf;
using ReactiveUI;
using AdventureObjectBase = AdventureLandCore.Domain.AdventureObjectBase;
using Room = AdventureLandCore.Domain.Room;

namespace AdventureLandWpf.Controls
{
    public partial class GameExplorer : INotifyPropertyChanged
    {
        private Room _currentLocation;
        private bool _trackCurrentRoom = true;
        private AdventureLandWpfProgram _gameExplorerProgam;
        private AdventureObjectBase _selectedAdventureObject;
        private string _consoleText;

        public static RoutedCommand ClearAllCommand = new RoutedCommand();
        private ReactiveList<AdventureLandCore.Domain.PlaceableObject> _currentRoomObjects;

        public ReactiveList<AdventureLandCore.Domain.PlaceableObject> CurrentRoomObjects
        {
            get => _currentRoomObjects;
            set
            {
                _currentRoomObjects = value;
                OnPropertyChanged();
            }
        }

        public AdventureObjectBase SelectedAdventureObject
        {
            get => _selectedAdventureObject;
            set
            {
                _selectedAdventureObject = value;

                OnPropertyChanged();
            }
        }

        public string ConsoleText
        {
            get => _consoleText;
            set
            {
                _consoleText = value;
                OnPropertyChanged();
            }
        }

        public Room CurrentLocation
        {
            get => _currentLocation;
            set
            {
                _currentLocation = value;
                OnPropertyChanged();
            }
        }

        public bool TrackCurrentRoom
        {
            get => _trackCurrentRoom;
            set
            {
                _trackCurrentRoom = value;

                if (_trackCurrentRoom)
                {
                    CurrentLocation = _gameExplorerProgam?.AdventureGameEngine.Location;
                }

                OnPropertyChanged();
            }
        }
        
        public GameExplorer()
        {
            InitializeComponent();

            DataContext = this;

            SetupCommands();
        }

        private void SetupCommands()
        {
            CommandBindings.Add(new CommandBinding(ClearAllCommand, ClearAllExecuted));
        }

        private void ClearAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ClearConsoleText();
        }

        private async void RunOnClick(object sender, RoutedEventArgs e)
        {
           await Task.Run(() => { RunTheGame(); });
        }

        private void ResetGame()
        {
            ClearConsoleText();

            CurrentRoomObjects.Clear();

            SelectedAdventureObject = null;

            CurrentLocation = null;

            Title.Text = string.Empty;
            Console.Text = string.Empty;
        }

        private void RunTheGame()
        {
            _gameExplorerProgam = new AdventureLandWpfProgram();

            ClearConsoleText();

       //     _gameExplorerProgam.Initialise(AdventureGameDesignerApi.Instance.Map(), Title, Console, Input, Prompt);
            _gameExplorerProgam.AdventureGameEngine.StateChanged += AdventureGameEngineStateChanged;

            _gameExplorerProgam.InitialiseGameCore();

            _gameExplorerProgam.Execute();
        }

        private void ClearConsoleText()
        {
            ConsoleText = null;
        }

        private void AdventureGameEngineStateChanged(object sender, AdventureLandCore.Domain.GameStateEventArgs e)
        {
            if (e.CurrentLocation != null && TrackCurrentRoom)
            {
                Dispatcher.Invoke(() =>
                {
                    CurrentLocation = e.CurrentLocation;
                    SetCurrentLocationObjects();
                    SelectedAdventureObject = CurrentLocation;
                });
            }

            Dispatcher.Invoke(() =>
            {
                ConsoleText += e.ToString().AddLineBreaks(2);
            });
        }

        private void SetCurrentLocationObjects()
        {
            CurrentRoomObjects = new ReactiveList<AdventureLandCore.Domain.PlaceableObject> {ChangeTrackingEnabled = true};

            foreach (var locationPlaceableObject in _gameExplorerProgam.AdventureApi.GetPlaceableObjectsInRoom(CurrentLocation.Name))
            {
                CurrentRoomObjects.Add(locationPlaceableObject);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ObjectOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedAdventureObject = (AdventureObjectBase) ((System.Windows.Controls.Border)sender).DataContext;
        }

        private void RoomSelectClicked(object sender, RoutedEventArgs e)
        {
            SelectedAdventureObject = CurrentLocation;
        }
    }
}
