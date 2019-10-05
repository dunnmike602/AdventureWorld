using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Services.Helpers;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.AdventureWorld.GameExecution;
using DiagramDesigner.Controls.AboutBox;
using DiagramDesigner.Extensions;
using DiagramDesigner.Helpers;
using DiagramDesigner.Interfaces;
using DiagramDesigner.Properties;
using DiagramDesigner.Services;
using DiagramDesigner.Symbols;
using DiagramDesigner.Symbols.Helpers;
using DiagramDesigner.Wizards;
using FontAwesome.WPF;
using Microsoft.Scripting.Utils;
using Microsoft.Win32;
using ReactiveUI;
using SharedControls;
using Syncfusion.Data.Extensions;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml;
using System.Xml.Linq;
using DiagramDesigner.AdventureWorld.Domain.Interfaces;
using DiagramDesigner.Controls.Helpers;
using AdventureObjectBase = DiagramDesigner.AdventureWorld.Domain.AdventureObjectBase;
using Color = System.Windows.Media.Color;
using Container = DiagramDesigner.AdventureWorld.Domain.Container;
using Exit = DiagramDesigner.AdventureWorld.Domain.Exit;
using Key = System.Windows.Input.Key;
using Npc = DiagramDesigner.AdventureWorld.Domain.Npc;
using Path = System.IO.Path;
using PlaceableObject = DiagramDesigner.AdventureWorld.Domain.PlaceableObject;
using Point = System.Windows.Point;
using Room = DiagramDesigner.AdventureWorld.Domain.Room;
using Size = System.Windows.Size;

namespace DiagramDesigner
{
    public partial class AdventureDesigner : INotifyPropertyChanged, IStatusMessageProvider
    {
        private const int MarginSize = 20;

        private ReactiveList<Client> _clients;
        private Client _selectedClient;

        private ReactiveList<DropDownItem> _newOptions;
        private DropDownItem _selectedOption;

        private ObservableCollection<ValidationItemAttribute> _validationItems;
        private static readonly object Lock = new object();
        public static AdventureDesigner Instance => Application.Current.Windows.OfType<AdventureDesigner>().FirstOrDefault();
        public bool DisableExplorer { get; set; }

        public RoutedCommand WizardCommand { get; set; } = new RoutedCommand();
        public RoutedCommand AboutCommand { get; set; } = new RoutedCommand();
        public RoutedCommand ExitCommand { get; set; } = new RoutedCommand();
        public RoutedCommand PropertiesCommand { get; set; } = new RoutedCommand();
        public RoutedCommand OptionsCommand { get; set; } = new RoutedCommand();
        public RoutedCommand RunCommand { get; set; } = new RoutedCommand();
        public RoutedCommand CheckCommand { get; set; } = new RoutedCommand();
        public RoutedCommand SaveCurrentProjectCommand { get; set; } = new RoutedCommand();

        public ReactiveCommand<AdventureObjectBase, Unit> EntityClickedCommand { get; set; }
        public ReactiveCommand<Exit, Unit> ExitClickedCommand { get; set; }
        public ReactiveCommand<Container, Unit> OpenContainerCommand { get; set; }
        public ReactiveCommand<Container, Unit> ContainerNavigateCommand { get; set; }
        public RoutedCommand PreviousRoomCommand { get; set; } = new RoutedCommand();
        public RoutedCommand NextRoomCommand { get; set; } = new RoutedCommand();

        private static string _lastFile;
        private double _roomScale = 1.0f;
        private double _objectScale = 1.0f;
        private ObservableCollection<string> _recentFiles;
        private ReactiveList<AdventureCommandMapping> _adventureCommandMappings;
        private bool _disableRefresh;
        private Room _currentRoom;
        private Container _currentContainer;

        private int _currentRoomHistoryIndex;

        public List<ScriptContainerHeader> ScriptHeaders => new List<ScriptContainerHeader>
        {
            new ScriptContainerHeader
            {
                Header = "Control Scripts",
                Image = "/Resources/Images/command.png",
                Type = ScriptTypes.Control
            },
            new ScriptContainerHeader
            {
                Header = "Common Scripts",
                Image = "/Resources/Images/command.png",
                Type = ScriptTypes.Common
            },
            new ScriptContainerHeader
            {
                Header = "Room Scripts",
                Image = "/Resources/Images/New-Room.png",
                Type = ScriptTypes.Room
            },
            new ScriptContainerHeader
            {
                Header = "Object Scripts",
                Image = "/Resources/Images/Key-icon.png",
                Type = ScriptTypes.PlaceableObject
            },
            new ScriptContainerHeader
            {
                Header = "Exit Scripts",
                Image = "/Resources/Images/exit.png",
                Type = ScriptTypes.Exit
            },
            new ScriptContainerHeader
            {
                Header = "Command Scripts",
                Image = "/Resources/Images/command.png",
                Type = ScriptTypes.Command
            }
        };

        public int CurrentRoomHistoryIndex
        {
            get => _currentRoomHistoryIndex;
            set
            {
                _currentRoomHistoryIndex = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Container CurrentContainer
        {
            get => _currentContainer;
            set
            {
                _currentContainer = value;
                OnPropertyChanged();
            }
        }

        public Room CurrentRoom
        {
            get => _currentRoom;
            set
            {
                _currentRoom = value;
                OnPropertyChanged();
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public ReactiveList<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        public AdventureCommandMapping SelectedCommand
        {
            get => (AdventureGameDesignerViewModel.Instance.SelectedCommand);
            set
            {
                AdventureGameDesignerViewModel.Instance.SelectedCommand = value;
                OnPropertyChanged();
            }
        }

        public ReactiveList<DropDownItem> NewOptions
        {
            get => _newOptions;
            set
            {
                _newOptions = value;
                OnPropertyChanged();
            }
        }

        public DropDownItem SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
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

        public ObservableCollection<ValidationItemAttribute> ValidationItems
        {
            get => _validationItems;
            set
            {
                _validationItems = value;
                BindingOperations.EnableCollectionSynchronization(ValidationItems, Lock);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> RecentFiles
        {
            get => (_recentFiles);
            set
            {
                _recentFiles = value;
                OnPropertyChanged();
            }
        }

        public double RoomScale
        {
            get => _roomScale;
            set
            {
                _roomScale = value;
                OnPropertyChanged();
            }
        }

        public double ObjectScale
        {
            get => _objectScale;
            set
            {
                _objectScale = value;
                OnPropertyChanged();
            }
        }

        public AdventureDesigner()
        {
            InitializeComponent();

            SetUpDataContexts();

            CreateClients();

            CreateOptions();

            SetDefaultTitle();

            SetupCommands();

            ResetEnvironmentForNewProject();
        }

        private void SetupCommands()
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, NewExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, SaveExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, PrintExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Stop, ExitExecuted));
            CommandBindings.Add(new CommandBinding(ExitCommand, ExitExecuted));
            CommandBindings.Add(new CommandBinding(AboutCommand, AboutExecuted));
            CommandBindings.Add(new CommandBinding(PropertiesCommand, PropertiesExecuted));
            CommandBindings.Add(new CommandBinding(OptionsCommand, OptionsExecuted));
            CommandBindings.Add(new CommandBinding(WizardCommand, WizardExecuted));
            CommandBindings.Add(new CommandBinding(CheckCommand, CheckExecuted));
            CommandBindings.Add(new CommandBinding(RunCommand, RunExecuted));
            CommandBindings.Add(new CommandBinding(SaveCurrentProjectCommand, SaveCurrentProjectCommandExecuted));
            
            RunCommand.InputGestures.Add(new KeyGesture(Key.F5));

            EntityClickedCommand = ReactiveCommand.Create<AdventureObjectBase>(EntityClickedCommandHandler);
            ExitClickedCommand = ReactiveCommand.Create<Exit>(ExitClickedCommandHandler);
            OpenContainerCommand = ReactiveCommand.Create<Container>(ContainerClickedHandler);
            ContainerNavigateCommand = ReactiveCommand.Create<Container>(ContainerNavigateClickedHandler);

            CommandBindings.Add(new CommandBinding(PreviousRoomCommand, PreviousRoomCommandExecuted));
            CommandBindings.Add(new CommandBinding(NextRoomCommand, NextRoomCommandExecuted));
        }

        private void SaveCurrentProjectCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveCurrentProject(false);
        }

        private void NextRoomCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (CurrentRoomHistoryIndex < AdventureGameDesignerViewModel.Instance.Rooms.Count - 1)
            {
                CurrentRoomHistoryIndex++;
            }
            else
            {
                CurrentRoomHistoryIndex = 0;
            }

            SetRoom();
        }

        private void SetRoom()
        {
            CurrentRoom = AdventureGameDesignerViewModel.Instance.Rooms[CurrentRoomHistoryIndex];
            CurrentContainer = null;
        }

        private void PreviousRoomCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (CurrentRoomHistoryIndex > 0)
            {
                CurrentRoomHistoryIndex--;
            }
            else
            {
                CurrentRoomHistoryIndex = AdventureGameDesignerViewModel.Instance.Rooms.Count - 1;
            }

            SetRoom();
        }


        private void ContainerNavigateClickedHandler(Container container)
        {
            CurrentContainer = container.Parent as Container;
            ShowPropertiesForObject(CurrentContainer);
        }

        private void ContainerClickedHandler(Container container)
        {
            CurrentContainer = container;
            ShowPropertiesForObject(CurrentContainer);
        }

        private void ExitClickedCommandHandler(Exit exit)
        {
            CurrentRoom = exit.ToRoom;
            AddRoomHistory();
            ShowPropertiesForObject(exit);
            CurrentContainer = null;
        }

        private void AddRoomHistory()
        {
            CurrentRoomHistoryIndex = AdventureGameDesignerViewModel.Instance.Rooms.FindIndex(a => a.ControlId == CurrentRoom.ControlId);
        }

        private void EntityClickedCommandHandler(AdventureObjectBase adventureObject)
        {
            ShowPropertiesForObject(adventureObject);
        }

        private void AboutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var aboutBox = new AboutBoxSimple(this);
            aboutBox.Show();
        }

        private void CreateOptions()
        {
            NewOptions = new ReactiveList<DropDownItem>
            {
                new DropDownItem
                {
                   IsChecked = true,
                    Label = "New",
                    ToolTip = "Create Empty Project"
                },
                new DropDownItem
                {
                    IsChecked = false,
                    Label = "Wizard",
                    ToolTip = "Create a project with the Wizard"
                },
            };


            SelectedOption = NewOptions[0];
        }

        private void CreateClients()
        {
            Clients = new ReactiveList<Client>
            {
                new Client
                {
                    ClientType = ClientType.ConsoleApplication,
                    IsChecked = true,
                    Label = "Console",
                    ToolTip = "Run in Command Prompt."
                },
                new Client
                {
                    ClientType = ClientType.GameExplorer,
                    IsChecked = false,
                    Label = "Explorer",
                    ToolTip = "Run in Game Explorer Debugger."
                },
                new Client
                {
                    ClientType = ClientType.WindowsStore10,
                    IsChecked = false,
                    Label = "Store Client",
                    ToolTip = "Run in Windows 10 Store Client."
                }
            };

            SelectedClient = Clients[0];
        }

        private async void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedOption.Label == "New")
            {
                await ResetEnvironmentForNewProject();
            }
            else
            {
                WizardExecuted(sender, e);
            }
        }

        private async Task ResetEnvironmentForNewProject()
        {
            ResetStatus();

            _lastFile = null;

            await Start(false);

            SetDefaultTitle();

            ScriptExplorer.Reset(ScriptHeaders);

            SetupErrors();

            SetDefaultPropertyGridData();

            SetupCommands(true);

            SetupRecentFileList();

            SetProjectExplorerTabsToIndex(ProjectExplorerTabIndexes.Explorer);

            SetMainScreenTabsToIndex(MainScreenTabIndexes.GameDesigner);

            ResetEntityExplorer();
        }

        private void SetupCommands(bool isNewProject)
        {
            InitialiseCommands(isNewProject);

            AdventureCommandProperties.SelectedObject = AdventureGameDesignerViewModel.Instance.AdventureCommandMappings[0];
            SelectedCommand = AdventureGameDesignerViewModel.Instance.AdventureCommandMappings[0];
        }

        private void SetupErrors()
        {
            Application.Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
        }

        private void CurrentDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            TaskDialogService.ShowApplicationError(this, e.Exception);

            e.Handled = true;
        }

        private async void CheckExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResetStatus();

            await SetUpValidationItems();
            ShowErrors();
        }

        internal bool IsModelValid()
        {
            return ValidationItems.Count(m => m.Severity == Severities.Error) == 0;
        }

        private async void RunExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await RunExecutedHelper(SelectedClient.ClientType);
        }

        private async Task RunExecutedHelper(ClientType clientType)
        {
            ResetStatus();

            await SetUpValidationItems();

            if (!IsModelValid())
            {
                ShowErrors();
                return;
            }

            ExecutorFactory.ExecuteGame(AdventureGameDesignerViewModel.Instance.Map(), clientType, null);
        }


        internal void ResetStatus()
        {
            SetStatus(string.Empty, Colors.Transparent, false);
        }

        private void ExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void PropertiesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResetStatus();

            ShowGameProperties();
        }

        private void WizardExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            using (var gameWizardViewModel = new GameWizardViewModel())
            {
                var wizardForm = new GameWizardView { DataContext = gameWizardViewModel };

                var returnValue = wizardForm.ShowDialog();

                if (returnValue == true)
                {
                    CreateProjectFromWizard(gameWizardViewModel);
                }
            }
        }

        private async Task CreateProjectFromWizard(GameWizardViewModel gameWizardViewModel)
        {
            await ResetEnvironmentForNewProject();

            try
            {
                MainWindow.IsEnabled = false;
                PopulateGameViewModelFromWizard(gameWizardViewModel);
                await SetUpValidationItems();

                ShowGameProperties();
            }
            finally
            {
                MainWindow.IsEnabled = true;
            }
        }

        private void PopulateGameViewModelFromWizard(GameWizardViewModel gameWizardViewModel)
        {
            var gameDesignerViewModel = PopulateFeatures(gameWizardViewModel);

            PopulateRooms(gameWizardViewModel, gameDesignerViewModel);

            PopulatePlaceableObjects(gameWizardViewModel, gameDesignerViewModel);

            PopulateContainer(gameWizardViewModel, gameDesignerViewModel);

            PopulateNpc(gameWizardViewModel, gameDesignerViewModel);

            SetStatus("FINISHING OFF.....", Colors.Black, true);

            ArrangeObjects(RoomDesigner.Children.OfType<DesignerItem>().ToList(), ScrollRooms, RoomDesigner);
            ArrangeObjects(ObjectDesigner.Children.OfType<DesignerItem>().ToList(), ScrollObjects, ObjectDesigner);
        }

        private void PopulatePlaceableObjects(GameWizardViewModel gameWizardViewModel, AdventureGameDesignerViewModel gameDesignerViewModel)
        {
            SetStatus("SETTING UP OBJECTS.....", Colors.Black, true);

            foreach (var wizardObject in gameWizardViewModel.PlaceableObjectsViewModel.PlaceableObjects.Cast<PlaceableObjectViewModel>())
            {
                var placeableObject = new PlaceableObject
                {
                    BaseName = wizardObject.BaseName,
                    Descriptions = wizardObject.Descriptions.ToObservableCollection(),
                    Fixed = wizardObject.Fixed,
                    HideFromAutoList = wizardObject.HideFromAutoList,
                    IsLightSource = wizardObject.IsLightSource,
                    IsLit = wizardObject.IsLit,
                    Visible = wizardObject.IsVisible,
                    InventoryDescription = wizardObject.InventoryDescription,
                };

                gameDesignerViewModel.PlaceableObjects.Add(placeableObject);
                var designerItem = ObjectDesigner.AddStencilItem("ObjectGrid");

                AdventureObjectHelper.ProcessExistingAdventureObject(designerItem, placeableObject, ObjectType.PlaceableObject);
            }
        }

        private void PopulateContainer(GameWizardViewModel gameWizardViewModel, AdventureGameDesignerViewModel gameDesignerViewModel)
        {
            SetStatus("SETTING UP CONTAINERS.....", Colors.Black, true);

            foreach (var wizardContainer in gameWizardViewModel.ContainersViewModel.Containers.Cast<ContainerViewModel>())
            {
                var container = new Container
                {
                    BaseName = wizardContainer.BaseName,
                    Descriptions = wizardContainer.Descriptions.ToObservableCollection(),
                    Fixed = wizardContainer.Fixed,
                    HideFromAutoList = wizardContainer.HideFromAutoList,
                    IsLightSource = wizardContainer.IsLightSource,
                    IsLit = wizardContainer.IsLit,
                    Visible = wizardContainer.IsVisible,
                    InventoryDescription = wizardContainer.InventoryDescription,
                    IsLocked = wizardContainer.IsLocked,
                    IsOpen = wizardContainer.IsOpen,
                };

                gameDesignerViewModel.PlaceableObjects.Add(container);
                var designerItem = ObjectDesigner.AddStencilItem("ContainerGrid");

                AdventureObjectHelper.ProcessExistingAdventureObject(designerItem, container, ObjectType.Container);
            }
        }

        private void PopulateNpc(GameWizardViewModel gameWizardViewModel, AdventureGameDesignerViewModel gameDesignerViewModel)
        {
            SetStatus("SETTING UP NPCs.....", Colors.Black, true);

            foreach (var wizardNpc in gameWizardViewModel.NpcsViewModel.Npcs.Cast<NpcViewModel>())
            {
                var npc = new AdventureWorld.Domain.Npc
                {
                    BaseName = wizardNpc.BaseName,
                    Descriptions = wizardNpc.Descriptions.ToObservableCollection(),
                    Fixed = wizardNpc.Fixed,
                    HideFromAutoList = wizardNpc.HideFromAutoList,
                    IsLightSource = wizardNpc.IsLightSource,
                    IsLit = wizardNpc.IsLit,
                    Visible = wizardNpc.IsVisible,
                    InventoryDescription = wizardNpc.InventoryDescription,
                    IsLocked = wizardNpc.IsLocked,
                    IsOpen = wizardNpc.IsOpen,
                    AutoFollow = wizardNpc.AutoFollow,
                };

                gameDesignerViewModel.PlaceableObjects.Add(npc);
                var designerItem = ObjectDesigner.AddStencilItem("NpcGrid");

                AdventureObjectHelper.ProcessExistingAdventureObject(designerItem, npc, ObjectType.Npc);
            }
        }

        private void PopulateRooms(GameWizardViewModel gameWizardViewModel, AdventureGameDesignerViewModel gameDesignerViewModel)
        {
            SetStatus("SETTING UP ROOMS.....", Colors.Black, true);

            foreach (var wizardRoom in gameWizardViewModel.RoomsViewModel.Rooms.Cast<RoomViewModel>())
            {
                var room = new Room
                {
                    BaseName = wizardRoom.BaseName,
                    Descriptions = wizardRoom.Descriptions.ToObservableCollection(),
                    IsDark = wizardRoom.IsDark,
                };

                gameDesignerViewModel.Rooms.Add(room);
                var designerItem = RoomDesigner.AddStencilItem("RoomGrid");

                AdventureObjectHelper.ProcessExistingAdventureObject(designerItem, room, ObjectType.Room);
            }

            gameDesignerViewModel.StartRoom = gameDesignerViewModel.Rooms.First(room =>
                room.BaseName == gameWizardViewModel.RoomsViewModel.SelectedRoom.BaseName);
        }

        private AdventureGameDesignerViewModel PopulateFeatures(GameWizardViewModel gameWizardViewModel)
        {
            SetStatus("SETTING BASIC PROPERTIES.....", Colors.Black, true);

            var gameDesignerViewModel = AdventureGameDesignerViewModel.Instance;

            gameDesignerViewModel.EnablePlayerLost = gameWizardViewModel.GameFeaturesViewModel.EnablePlayerLost;
            gameDesignerViewModel.EnableExitDescriptions = gameWizardViewModel.GameFeaturesViewModel.EnableExitDescriptions;
            gameDesignerViewModel.EnableInventorySize = gameWizardViewModel.GameFeaturesViewModel.EnableInventorySize;
            gameDesignerViewModel.EnableScore = gameWizardViewModel.GameFeaturesViewModel.EnableScore;
            gameDesignerViewModel.EnableTitles = gameWizardViewModel.GameFeaturesViewModel.EnableTitles;
            gameDesignerViewModel.EnableShowItemsInRoom = gameWizardViewModel.GameFeaturesViewModel.EnableShowItemsInRoom;

            gameDesignerViewModel.Title = gameWizardViewModel.GameWizardBasicViewModel.Title;
            gameDesignerViewModel.Introduction = gameWizardViewModel.GameWizardBasicViewModel.Introduction;
            gameDesignerViewModel.GameName = gameWizardViewModel.GameWizardBasicViewModel.GameName;
            gameDesignerViewModel.GameDescription = gameWizardViewModel.GameWizardBasicViewModel.GameDescription;
            gameDesignerViewModel.AnotherGameText = gameWizardViewModel.GameWizardBasicViewModel.AnotherGameText;
            gameDesignerViewModel.AnotherGameYesResponse = gameWizardViewModel.GameWizardBasicViewModel.AnotherGameYesResponse;
            gameDesignerViewModel.CommandPromptText = gameWizardViewModel.GameWizardBasicViewModel.CommandPromptText;
            gameDesignerViewModel.MaximumInventorySize = gameWizardViewModel.GameWizardBasicViewModel.MaximumInventorySize;
            gameDesignerViewModel.MaximumScore = gameWizardViewModel.GameWizardBasicViewModel.MaximumScore;
            gameDesignerViewModel.PlayerLostMessage = gameWizardViewModel.GameWizardBasicViewModel.PlayerLostMessage;
            gameDesignerViewModel.WonGame = gameWizardViewModel.GameWizardBasicViewModel.WonGameMessage;
            return gameDesignerViewModel;
        }

        private void OptionsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResetStatus();

            SetUpOptions();
        }

        private void SetDefaultTitle()
        {
            Title = "Adventure Game Designer";
        }

        private async Task Start(bool isStarting)
        {
            ValidationItems = new ObservableCollection<ValidationItemAttribute>();
            ClearUiElements();

            if (!isStarting)
            {
                ClearObjects();
            }

            await SetUpValidationItems();
            SetUpDataContexts();
        }

        public void ClearObjects()
        {
            ResetStatus();

            AdventureGameDesignerViewModel.Instance.Clear();
        }

        private void SetUpDataContexts()
        {
            FileOpenButton.DataContext = this;

            DataContext = this;
            Explorer.DataContext = AdventureGameDesignerViewModel.Instance.GameRootList;
            RoomDesigner.SetDataContext(AdventureGameDesignerViewModel.Instance.Rooms);
            ObjectDesigner.SetDataContext(AdventureGameDesignerViewModel.Instance.PlaceableObjects);

            AdventureGameDesignerViewModel.Instance.PlaceableObjects.CollectionChanged += PlaceableObjectsCollectionChanged;
        }

        public async Task<bool> CheckCurrentGameIsValid()
        {
            SetStatus("VALIDATING GAME MODEL.....", Colors.Black, true);
            CheckModelButton.IsEnabled = true;

            try
            {
                await Task.Run(async () => { await SetUpValidationItems(); });
            }
            finally
            {
                CheckModelButton.IsEnabled = false;
            }

            return IsModelValid();
        }

        internal async Task SetUpValidationItems()
        {
            try
            {

                await Task.Run(() => { SetUpValidationItemsHelper(); });

                if (IsModelValid())
                {
                    SetStatus("GAME MODEL IS VALID", Colors.Green, false);
                }
                else
                {
                    SetStatus("GAME MODEL IS INVALID", Colors.Red, false);
                }
            }
            catch (Exception)
            {
                SetStatus(string.Empty, Colors.Transparent, false);
                throw;
            }
        }

        private void SetUpValidationItemsHelper()
        {
            ValidationItems.Clear();

            var gameItems = ValidationItemHelper.CreateOptionsValidationItems(AdventureGameDesignerViewModel.Instance,
                args => ShowGameProperties(),
                ValidationCategories.Game, string.Empty);

            ValidationItemHelper.Add(ValidationItems, gameItems);

            foreach (var nextRoom in AdventureGameDesignerViewModel.Instance.RoomsList)
            {
                ValidateRoomItem(nextRoom);
            }

            foreach (var nextObject in AdventureGameDesignerViewModel.Instance.PlaceableObjectsList)
            {
                ValidatePlaceableItem(nextObject);
            }

            foreach (var exit in AdventureGameDesignerViewModel.Instance.ExitList)
            {
                exit.Init();
                ValidateExitItem(exit);
            }

            ValidateDuplicates(AdventureGameDesignerViewModel.Instance.RoomsList.Cast<AdventureObjectBase>().ToList(),
                ValidationCategories.Room, ShowRoom);

            ValidateDuplicates(AdventureGameDesignerViewModel.Instance.PlaceableObjectsList.Cast<AdventureObjectBase>().ToList(),
                ValidationCategories.Object, ShowPlaceableObject);

            ValidateEmptyCommands(AdventureGameDesignerViewModel.Instance.GetActiveAdventureCommandMappings());
            ValidateDuplicateCommands(AdventureGameDesignerViewModel.Instance.GetActiveAdventureCommandMappings());

            ValidateConversations();
        }

        private void ValidateConversations()
        {
            foreach (var npc in AdventureGameDesignerViewModel.Instance.PlaceableObjects.OfType<Npc>())
            {
                if (npc.ConversationTree?.ValidationItems.Count > 0)
                {
                    ValidationItems.Add(new ValidationItemAttribute
                    {
                        Action = ShowConversations,
                        ControlId = npc.ControlId,
                        Severity = Severities.Error,
                        ValidationCategory = ValidationCategories.Conversation,
                        Description = "One or more Conversations have validation errors",
                        ValidationType = ValidationTypes.InvalidConversation,
                        Name = npc.BaseName,
                    });
                }
            }
        }

        private bool ShowConversations(Guid? controlId)
        {
            ConversationHelper.OpenEditor((Npc)AdventureGameDesignerViewModel.Instance.PlaceableObjects.First(obj => obj.ControlId == controlId));

            return true;
        }

        private void ValidateDuplicateCommands(IList<AdventureCommandMapping> adventureCommandMapping)
        {
            var duplicates = adventureCommandMapping.Select(acm => acm.GetAllNames()).SelectMany(name => name)
                .GroupBy(name => name).Where(m => m.Count() > 1).ToList();

            if (duplicates.Any())
            {
                foreach (var duplicate in duplicates)
                {
                    var itemsWithDuplicate =
                        adventureCommandMapping.Where(acm => acm.GetAllNames().Contains(duplicate.Key));

                    foreach (var itemWithDuplicate in itemsWithDuplicate)
                    {
                        ValidationItems.Add(new ValidationItemAttribute
                        {
                            Action = ShowCommand,
                            Severity = Severities.Error,
                            ControlId = itemWithDuplicate.ControlId,
                            ValidationCategory = ValidationCategories.Command,
                            Description =
                                $"Name,alias or substitution list is duplicated in same or another command: {duplicate.Key}",
                            ValidationType = ValidationTypes.DuplicateCommand,
                            Name = itemWithDuplicate.VerbName,
                        });
                    }
                }
            }
        }

        private void ValidateEmptyCommands(IList<AdventureCommandMapping> adventureCommandMapping)
        {
            foreach (var adventureCommand in adventureCommandMapping)
            {
                if (string.IsNullOrWhiteSpace(adventureCommand.VerbName))
                {
                    ValidationItems.Add(new ValidationItemAttribute
                    {
                        Action = ShowCommand,
                        ControlId = adventureCommand.ControlId,
                        Severity = Severities.Error,
                        ValidationCategory = ValidationCategories.Command,
                        Description = "All commands must have a Verb Name",
                        ValidationType = ValidationTypes.EmptyCommand,
                    });
                }
            }
        }

        private void ValidateDuplicates(IList<AdventureObjectBase> items, ValidationCategories category,
            Func<Guid?, bool> validationAction)
        {
            var duplicates = items.GroupBy(m => m.BaseName).Where(m => m.Count() > 1).ToList();

            if (duplicates.Any())
            {
                foreach (var duplicate in duplicates)
                {
                    ValidationItems.Add(new ValidationItemAttribute
                    {
                        Action = validationAction,
                        Severity = Severities.Error,
                        ControlId = duplicate.First().ControlId,
                        ValidationCategory = category,
                        Description = $"More than one item has the name {duplicate.First().BaseName}",
                        ValidationType = ValidationTypes.Duplicate,
                        Name = duplicate.First().BaseName
                    });
                }
            }
        }

        public void ValidateExitItem(Exit exit)
        {
            var items = ValidationItemHelper.CreateOptionsValidationItems(exit, ShowExit,
                ValidationCategories.Exit, exit.BaseName, ValidationTypes.All, exit.ControlId);

            ValidationItemHelper.Add(ValidationItems, items);
        }

        public void ValidatePlaceableItem(PlaceableObject placeableObject)
        {
            var items = ValidationItemHelper.CreateOptionsValidationItems(placeableObject, ShowPlaceableObject,
                ValidationCategories.Object, placeableObject.BaseName, ValidationTypes.All, placeableObject.ControlId);

            ValidationItemHelper.Add(ValidationItems, items);
        }

        public void ValidateRoomItem(Room room)
        {
            var items = ValidationItemHelper.CreateOptionsValidationItems(room, ShowRoom,
                ValidationCategories.Room, room.BaseName, ValidationTypes.All, room.ControlId);

            ValidationItemHelper.Add(ValidationItems, items);
        }

        private bool ShowExit(Guid? controlId)
        {
            var itemToSelect = RoomDesigner.GetConnectorByUid(controlId.Value);

            if (itemToSelect != null)
            {
                RoomDesigner.SelectionService.SelectItem(itemToSelect);

                if (itemToSelect.Source.Position.X > itemToSelect.Sink.Position.X)
                {
                    itemToSelect.Source.BringIntoView();
                }
                else
                {
                    itemToSelect.Sink.BringIntoView();
                }

                ShowPropertiesForObject(controlId.Value);

                return true;
            }

            return false;
        }

        public void ShowPropertiesForObject(Guid controlId)
        {
            var adventureObject = AdventureGameDesignerViewModel.Instance.FindObjectByGuid(controlId);
            ShowPropertiesForObject(adventureObject);
        }

        public AdventureObjectBase GetSelectedObject()
        {
            return GameProperties.SelectedObject as AdventureObjectBase;
        }

        public void RefreshObjectIfSelected(AdventureObjectBase adventureObject)
        {
            if ((GameProperties.SelectedObject as AdventureObjectBase)?.ControlId == adventureObject.ControlId)
            {
                GameProperties.SelectedObject = null;
                ShowPropertiesForObject(adventureObject);
            }
        }

        public void ShowPropertiesForObject(AdventureObjectBase adventureObject)
        {
            GameProperties.SelectedObject = adventureObject;

            if (adventureObject != null)
            {
                SetProjectExplorerTabsToIndex(ProjectExplorerTabIndexes.Explorer);
            }
        }

        public void UpdatePropertyGrid()
        {
            var tempObject = GameProperties.SelectedObject;

            GameProperties.SelectedObject = null;
            GameProperties.SelectedObject = tempObject;
        }

        private bool ShowRoom(Guid? controlId)
        {
            return ShowRoom(controlId, true);
        }

        private bool ShowRoom(Guid? controlId, bool setObjectBrowser)
        {
            if (controlId != null)
            {
                var itemToSelect = RoomDesigner.GetDesignerItemByUid(controlId.Value);

                if (itemToSelect != null)
                {
                    RoomDesigner.SelectionService.ClearSelection();
                    RoomDesigner.SelectionService.SelectItem(itemToSelect);
                    itemToSelect.BringIntoView();

                    if (setObjectBrowser)
                    {
                        ShowPropertiesForObject(controlId.Value);
                    }

                    return true;
                }
            }

            return false;
        }

        private bool ShowCommand(Guid? controlId)
        {
            var commandToSelect =
                AdventureGameDesignerViewModel.Instance.AdventureCommandMappings.FirstOrDefault(cmd =>
                    cmd.ControlId == controlId);

            if (commandToSelect != null)
            {
                AdventureGameDesignerViewModel.Instance.SelectedCommand = commandToSelect;
                SetProjectExplorerTabsToIndex(ProjectExplorerTabIndexes.Commands);

                return true;
            }

            return false;
        }

        private bool ShowPlaceableObject(Guid? controlId)
        {
            var itemToSelect = ObjectDesigner.GetDesignerItemByUid(controlId.Value);

            if (itemToSelect != null)
            {
                ObjectDesigner.SelectionService.ClearSelection();
                ObjectDesigner.SelectionService.SelectItem(itemToSelect);
                itemToSelect.BringIntoView();

                ShowPropertiesForObject(controlId.Value);

                return true;
            }

            return false;
        }

        private void ClearUiElements()
        {
            RoomDesigner.Children.Clear();
            RoomDesigner.SelectionService.ClearSelection();

            ObjectDesigner.Children.Clear();
            RoomDesigner.SelectionService.ClearSelection();
        }

        public void ShowScriptDesigner(Script script)
        {
            ScriptExplorer.InitialiseFromScript(script);

            if (MainScreenTabs.SelectedIndex != (int)MainScreenTabIndexes.ScriptDesigner)
            {
                SetMainScreenTabsToIndex(MainScreenTabIndexes.ScriptDesigner);
            }
            else
            {
                ScriptExplorer.SetupExplorer();
            }
        }

        private XElement PromptForFileAndOpenIt()
        {
            var openFile = new OpenFileDialog
            {
                Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = AdventureGameDesignerViewModel.Instance.ProjectDirectory,
            };

            if (openFile.ShowDialog() == true)
            {
                AddToRecentFiles(openFile.FileName);

                return OpenSpecifiedFile(openFile.FileName);
            }
            else
            {
                return null;
            }
        }

        private XElement OpenSpecifiedFile(string fileName)
        {
            Options.Instance.LastProjectDirectory = AdventureGameDesignerViewModel.Instance.ProjectDirectory;

            try
            {
                var fileXml = XElement.Load(fileName);

                _lastFile = fileName;

                return XElement.Load(fileName);
            }
            catch (Exception ex)
            {
                TaskDialogService.ShowError(this, "This file is missing, corrupt or does not appear to be the correct format for an Adventure Designer Project.", ex.ToString());

                return null;
            }
        }

        private static void LoadDesigner(XContainer root, DesignerCanvas designerCanvas)
        {
            designerCanvas.InvalidateVisual();

            var itemsXml = root.Element(designerCanvas.Name)
                ?.Elements("DesignerItems")
                .Elements("DesignerItem");

            foreach (var item in from itemXml in itemsXml
                                 let id = new Guid(itemXml.Element("ID")?.Value)
                                 select DesignerCanvas.DeserializeDesignerItem(itemXml, id, 0, 0))
            {
                designerCanvas.Children.Add(item);
                DesignerCanvas.SetConnectorDecoratorTemplate(item);
            }
            
            var connectionsXml = root.Element(designerCanvas.Name).Elements("Connections").Elements("Connection");

            foreach (var connectionXml in connectionsXml)
            {
                var sourceId = new Guid(connectionXml.Element("SourceID").Value);
                var sinkId = new Guid(connectionXml.Element("SinkID").Value);

                var sourceConnectorName = connectionXml.Element("SourceConnectorName").Value;
                var sinkConnectorName = connectionXml.Element("SinkConnectorName").Value;

                var sourceConnector = DesignerCanvas.GetConnector(designerCanvas, sourceId, sourceConnectorName);
                var sinkConnector = DesignerCanvas.GetConnector(designerCanvas, sinkId, sinkConnectorName);

                var connection = new Connection(sourceConnector, sinkConnector)
                { ID = new Guid(connectionXml.Element("ID").Value) };

                Panel.SetZIndex(connection, int.Parse(connectionXml.Element("zIndex").Value));

                designerCanvas.Children.Add(connection);

                var exit = AdventureGameDesignerViewModel.Instance.FindObjectByGuid(connection.ID);

                ApplyExitTemplate(connection);

                StencilHelpers.BindConnectorToObject(connection, exit, "Direction");
            }
        }

        public static void ApplyExitTemplate(Connection connection)
        {
            connection.ApplyTemplate();
        }

        private static void ReapplyBindings(DesignerItem item, IEnumerable<DesignerItem> allItems)
        {
            if (!(item.Content is Grid childGrid))
            {
                return;
            }

            var adventureObject = AdventureGameDesignerViewModel.Instance.FindObjectByGuid(item.ID);

            string labelName;

            switch (adventureObject.ObjectType)
            {
                case ObjectType.Room:
                    labelName = "RoomText";
                    StencilHelpers.BindStencilImageToAdventureObject(childGrid, adventureObject, "RoomImage");
                    break;
                case ObjectType.PlaceableObject:
                    labelName = "ObjectText";
                    StencilHelpers.BindStencilImageToAdventureObject(childGrid, adventureObject, "ObjectImage");
                    break;
                case ObjectType.Npc:
                    labelName = "NpcText";
                    StencilHelpers.BindStencilImageToAdventureObject(childGrid, adventureObject, "NpcImage");
                    break;
                default:
                    labelName = "ContainerText";
                    StencilHelpers.BindStencilImageToAdventureObject(childGrid, adventureObject, "ContainerImage");
                    break;
            }

            StencilHelpers.BindStencilToObject(childGrid, adventureObject, labelName, "BaseName");

            if (adventureObject.IsPlaceableObject &&
                adventureObject is PlaceableObject placeableObject && placeableObject.Parent != null &&
                allItems.FirstOrDefault(desItem => desItem.ID == placeableObject.Parent.ControlId)?.Content is Grid
                    parentGrid)
            {
                StencilHelpers.BindParentToObject(childGrid, parentGrid, placeableObject, placeableObject.Parent);
            }
        }

        private async void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await OpenNewFile(string.Empty);
        }

        private async Task<XElement> OpenNewFile(string fileName)
        {
            ResetStatus();

            var root = string.IsNullOrWhiteSpace(fileName) ? PromptForFileAndOpenIt() : OpenSpecifiedFile(fileName);

            if (root == null)
            {
                return null;
            }

            ClearUiElements();

            ResetStatus();

            AdventureGameDesignerViewModel.CreateInstance(root.Element("AdventureGame").Value);
            AdventureGameDesignerViewModel.Instance.SetRootForExplorer();

            LoadDesigner(root, RoomDesigner);
            LoadDesigner(root, ObjectDesigner);

            ReapplyAllBindings();

            await SetUpValidationItems();

            InitialiseRooms();
            InitialisePlaceableObjects();
            InitialiseExits();

            SetUpDataContexts();

            SetupCommands(false);

            Title = "Adventure Game Designer (" + _lastFile + ")";

            SetDefaultPropertyGridData();

            ScriptExplorer.Reset(ScriptHeaders);

            SetProjectExplorerTabsToIndex(ProjectExplorerTabIndexes.Explorer);

            SetMainScreenTabsToIndex(MainScreenTabIndexes.GameDesigner);

            ResetEntityExplorer();

            PruneImageDirectory();

            return root;
        }

        private void ReapplyAllBindings()
        {
            var allItems = RoomDesigner.Children.OfType<DesignerItem>()
                .Union(ObjectDesigner.Children.OfType<DesignerItem>()).ToList();

            foreach (var item in RoomDesigner.Children.OfType<DesignerItem>())
            {
                ReapplyBindings(item, allItems);
            }

            foreach (var item in ObjectDesigner.Children.OfType<DesignerItem>())
            {
                ReapplyBindings(item, allItems);
            }
        }

        private void PruneImageDirectory()
        {
            var imageDirectoryInfo = new DirectoryInfo(Path.Combine(Options.Instance.ImageDirectory, AdventureGameDesignerViewModel.Instance.ControlId.ToString()));

            if (!imageDirectoryInfo.Exists)
            {
                return;
            }

            var files = imageDirectoryInfo.GetFiles();

            foreach (var file in files)
            {
                var controlId = Path.GetFileNameWithoutExtension(file.Name);

                var hasGuid = controlId.TryStrToGuid(out Guid guidValue);

                if (!hasGuid || AdventureGameDesignerViewModel.Instance.FindObjectByGuid(guidValue) == null)
                {
                    file.Delete();
                }
            }
        }

        private void InitialiseExits()
        {
            // Fixing up bad files - remove exits that don't exist in the designer
            foreach (var exitToDelete in AdventureGameDesignerViewModel.Instance.Exits.Where(exit =>
                RoomDesigner.Children.OfType<Connection>().All(item => item.ID != exit.ControlId)).ToList())
            {
                AdventureGameDesignerViewModel.Instance.Exits.Remove(exitToDelete);
            }
        }

        private void InitialisePlaceableObjects()
        {
            foreach (var placeableObject in AdventureGameDesignerViewModel.Instance.PlaceableObjects.Where(obj =>
                ObjectDesigner.Children.OfType<DesignerItem>().All(item => item.ID != obj.ControlId)).ToList())
            {
                AdventureGameDesignerViewModel.Instance.PlaceableObjects.Remove(placeableObject);
            }
        }

        private void SetDefaultPropertyGridData()
        {
            ShowGameProperties();

            OptionsProperties.SelectedObject = Options.Instance;
        }

        internal void CleanUpParentsAfterDelete()
        {
            var placeableItemsToCheck = AdventureGameDesignerViewModel.Instance.PlaceableObjects;

            //// DO NOT CHANGE THIS TO FOREACH ENUM ISSUES
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < placeableItemsToCheck.Count; index++)
            {
                var placeableObject = placeableItemsToCheck[index];
                // If this items parent (room or container) has been deleted remove it
                var o = placeableObject;
                if (AdventureGameDesignerViewModel.Instance.PlaceableObjects.All(parent =>
                        parent.ControlId != o.Parent?.ControlId) &&
                    AdventureGameDesignerViewModel.Instance.Rooms.All(parent =>
                        parent.ControlId != placeableObject.Parent?.ControlId))
                {
                    placeableObject.Parent = null;
                }
            }
        }

        internal void CleanUpChildrenAfterDelete()
        {
            var placeableItemsToCheck = AdventureGameDesignerViewModel.Instance.PlaceableObjects;

            foreach (var placeableObject in placeableItemsToCheck.OfType<Container>())
            {
                // Clean up child collections
                for (var i = placeableObject.PlaceableObjects.Count - 1; i == 0; i--)
                {
                    var childObject = placeableObject.PlaceableObjects[i];

                    if (AdventureGameDesignerViewModel.Instance.PlaceableObjects.All(obj =>
                        obj.ControlId != childObject.ControlId))
                    {
                        placeableObject.PlaceableObjects.Remove(childObject);
                    }
                }
            }
        }

        private void PlaceableObjectsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null) return;

            foreach (var newItem in e.NewItems)
            {
                var placeableObject = (PlaceableObject)newItem;

                if (placeableObject.Parent is Room roomToRefresh)
                {
                    roomToRefresh.RefreshPlaceableObject();
                }
                else if (placeableObject.Parent is Container containerToRefresh)
                {
                    containerToRefresh.RefreshPlaceableObject();
                }
                else if (placeableObject.Parent == null && e.Action == NotifyCollectionChangedAction.Replace)
                {
                    // Remove from any collections
                    var placeableItemsToCheck = AdventureGameDesignerViewModel.Instance.PlaceableObjects;

                    foreach (var container in AdventureGameDesignerViewModel.Instance.PlaceableObjects.OfType<Container>())
                    {
                        // Clean up missing parents from PlaceableObjects
                        var itemToRemove = container.PlaceableObjects
                            .FirstOrDefault(obj => obj.ControlId == placeableObject.ControlId);

                        if (itemToRemove != null)
                        {
                            container.PlaceableObjects.Remove(itemToRemove);
                        }
                    }

                    foreach (var room in AdventureGameDesignerViewModel.Instance.Rooms)
                    {
                        // Clean up missing parents from PlaceableObjects
                        var itemToRemove = room.PlaceableObjects
                            .FirstOrDefault(obj => obj.ControlId == placeableObject.ControlId);

                        if (itemToRemove != null)
                        {
                            room.PlaceableObjects.Remove(itemToRemove);
                        }
                    }
                }
            }
        }

        private void InitialiseRooms()
        {
            // Fixing up bad files - remove rooms that don't exist in the designer
            foreach (var roomToDelete in AdventureGameDesignerViewModel.Instance.Rooms.Where(room =>
                RoomDesigner.Children.OfType<DesignerItem>().All(item => item.ID != room.ControlId)).ToList())
            {
                AdventureGameDesignerViewModel.Instance.Rooms.Remove(roomToDelete);
            }

            // Do not convert causes error
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < AdventureGameDesignerViewModel.Instance.Rooms.Count; index++)
            {
                var nextRoom = AdventureGameDesignerViewModel.Instance.Rooms[index];
                nextRoom.SetNotifications(false);
                nextRoom.Initialize();
                nextRoom.SetNotifications(true);
            }
        }

        private static XElement SerializeDesigner(DesignerCanvas designerCanvas)
        {
            var designerItems = designerCanvas.Children.OfType<DesignerItem>();
            var connections = designerCanvas.Children.OfType<Connection>();

            var element = new XElement(designerCanvas.Name);

            var designerItemsXml = DesignerCanvas.SerializeDesignerItems(designerItems);
            element.Add(designerItemsXml);

            var connectionsXml = DesignerCanvas.SerializeConnections(connections);
            element.Add(connectionsXml);

            return element;
        }

        private void AddToRecentFiles(string saveFileFileName)
        {
            var recentFiles = Settings.Default.RecentFiles ?? new StringCollection();

            if (recentFiles.Contains(saveFileFileName))
            {
                recentFiles.Remove(saveFileFileName);
            }

            recentFiles.Insert(0, saveFileFileName);

            Settings.Default.RecentFiles = recentFiles;

            Settings.Default.Save();

            SetupRecentFileList();
        }

        private void SetupRecentFileList()
        {
            RecentFiles = new ObservableCollection<string>();

            if (Settings.Default.RecentFiles != null)
            {
                RecentFiles.AddRange(Settings.Default.RecentFiles.Cast<string>().Take(20).ToList());
            }
        }

        private void ProcessBackups(string rootBackupDirectory, string fileName)
        {
            if (Options.Instance.Backups > 0)
            {
                var directoryNameOnly = GetBackupDirName();

                var backupDirectory = Path.Combine(rootBackupDirectory, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                Directory.CreateDirectory(backupDirectory);

                PruneBackups(rootBackupDirectory);

                if (File.Exists(fileName))
                {
                    // Copy project file.
                    File.Copy(fileName, Path.Combine(backupDirectory, Path.GetFileName(fileName)));

                    // Copy Script Directory
                    FileHelper.CopyFolder(GetScriptPath(), Path.Combine(backupDirectory, "Scripts"));

                    // Copy Conversation Directory
                }
            }
        }

        private void PruneBackups(string rootBackupDirectory)
        {
            var dirInfo = new DirectoryInfo(rootBackupDirectory);

            var directoriesToDelete = dirInfo.GetDirectories().OrderByDescending(fi => fi.CreationTime).Skip(Options.Instance.Backups - 1).ToArray();

            foreach (var directory in directoriesToDelete)
            {
                FileHelper.DeleteDirectoryRecursive(directory.FullName);
            }
        }

        private void SaveFileHelper(XElement xElement, string fileName)
        {
            xElement.Save(fileName);

            _lastFile = fileName;

            Title = "Adventure Game Designer (" + fileName + ")";
        }

        public string GetGameDesign()
        {
            var gameDesignXml = GetDesignAsXElement();

            using (var gameDesignStream = new MemoryStream())
            {
                var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

                using (var writer = XmlWriter.Create(gameDesignStream, settings))
                {
                    gameDesignXml.WriteTo(writer);
                }

                gameDesignStream.Position = 0;

                using (var gameDesignStreamReader = new StreamReader(gameDesignStream))
                {
                    return gameDesignStreamReader.ReadToEnd();
                }
            }
        }

        private XElement GetDesignAsXElement()
        {
            var serializedRooms = SerializeDesigner(RoomDesigner);
            var serializedObjects = SerializeDesigner(ObjectDesigner);
            var root = new XElement("Root");

            root.Add(serializedRooms);
            root.Add(serializedObjects);

            var adventureGameData = new XElement("AdventureGame");
            adventureGameData.Add(AdventureGameDesignerViewModel.SerializeToString());
            root.Add(adventureGameData);

            return root;
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var isSaveAs = e.Command == ApplicationCommands.SaveAs;

            SaveCurrentProject(isSaveAs);
        }

        public void SaveCurrentProject(bool isSaveAs)
        {
            try
            {
                SetStatus("Saving Project.....", Colors.Green, true);

                var xElement = GetDesignAsXElement();

                if (!string.IsNullOrWhiteSpace(_lastFile) && !isSaveAs)
                {
                    SaveFileHelper(xElement, _lastFile);
                    AddToRecentFiles(_lastFile);
                    SaveScriptsDirectory(_lastFile);
                    ProcessBackups(GetBackupDirPath(), _lastFile);
                    return;
                }

                var saveFile = new SaveFileDialog
                {
                    Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*",
                    RestoreDirectory = true,
                    InitialDirectory = AdventureGameDesignerViewModel.Instance.ProjectDirectory
                };

                if (saveFile.ShowDialog() == true)
                {
                    _lastFile = saveFile.FileName;
                    SaveFileHelper(xElement, saveFile.FileName);
                    AddToRecentFiles(saveFile.FileName);
                    SaveScriptsDirectory(_lastFile);
                    ProcessBackups(GetBackupDirPath(), _lastFile);
                    AdventureGameDesignerViewModel.Instance.ProjectDirectory = Path.GetDirectoryName(_lastFile);
                }

                ResetStatus();
            }
            finally
            {
                SetStatus("Save Complete", Colors.Green, false);
            }
        }

        public string GetProjectSupportDirName()
        {
            return string.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(_lastFile))
                ? null
                : $"{Path.GetFileNameWithoutExtension(_lastFile)}-Files"; ;
        }

        public string GetBackupDirName()
        {
            return string.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(_lastFile))
                ? null
                : $"{Path.GetFileNameWithoutExtension(_lastFile)}-Backups";
        }

        public string GetBackupDirPath()
        {
            return Path.Combine(Path.GetDirectoryName(_lastFile), GetBackupDirName());
        }

        private (string controlDirectory, string commonDirectory, string roomDirectory, string objectDirectory, string exitDirectory, string commandDirectory) GetScriptDirectories()
        {
            var scriptDirectoryRoot = GetScriptPath();
            var controlDirectory = Path.Combine(scriptDirectoryRoot, "Control");
            var commonDirectory = Path.Combine(scriptDirectoryRoot, "Common");
            var roomDirectory = Path.Combine(scriptDirectoryRoot, "Room");
            var objectDirectory = Path.Combine(scriptDirectoryRoot, "Object");
            var exitDirectory = Path.Combine(scriptDirectoryRoot, "Exit");
            var commandDirectory = Path.Combine(scriptDirectoryRoot, "Command");

            return (controlDirectory, commonDirectory, roomDirectory, objectDirectory, exitDirectory, commandDirectory);
        }

        private string GetScriptPath()
        {
            return Path.Combine(AdventureGameDesignerViewModel.Instance.ProjectDirectory, $"{GetProjectSupportDirName()}", "Scripts");
        }

        private void SaveScriptsDirectory(string lastFile)
        {
            var directories = GetScriptDirectories();

            FileHelper.EnsureDirectories(directories.commonDirectory, directories.commandDirectory,
                directories.controlDirectory, directories.exitDirectory, directories.objectDirectory,
                directories.roomDirectory);

            FileHelper.SaveDataFile(Path.Combine(directories.controlDirectory, "GameInitCode.py"), AdventureGameDesignerViewModel.Instance.InitialisationCode.Source);
            FileHelper.SaveDataFile(Path.Combine(directories.controlDirectory, "GameLoopPreProcess.py"), AdventureGameDesignerViewModel.Instance.GameLoopPreProcessCode.Source);
            FileHelper.SaveDataFile(Path.Combine(directories.controlDirectory, "GameLoopPostProcess.py"), AdventureGameDesignerViewModel.Instance.GameLoopPostProcessCode.Source);
            FileHelper.SaveDataFile(Path.Combine(directories.commonDirectory, "CommonCode.py"), AdventureGameDesignerViewModel.Instance.CommonCode.Source);

            foreach (var command in AdventureGameDesignerViewModel.Instance.AdventureCommandMappings)
            {
                FileHelper.SaveDataFile(Path.Combine(directories.commandDirectory, $"{command.VerbName}.py"), command.ScriptCommand.Source);
            }

            foreach (var room in AdventureGameDesignerViewModel.Instance.Rooms)
            {
                FileHelper.SaveDataFile(Path.Combine(directories.roomDirectory, $"{room.BaseName}.py"), room.ObjectPreprocessScript.Source);
            }

            foreach (var exit in AdventureGameDesignerViewModel.Instance.Exits)
            {
                FileHelper.SaveDataFile(Path.Combine(directories.exitDirectory, $"{exit}.py"), exit.ObjectPreprocessScript.Source);
            }

            foreach (var placeableObject in AdventureGameDesignerViewModel.Instance.PlaceableObjects)
            {
                FileHelper.SaveDataFile(Path.Combine(directories.objectDirectory, $"{placeableObject.BaseName}.py"), placeableObject.ObjectPreprocessScript.Source);
            }
        }

        private void PrintExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResetStatus();

            RoomDesigner.SelectionService.ClearSelection();
            ObjectDesigner.SelectionService.ClearSelection();

            var printDialog = new PrintDialog();

            if (printDialog.ShowDialog() != true)
            {
                return;
            }

            PrintFitToPage(RoomDesigner, printDialog, "Rooms");
            PrintFitToPage(ObjectDesigner, printDialog, "Objects");

            // TODO Print out generated code and object definitions
        }

        private void PrintFitToPage(FrameworkElement designerCanvas, PrintDialog printDialog, string title)
        {
            var capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

            //get scale of the print wrt to screen of WPF visual
            var scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / designerCanvas.ActualWidth,
                capabilities.PageImageableArea.ExtentHeight / ActualHeight);

            // Preserve original transform
            var originalTransform = designerCanvas.LayoutTransform;

            //Transform the Visual to scale
            designerCanvas.LayoutTransform = new ScaleTransform(scale, scale);

            //get the size of the printer page
            var sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

            //update the layout of the visual to the printer page size.
            designerCanvas.Measure(sz);
            designerCanvas.Arrange(new Rect(
                new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight),
                sz));

            //now print the visual to printer to fit on the one page.
            printDialog.PrintVisual(designerCanvas, title);
            designerCanvas.LayoutTransform = originalTransform;
        }

        private void CmdZoomRoomsDefaultClick(object sender, RoutedEventArgs e)
        {
            SetRoomDesignerToDefaultScale();
        }

        private void SetRoomDesignerToDefaultScale()
        {
            RoomScale = 1.0f;
        }

        private void CmdZoomObjectsDefaultClick(object sender, RoutedEventArgs e)
        {
            SetObjectDesignerToDefaultScale();

        }

        private void SetObjectDesignerToDefaultScale()
        {
            ObjectScale = 1.0f;
        }

        public void ClearPropertiesOfObject(Guid objectId)
        {
            var adventureObject = GameProperties.SelectedObject as AdventureObjectBase;

            if (adventureObject?.ControlId == objectId)
            {
                ShowGameProperties();
            }

            if (adventureObject?.ControlId == CurrentRoom?.ControlId)
            {
                CurrentRoom = null;
                CurrentContainer = null;
            }
        }

        public bool ShowGameProperties()
        {
            GameProperties.SelectedObject = null;

            GameProperties.SelectedObject = AdventureGameDesignerViewModel.Instance;
            SetProjectExplorerTabsToIndex(ProjectExplorerTabIndexes.Explorer);
            return true;
        }

        public void ShowErrors()
        {
            SetProjectExplorerTabsToIndex(ProjectExplorerTabIndexes.Errors);
        }

        public bool ShowOptions()
        {
            SetUpOptions();

            return true;
        }

        public void SetUpOptions()
        {
            SetProjectExplorerTabsToIndex(ProjectExplorerTabIndexes.Options);
        }

        public void SetProjectExplorerTabsToIndex(ProjectExplorerTabIndexes tabId)
        {
            ProjectExplorerTab.SelectedIndex = (int)tabId;
        }

        public void SetMainScreenTabsToIndex(MainScreenTabIndexes tabId)
        {
            MainScreenTabs.SelectedIndex = (int)tabId;
        }

        private async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            await Start(true);
            ScriptExplorer.Reset(ScriptHeaders);
        }

        private void ExplorerSelected(object sender, RoutedEventArgs e)
        {
            if (!(Explorer.SelectedItem is AdventureObjectBase adventureObject) || DisableExplorer)
            {
                return;
            }

            SetMainScreenTabsToIndex(MainScreenTabIndexes.GameDesigner);

            if (Explorer.SelectedItem is Room)
            {
                ShowRoom(adventureObject.ControlId, true);
            }

            if (Explorer.SelectedItem is PlaceableObject)
            {
                ShowPlaceableObject(adventureObject.ControlId);

                var placeableObject = Explorer.SelectedItem as PlaceableObject;

                if (placeableObject.Parent != null)
                {
                    ShowRoom(placeableObject.Parent.ControlId, false);
                }
            }

            if (Explorer.SelectedItem is Exit)
            {
                ShowExit(adventureObject.ControlId);
            }
        }

        public void MainWindowClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AdventureDesignerOnClosing(object sender, CancelEventArgs e)
        {
            if (Application.Current.Windows.OfType<ConversationDesigner.ConversationDesigner>().Any())
            {
                TaskDialogService.ShowError(this,"The application cannot terminate because one ore more conversation windows are open. Pease close them and try again.", null);
                e.Cancel = true;
            }

            if (Options.Instance.AutoSave)
            {
                SaveCurrentProject(false);
                return;
            }

            var result = TaskDialogService.AskQuestion(this, "Are you sure you want to quit without Saving?",
                "Don't show me this message again (switches on Auto Save)",
                new[] { "&Yes", "&No" });

            if (result.VerificationChecked == true)
            {
                Options.Instance.AutoSave = true;
            }

            if (result.CustomButtonResult == 1)
            {
                e.Cancel = true;
            }
        }

        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            if (RoomScale < 0.05f)
            {
                return;
            }

            RoomScale *= 0.95f;
        }

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
            if (RoomScale > 3f)
            {
                return;
            }

            RoomScale *= 1.05f;
        }

        private void ZoomToFit(object sender, RoutedEventArgs e)
        {
            SetRoomDesignerToDefaultScale();

            var scale = ResizeToFit(RoomDesigner, ScrollRooms);

            RoomScale = scale.Value;

            ScrollRooms.InvalidateScrollInfo();
        }

        private double? ResizeToFit(DesignerCanvas canvas, ScrollViewer viewer)
        {
            double widthScale = 0;
            double heightScale = 0;
            var scale = 1.0;

            canvas.UpdateLayout();

            if ((canvas.ActualWidth > (viewer.ViewportWidth - MarginSize)) ||
                (canvas.ActualHeight > (viewer.ViewportHeight - MarginSize)))
            {
                widthScale = (viewer.ViewportWidth - MarginSize) / canvas.ActualWidth;
                heightScale = (viewer.ViewportHeight - MarginSize) / canvas.ActualHeight;

                scale = Math.Min(widthScale, heightScale);
            }

            return scale;
        }

        private void ObjectZoomInClick(object sender, RoutedEventArgs e)
        {
            if (ObjectScale < 0.05f)
            {
                return;
            }

            ObjectScale *= 0.95f;
        }

        private void ObjectZoomOutClick(object sender, RoutedEventArgs e)
        {
            if (ObjectScale > 3f)
            {
                return;
            }

            ObjectScale *= 1.05f;
        }

        private void CmdZoomObjectDefaultClick(object sender, RoutedEventArgs e)
        {
            ObjectScale = 1.0f;
        }

        private void ZoomObjectsToFit(object sender, RoutedEventArgs e)
        {
            SetObjectDesignerToDefaultScale();

            var scale = ResizeToFit(ObjectDesigner, ScrollObjects);

            ObjectScale = scale.Value;

            ScrollObjects.InvalidateScrollInfo();
        }

        public void RemoveParentBinding(PlaceableObject placeableObject)
        {
            var designerItem = ObjectDesigner.Children.OfType<DesignerItem>().FirstOrDefault(item => item.ID == placeableObject.ControlId);

            if (designerItem == null)
            {
                return;
            }

            StencilHelpers.RemoveParentBindingFromObject(designerItem.Content as Grid, placeableObject);
        }

        public void RefreshParentBinding(PlaceableObject placeableObject)
        {
            var designerItem = ObjectDesigner.Children.OfType<DesignerItem>().FirstOrDefault(item => item.ID == placeableObject.ControlId);

            if (designerItem == null)
            {
                return;
            }

            var parentDesigner = placeableObject.Parent is Room ? RoomDesigner : ObjectDesigner;

            var parentGrid = parentDesigner.Children.OfType<DesignerItem>()
                .FirstOrDefault(desItem => desItem.ID == placeableObject.Parent.ControlId).Content as Grid;

            StencilHelpers.BindParentToObject(designerItem.Content as Grid, parentGrid, placeableObject,
                placeableObject.Parent);
        }

        public void SetStatus(string text, Color color, bool spinnerState)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (spinnerState)
                {
                    ((Storyboard)FindResource("WaitStoryboard")).Begin();
                    WaitSpinner.Visibility = Visibility.Visible;
                }
                else
                {
                    ((Storyboard)FindResource("WaitStoryboard")).Stop();
                    WaitSpinner.Visibility = Visibility.Collapsed;
                }

                Status.Text = text;
                Status.Foreground = new SolidColorBrush(color);
            });
        }

        private void CmdExpandClick(object sender, RoutedEventArgs e)
        {
            Explorer.Expand(true);
        }

        private void CmdCollapseClick(object sender, RoutedEventArgs e)
        {
            Explorer.Expand(false);
        }

        private void MainScreenTabsOnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == (int)MainScreenTabIndexes.ScriptDesigner)
            {
                ScriptExplorer.SetupExplorer();
            }
        }

        private Size GetSnapGridSize()
        {
            var pointBrush = ObjectDesigner.Background as VisualBrush;

            return new Size(pointBrush.Viewbox.Width, pointBrush.Viewbox.Height);
        }

        private void ArrangeObjects(List<DesignerItem> designerItems, ScrollViewer viewer, DesignerCanvas canvas)
        {
            SetObjectDesignerToDefaultScale();

            canvas.UpdateLayout();

            var snapSize = GetSnapGridSize();

            var dimension = snapSize.Width * 2;
            var xSpacing = snapSize.Width;
            var ySpacing = snapSize.Height;

            var numberOfItemsInRow = (int)((viewer.ViewportWidth - MarginSize) / (dimension + xSpacing));

            var x = xSpacing;
            var y = ySpacing;

            for (var i = 0; i < designerItems.Count; i++)
            {
                designerItems[i].Width = dimension;
                designerItems[i].Height = dimension;

                Canvas.SetLeft(designerItems[i], x);
                Canvas.SetTop(designerItems[i], y);

                x += xSpacing;
                x += dimension;

                if ((i + 1) % numberOfItemsInRow == 0 && (i + 1) >= numberOfItemsInRow)
                {
                    x = xSpacing;
                    y += (ySpacing);
                    y += (int)dimension;
                }
            }

            canvas.UpdateLayout();
        }

        public void SetGrid(bool showGrid)
        {
            var gridBrush = Application.Current.Resources["SnappingGridBrush_Ponits"] as VisualBrush;

            var rectangle = gridBrush.Visual as Border;

            if (showGrid)
            {
                rectangle.Visibility = Visibility.Visible;
            }
            else
            {
                rectangle.Visibility = Visibility.Hidden;
            }
        }

        public void SetGridSize(int gridSize)
        {
            var gridBrush = Application.Current.Resources["SnappingGridBrush_Ponits"] as VisualBrush;

            gridBrush.Viewbox = new Rect(new Size(gridSize, gridSize));
            gridBrush.Viewport = new Rect(new Size(gridSize, gridSize));

            var rectangle = gridBrush.Visual as Border;
            rectangle.Width = gridSize;
            rectangle.Height = gridSize;
        }

        private void ArrangeAllRoomObjectsClick(object sender, RoutedEventArgs e)
        {
            ArrangeObjects(RoomDesigner.Children.OfType<DesignerItem>().ToList(), ScrollRooms, RoomDesigner);
        }

        private void ArrangeAllObjectsClick(object sender, RoutedEventArgs e)
        {
            ArrangeObjects(ObjectDesigner.Children.OfType<DesignerItem>().ToList(), ScrollObjects, ObjectDesigner);
        }


        private void SortRooms(object sender, RoutedEventArgs e)
        {
            SetRoomDesignerToDefaultScale();

            List<DesignerItem> designerItems;

            if (SortRoomByNameImage.Icon == FontAwesomeIcon.SortAlphaAsc)
            {
                designerItems = RoomDesigner.Children.OfType<DesignerItem>().OrderBy(item => AdventureGameDesignerViewModel
                    .Instance.Rooms.FirstOrDefault(po =>
                        po.ControlId == item.ID)
                    ?.BaseName).ToList();

                SortRoomByNameImage.Icon = FontAwesomeIcon.SortAlphaDesc;
            }
            else
            {
                designerItems = RoomDesigner.Children.OfType<DesignerItem>().OrderByDescending(item =>
                    AdventureGameDesignerViewModel
                        .Instance.Rooms.FirstOrDefault(po =>
                            po.ControlId == item.ID)
                        ?.BaseName).ToList();

                SortRoomByNameImage.Icon = FontAwesomeIcon.SortAlphaAsc;
            }

            ArrangeObjects(designerItems.ToList(), ScrollRooms, RoomDesigner);
        }

        private void SortObjects(object sender, RoutedEventArgs e)
        {
            SetObjectDesignerToDefaultScale();

            List<DesignerItem> designerItems;

            if (SortByNameImage.Icon == FontAwesomeIcon.SortAlphaAsc)
            {
                designerItems = ObjectDesigner.Children.OfType<DesignerItem>().OrderBy(item => AdventureGameDesignerViewModel
                    .Instance.PlaceableObjects.FirstOrDefault(po =>
                        po.ControlId == item.ID)
                    ?.BaseName).ToList();

                SortByNameImage.Icon = FontAwesomeIcon.SortAlphaDesc;
            }
            else
            {
                designerItems = ObjectDesigner.Children.OfType<DesignerItem>().OrderByDescending(item =>
                    AdventureGameDesignerViewModel
                        .Instance.PlaceableObjects.FirstOrDefault(po =>
                            po.ControlId == item.ID)
                        ?.BaseName).ToList();

                SortByNameImage.Icon = FontAwesomeIcon.SortAlphaAsc;
            }

            ArrangeObjects(designerItems.ToList(), ScrollObjects, ObjectDesigner);
        }

        private void SortObjectsByName(object sender, RoutedEventArgs e)
        {
            SetObjectDesignerToDefaultScale();

            List<DesignerItem> designerItems;

            if (SortByRoomNameImage.Icon == FontAwesomeIcon.SortAlphaAsc)
            {
                designerItems = ObjectDesigner.Children.OfType<DesignerItem>().OrderBy(item => AdventureGameDesignerViewModel
                    .Instance.PlaceableObjects.FirstOrDefault(po => po.ControlId == item.ID)?.Parent?.BaseName).ThenBy(
                    item => AdventureGameDesignerViewModel
                        .Instance.PlaceableObjects.FirstOrDefault(po => po.ControlId == item.ID) is Container).ToList();

                SortByRoomNameImage.Icon = FontAwesomeIcon.SortAlphaDesc;
            }
            else
            {
                designerItems = ObjectDesigner.Children.OfType<DesignerItem>().OrderByDescending(item =>
                        AdventureGameDesignerViewModel
                            .Instance.PlaceableObjects.FirstOrDefault(po => po.ControlId == item.ID)?.Parent?.BaseName)
                    .ThenByDescending(item => AdventureGameDesignerViewModel
                        .Instance.PlaceableObjects.FirstOrDefault(po => po.ControlId == item.ID) is Container).ToList();

                SortByRoomNameImage.Icon = FontAwesomeIcon.SortAlphaAsc;
            }

            ArrangeObjects(designerItems.ToList(), ScrollObjects, ObjectDesigner);
        }

        private async void MenuItemOnClick(object sender, RoutedEventArgs e)
        {
            var fileName = ((DropDownMenuItem)sender).Header.ToString();

            var file = await OpenNewFile(fileName);

            if (file == null)
            {
                RecentFiles.Remove(fileName);
            }
        }

        private async void RecentFilesItemOnClick(object sender, RoutedEventArgs e)
        {
            var fileName = ((MenuItem)e.OriginalSource).Header.ToString();

            var file = await OpenNewFile(fileName);

            if (file == null)
            {
                RecentFiles.Remove(fileName);
            }
        }

        private void AddNewCommand(object sender, RoutedEventArgs e)
        {
            var adventureCommand = new AdventureCommandMapping { IsEnabled = true, IsBuiltInCommand = false, VerbName = "<COMMAND>" };

            AdventureGameDesignerViewModel.Instance.SelectedCommand = adventureCommand;

            AdventureGameDesignerViewModel.Instance.AdventureCommandMappings.Add(adventureCommand);

            OnPropertyChanged(nameof(SelectedCommand));
        }

        private void DeleteCommand(object sender, RoutedEventArgs e)
        {
            AdventureGameDesignerViewModel.Instance.AdventureCommandMappings.Remove(AdventureGameDesignerViewModel.Instance.SelectedCommand);
            AdventureGameDesignerViewModel.Instance.SelectedCommand = null;
        }

        private void ResetCommand(object sender, RoutedEventArgs e)
        {
            if (Options.Instance.WarnOnReset)
            {
                var result = TaskDialogService.AskQuestion(this,
                    "This will reset the current command back to its initial setting, all changes will be lost. Are you sure you want to continue?",
                    "Don't show me this message again",
                    new[] { "&Yes", "&No" });


                if (result.VerificationChecked == true)
                {
                    Options.Instance.WarnOnReset = false;
                }

                if (result.CustomButtonResult == 1)
                {
                    return;
                }
            }

            AdventureGameDesignerViewModel.Instance.ResetCommand(AdventureGameDesignerViewModel.Instance.SelectedCommand);
        }

        private async void ValidationGridClick(object sender, MouseButtonEventArgs e)
        {
            var itemFound = false;

            switch (((ValidationItemAttribute)ErrorList.SelectedItem).ValidationCategory)
            {
                case ValidationCategories.Game:
                case ValidationCategories.Option:
                    var validationItem = (ValidationItemAttribute)ErrorList.SelectedItem;
                    itemFound = validationItem.Action(null);
                    break;

                case ValidationCategories.Object:
                case ValidationCategories.Room:
                case ValidationCategories.Exit:
                case ValidationCategories.Command:
                case ValidationCategories.Conversation:
                    var multipleValidationItem = (ValidationItemAttribute)ErrorList.SelectedItem;
                    itemFound = multipleValidationItem.Action(multipleValidationItem.ControlId);
                    break;
            }

            if (!itemFound)
            {
                await SetUpValidationItems();
            }
            else
            {
                ErrorList.SelectedIndex = -1;
            }
        }

        private void InitialiseCommands(bool isNewProject)
        {
            // Reference the ones just loaded in the existing project
            if (isNewProject)
            {
                EnsureCommandMappings();
            }
            else
            {
                _disableRefresh = true;
                _adventureCommandMappings.Clear();
            }

            _disableRefresh = false;

            ReactiveList<AdventureCommandMapping> actualMappingsToLoad;

            if (isNewProject)
            {
                actualMappingsToLoad = new ReactiveList<AdventureCommandMapping>(CommandServices
                    .GetAdventureCommandMappings()
                    .OrderBy(acm => acm.VerbName).ToList());
            }
            else
            {
                actualMappingsToLoad = AdventureGameDesignerViewModel.Instance.AdventureCommandMappings;
            }

            AdventureCommandMappings.Clear();
            AdventureCommandMappings.AddRange(actualMappingsToLoad);

            AdventureGameDesignerViewModel.Instance.AdventureCommandMappings = AdventureCommandMappings;
            AdventureGameDesignerViewModel.Instance.BuildScriptObjects();

            AdventureCommandMappings.ChangeTrackingEnabled = true;
        }

        private void EnsureCommandMappings()
        {
            if (AdventureCommandMappings == null)
            {
                AdventureCommandMappings = new ReactiveList<AdventureCommandMapping> { ChangeTrackingEnabled = false };

                AdventureCommandMappings.Changed.Subscribe(item =>
                {
                    if (!_disableRefresh)
                    {
                        AdventureGameDesignerViewModel.Instance.AdventureCommandMappings = AdventureCommandMappings;
                    }

                    AdventureGameDesignerViewModel.Instance.BuildScriptObjects();
                });

                AdventureCommandMappings.ItemChanged.Subscribe(item =>
                {
                    AdventureCommandMappings.Remove(item.Sender);
                    AdventureCommandMappings.Add(item.Sender);
                    SelectedCommand = item.Sender;

                    AdventureGameDesignerViewModel.Instance.AdventureCommandMappings = AdventureCommandMappings;

                    if (item.PropertyName == "VerbName")
                    {
                        AdventureGameDesignerViewModel.Instance.BuildScriptObjects();
                    }
                });
            }
        }

        private void ClientDropDownClicked(object sender, RoutedEventArgs e)
        {
            foreach (var nextClient in Clients)
            {
                nextClient.IsChecked = false;
            }

            var client = (Client)((DropDownMenuItem)sender).DataContext;
            client.IsChecked = true;
            SelectedClient = client;
        }

        private async void RunMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            if ((((MenuItem)e.OriginalSource).Header) is Client client)
            {
                await RunExecutedHelper(client.ClientType);
            }
        }

        private void NewDropDownClicked(object sender, RoutedEventArgs e)
        {
            foreach (var nextOption in NewOptions)
            {
                nextOption.IsChecked = false;
            }

            var option = (DropDownItem)((DropDownMenuItem)sender).DataContext;
            option.IsChecked = true;
            SelectedOption = option;
        }

        public void ShowEntityExplorer(DesignerItem itemClicked)
        {
            var adventureObject = AdventureGameDesignerViewModel.Instance.FindObjectByGuid(itemClicked.ID);

            if (adventureObject is Room room)
            {
                CurrentRoom = room;
                AddRoomHistory();
                CurrentContainer = null;
            }

            if (adventureObject is PlaceableObject placeableObject)
            {
                CurrentRoom = placeableObject.GetParentRoom();
                AddRoomHistory();
                CurrentContainer = null;
            }

            if (adventureObject is Container container)
            {
                CurrentContainer = container;
            }

            SetMainScreenTabsToIndex(MainScreenTabIndexes.EntityExplorer);
        }

        private void ResetEntityExplorer()
        {
            CurrentRoomHistoryIndex = 0;

            CurrentRoom = AdventureGameDesignerViewModel.Instance.Rooms.Count > 1 ?
                AdventureGameDesignerViewModel.Instance.Rooms[CurrentRoomHistoryIndex] : null;

            CurrentContainer = null;
        }

        public void RefreshEntityExplorer()
        {
            if (AdventureGameDesignerViewModel.Instance.Rooms.Any(room => CurrentRoom?.ControlId == room.ControlId))
            {
                CurrentRoom = CurrentRoom;

                CurrentContainer = CurrentContainer;
            }
            else if (AdventureGameDesignerViewModel.Instance.Rooms.Count > 0)
            {
                CurrentRoomHistoryIndex = 0;
                CurrentRoom = AdventureGameDesignerViewModel.Instance.Rooms[0];
                CurrentContainer = null;
            }
            else
            {
                CurrentRoomHistoryIndex = 0;
                CurrentRoom = null;
                CurrentContainer = null;
            }
        }
    }
}
