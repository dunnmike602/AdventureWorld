using System;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandExplorer.AdventureLandWpf;
using ReactiveUI;
using AdventureObjectBase = AdventureLandCore.Domain.AdventureObjectBase;
using Room = AdventureLandCore.Domain.Room;

namespace AdventureLandExplorer.Controls
{
    public partial class GameExplorer : INotifyPropertyChanged
    {
        private Room _currentLocation;
        private bool _trackCurrentRoom = true;
        private AdventureLandWpfProgram _gameExplorerProgam;
        private AdventureObjectBase _selectedAdventureObject;
        private string _consoleText;

        public ReactiveCommand<Exit, Unit> ExitClickedCommand { get; set; }
        public static RoutedCommand ClearAllCommand = new RoutedCommand();
        public static RoutedCommand OpenContainer = new RoutedCommand();
        public static RoutedCommand GotoRoomCommand = new RoutedCommand();

        private ReactiveList<PlaceableObject> _currentRoomObjects;
        private PlaceableObject _selectedContainer;
        private ReactiveList<PlaceableObject> _containedObjects;
        private ReactiveList<AdventureObjectBase> _allAdventureObjects;
        private string _readyPrompt;

        public ReactiveList<AdventureObjectBase> AllAdventureObjects
        {
            get => _allAdventureObjects;
            set
            {
                _allAdventureObjects = value;
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

        public ReactiveList<PlaceableObject> CurrentRoomObjects
        {
            get => _currentRoomObjects;
            set
            {
                _currentRoomObjects = value;
                OnPropertyChanged();
            }
        }
        
        public PlaceableObject SelectedContainer
        {
            get => _selectedContainer;
            set
            {
                _selectedContainer = value;
                SetCurrentContainerObjects();
                OnPropertyChanged();
            }
        }

        public ReactiveList<PlaceableObject> ContainedObjects
        {
            get => _containedObjects;
            set
            {
                _containedObjects = value;

                OnPropertyChanged();
            }
        }

        public string ReadyPrompt
        {
            get => _readyPrompt;
            set
            {
                _readyPrompt = value;
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

                if (CurrentLocation != null)
                {
                    SetCurrentLocationObjects();
                }

                OnPropertyChanged();
            }
        }

        public bool TrackCurrentRoom
        {
            get => _trackCurrentRoom;
            set
            {
                _trackCurrentRoom = value;

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
            CommandBindings.Add(new CommandBinding(OpenContainer, OpenContainerExecuted));
            CommandBindings.Add(new CommandBinding(GotoRoomCommand, GotoRoomExecuted));
            ExitClickedCommand = ReactiveCommand.Create<Exit>(ExitClickedCommandHandler);
        }

        private void OpenContainerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ExplorerTabs.SelectedIndex = 1;

            SelectedContainer = e.Parameter as PlaceableObject;
            SelectedAdventureObject = SelectedContainer;
        }

        private void GotoRoomExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _gameExplorerProgam.AdventureApi.SetCurrentLocation(SelectedAdventureObject as Room);

            ConsoleText += $"Game location has been changed to {_gameExplorerProgam.AdventureApi.GetCurrentLocation().Name}".AddLineBreaks(2);

            if (_trackCurrentRoom)
            {
                CurrentLocation = _gameExplorerProgam.AdventureApi.GetCurrentLocation();
            }
        }

        private void ClearAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ClearConsoleText();
        }

        private async void RunOnClick(object sender, RoutedEventArgs e)
        {
            RunCommand.IsEnabled = false;

            await Task.Run(() => { RunTheGame(); });
            
            ResetGame();
        }

        private void ResetGame()
        {
            ClearConsoleText();

            CurrentRoomObjects.Clear();

            SelectedAdventureObject = null;

            CurrentLocation = null;

            Title.Text = string.Empty;
            Console.Text = string.Empty;

            RunCommand.IsEnabled = true;
        }

        private void RunTheGame()
        {
            _gameExplorerProgam = new AdventureLandWpfProgram();

            ClearConsoleText();

            _gameExplorerProgam.Initialise(Environment.GetCommandLineArgs()[1], Title, Console, Input, Prompt);
            _gameExplorerProgam.AdventureGameEngine.StateChanged += AdventureGameEngineStateChanged;

            _gameExplorerProgam.InitialiseGameCore();

            ReadyPrompt = _gameExplorerProgam.AdventureApi.GameData.CommandPromptText;

            _gameExplorerProgam.Execute();
        }

        private void ClearConsoleText()
        {
            ConsoleText = null;
        }

        private void AdventureGameEngineStateChanged(object sender, GameStateEventArgs e)
        {
            if (e.CurrentLocation != null && TrackCurrentRoom)
            {
                Dispatcher.Invoke(() =>
                {
                    PopulateAllObjects();
                    CurrentLocation = e.CurrentLocation;
                    SelectedAdventureObject = CurrentLocation;
                });
            }

            Dispatcher.Invoke(() =>
            {
                ConsoleText += e.ToString().AddLineBreaks(2);
            });
        }

        private void PopulateAllObjects()
        {
            AllAdventureObjects = new ReactiveList<AdventureObjectBase>(_gameExplorerProgam.AdventureApi.GetAllAdventureObjects());
        }

        private void SetCurrentContainerObjects()
        {
            ContainedObjects = new ReactiveList<PlaceableObject> {ChangeTrackingEnabled = true};

            foreach (var containerPlaceableObject in _gameExplorerProgam.AdventureApi.GetChildObjects(SelectedContainer))
            {
                ContainedObjects.Add(containerPlaceableObject);
            }
        }

        private void SetCurrentLocationObjects()
        {
            CurrentRoomObjects = new ReactiveList<PlaceableObject> {ChangeTrackingEnabled = true};

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

        private void RoomSelectClicked(object sender, RoutedEventArgs e)
        {
            SelectedAdventureObject = CurrentLocation;
        }

        private void ExitClickedCommandHandler(Exit leavingByExit)
        {
            var newRoom = _gameExplorerProgam.AdventureApi.GetRoomFromName(leavingByExit.RoomName);

            SelectedAdventureObject = leavingByExit;
            CurrentLocation = newRoom;
        }
        
        private void PlaceableObjectSelectClicked(object sender, RoutedEventArgs e)
        {
            SelectedAdventureObject = (AdventureObjectBase)((FrameworkElement)sender).DataContext;
        }

        private void ContainerSelectClicked(object sender, RoutedEventArgs e)
        {
            SelectedAdventureObject = SelectedContainer;
        }

        private void ParentContainerClicked(object sender, RoutedEventArgs e)
        {
            SelectedContainer = SelectedContainer.Parent as PlaceableObject;
            SelectedAdventureObject = SelectedContainer;
        }
    }
}
