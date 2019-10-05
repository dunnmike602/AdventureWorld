using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Services;
using AdventureLandCore.Services.Data;
using DiagramDesigner.AdventureWorld.CustomCollections;
using DiagramDesigner.AdventureWorld.Domain.Interfaces;
using DiagramDesigner.AdventureWorld.Domain.Mappers;
using DiagramDesigner.AdventureWorld.Extensions;
using DiagramDesigner.Controls;
using DiagramDesigner.Helpers;
using DiagramDesigner.Services;
using ReactiveUI;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract]
    [DefaultProperty("Details")]
    [DisplayName("Game Details")]
    [CategoryOrder("Messages", 1)]
    [CategoryOrder("Debugging", 2)]
    [CategoryOrder("Publishing", 3)]
    [CategoryOrder("Feature Toggles", 4)]
    [CategoryOrder("Project", 6)]
    [CategoryOrder("Startup", 7)]
    [CategoryOrder("Scripts", 8)]
    [CategoryOrder("Language Processing", 9)]
  
    public class AdventureGameDesignerViewModel : IdentifableObjectBase, IValidationFilter
    {
        [Browsable(false)]
        public ObservableCollection<ScriptContainer> ScriptContainers { get; set; }

        private static AdventureGameDesignerViewModel _instance = new AdventureGameDesignerViewModel();
        private Room _startRoom;
        private string _gameDescription;
        private string _gameName;
        private string _title;
        private string _introduction;
        private string _wonGame;
        private string _playerLostMessage;
        private string _anotherGameText;
        private string _anotherGameYesResponse;
        private Script _initialisationCode;
        private Script _gameLoopPreProcessCode;
        private Script _gameLoopPostProcessCode;
        private Script _commonCode;
        private int _maximumScore;
        private int _maximumInventorySize;
        private string _stopWords;
        private string _projectDirectory;
        private bool _enableTitles = true;
        private bool _enableScore = true;
        private bool _enableShowItemsInRoom = true;
        private bool _enableInventorySize = true;
        private bool _enableExitDescriptions;
        private bool _enablePlayerLost = true;
        private ReactiveList<AdventureCommandMapping> _adventureCommandMappings;
        private ObservableItemCollection<PlaceableObject> _placeableObjects;
        private ObservableItemCollection<Exit> _exits;
        private ObservableItemCollection<Room> _rooms;
        private string _commandPromptText = GlobalConstants.DefaultPrompt;
        private AdventureCommandMapping _selectedCommand;
        private Dictionary<int, Direction> _directionMappings = LanguageHelper.GetDirections();
        private bool _enableDebug = true;
        private bool _logGameConsole;
        private string _gameLogPath;
        private string _gameLogFileName;
        private bool _showImages = true;
        private string _noItemsInRoomText;

        [Browsable(false)]
        public AdventureCommandMapping SelectedCommand
        {
            get => (_selectedCommand);
            set
            {
                _selectedCommand = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public ReactiveList<GameRoot> GameRootList { get; set; }

        [DataMember]
        [Browsable(false)]
        public ReactiveList<AdventureCommandMapping> AdventureCommandMappings
        {
            get => _adventureCommandMappings;
            set
            {

                _adventureCommandMappings = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public List<AdventureCommandMapping> GetActiveAdventureCommandMappings()
        {
            return AdventureCommandMappings?.Where(cmd => cmd.IsEnabled).ToList() ??
                   new List<AdventureCommandMapping>();
        }
        
        [DataMember, PropertyOrder(4), DisplayName("Command Prompt Text"), ValidationItem(
             Description = "You must specify a Command Prompt for your game",
             Severity = Severities.Error, ValidationType = ValidationTypes.MissingCommandPromptText), NotNull, Category("Messages"),
         Description("Text that is displayed before the command input prompt when waiting for player input.")]
        public string CommandPromptText
        {
            get => _commandPromptText;
            set
            {
                _commandPromptText = value;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(5), DisplayName("No Items In Room Text"), ValidationItem(
             Description = "You must specify No Items In Room Text for your game.",
             Severity = Severities.Error, ValidationType = ValidationTypes.MissingNotItemsInRoomText), NotNull, Category("Messages"),
         Description("If DisplayItemsInRoom feature toggle is enabled this is the text to be displayed when there are no items visible.")]
        public string NoItemsInRoomText
        {
            get => _noItemsInRoomText;
            set
            {
                _noItemsInRoomText = value;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(1), Category("Feature Toggles"), DisplayName("Enable Titles"), Description("When True the specified Title and Introduction will be displayed at the start of the game.  Set to False to remove this feature.")]
        public bool EnableTitles
        {
            get => _enableTitles;
            set
            {
                _enableTitles = value;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(2), Category("Feature Toggles"), DisplayName("Enable Score"), Description("When True a Score will be displayed in the title bar, this score can be incremented in Scripts. Set to False to remove this feature and also disable the Won Game message.")]
        public bool EnableScore
        {
            get => _enableScore;
            set
            {
                _enableScore = value;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(3), Category("Feature Toggles"), DisplayName("Enable Show Items In Room"), Description("When True every time a player enters a new room, a list of visible items in it will be shown.")]
        public bool EnableShowItemsInRoom
        {
            get => _enableShowItemsInRoom;
            set
            {
                _enableShowItemsInRoom = value;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(4), Category("Feature Toggles"), DisplayName("Enable Maximum Inventory Size"), Description("When True the player can only carry the specified maximum inventory size.")]
        public bool EnableInventorySize
        {
            get => _enableInventorySize;
            set
            {
                _enableInventorySize = value;
                OnPropertyChanged();
            }
        }


        [DataMember, PropertyOrder(5), Category("Feature Toggles"), DisplayName("Enable Automatic Exit Description"),
         Description("When True, when a player moves through an exit, the currently exit description will be output to the screen.")]
        public bool EnableExitDescriptions
        {
            get => _enableExitDescriptions;
            set
            {
                _enableExitDescriptions = value;
                OnPropertyChanged();
            }
        }


        [DataMember, PropertyOrder(6), Category("Feature Toggles"), DisplayName("Enable Lost Game Message."),
         Description("When True the Player Lost message is automatically shown when the game is lost. If false this can be controlled by scripts.")]
        public bool EnablePlayerLost
        {
            get => _enablePlayerLost;
            set
            {
                _enablePlayerLost = value;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(1), DisplayName("Title"), ValidationItem(
             Description = "You must specify a title for your game",
             Severity = Severities.Error, ValidationType = ValidationTypes.TitleMissing), NotNull, Category("Messages"),
         Description("Title that will be displayed at the very start of a new game.")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public string Details => "Game Details";

        [DataMember, DisplayName("Introduction"), PropertyOrder(2), Editor(typeof(TextEditor), typeof(TextEditor)),
         ValidationItem(Description = "You must specify an introduction for your game",
             Severity = Severities.Error, ValidationType = ValidationTypes.IntroductionMissing), NotNull,
         Category("Messages"), Description("Introduction that will be displayed at the very start of a new game.")]
        public string Introduction
        {
            get => _introduction;
            set
            {
                _introduction = value;
                OnPropertyChanged();
            }
        }

        [DataMember, Category("Project"), DisplayName("Project Directory"), Editor(typeof(DirectoryEditor), typeof(DirectoryEditor)), 
         PropertyOrder(1), Description("Base location for game project and all associated assets.")]
        public string ProjectDirectory
        {
            get
            {
                var directory = string.IsNullOrWhiteSpace(_projectDirectory)
                    ? Options.Instance.LastProjectDirectory
                    : _projectDirectory;

                Directory.CreateDirectory(directory);

                return directory;
            }
            set
            {
                _projectDirectory = value;
                OnPropertyChanged();
            }
        }
        
        [Category("Project")]
        [PropertyOrder(4)]
        [DisplayName("Show Background Images")]
        [Description("Show or hide background images in designer items.")]
        [DataMember]
        public bool ShowImages
        {
            get => _showImages;
            set
            {
                _showImages = value;
                OnPropertyChanged();
            }
        }

        [DataMember, DisplayName("Won Game Message"), PropertyOrder(3), Editor(typeof(TextEditor), typeof(TextEditor)),
         ValidationItem(Description = "You must specify a won game message for your game",
             Severity = Severities.Error, ValidationType = ValidationTypes.WonGameMissing), NotNull,
         Category("Messages"), Description("Message that will be displayed when a game is won, provided that scoring is enabled.")]
        public string WonGame
        {
            get => _wonGame;
            set
            {
                _wonGame = value;
                OnPropertyChanged();
            }
        }

        [DataMember, DisplayName("Lost Game Message"), PropertyOrder(3), Editor(typeof(TextEditor), typeof(TextEditor)),
         ValidationItem(Description = "You must specify a player lost message for your game",
             Severity = Severities.Error, ValidationType = ValidationTypes.PlayerLostMissing), NotNull,
         Category("Messages"), Description("Message that will be displayed when a player has lost the game.")]
        public string PlayerLostMessage
        {
            get => _playerLostMessage;
            set
            {
                _playerLostMessage = value;
                OnPropertyChanged();
            }
        }

        [DataMember, DisplayName("Another Game Prompt Message"), PropertyOrder(4), Editor(typeof(TextEditor), typeof(TextEditor)),
         ValidationItem(Description = "You must specify a message for the Another Game prompt",
             Severity = Severities.Error, ValidationType = ValidationTypes.AnotherGameMissing), NotNull,
         Category("Messages"), Description("Message that will be displayed when a player is prompted for another game.")]
        public string AnotherGameText
        {
            get => _anotherGameText;
            set
            {
                _anotherGameText = value;
                OnPropertyChanged();
            }
        }

        [DataMember, DisplayName("Another Game Yes Response"), PropertyOrder(5), 
         ValidationItem(Description = "You must specify the Yes (positive) response for the 'Another Game' prompt.",
             Severity = Severities.Error, ValidationType = ValidationTypes.AnotherGameYesResponseMissing), NotNull,
         Category("Messages"), Description("To start a new game this is the text that must be input. For example Y or Yes.")]
        public string AnotherGameYesResponse
        {
            get => _anotherGameYesResponse;
            set
            {
                _anotherGameYesResponse = value;
                OnPropertyChanged();
            }
        }

        [PropertyOrder(2), DisplayName("Game Initialisation Script"), Editor(typeof(ScriptEditor), typeof(ScriptEditor)),
        DataMember,
        Category("Scripts"),
        Description("Script that is executed only once at the start of the game. Use this to perform your own game initialisation.")]
        public Script InitialisationCode
        {
            get => _initialisationCode;
            set
            {
                _initialisationCode = value;
                OnPropertyChanged();
            }
        }

        [PropertyOrder(2), DisplayName("Game Loop Pre Script"), Editor(typeof(ScriptEditor), typeof(ScriptEditor)),
         DataMember,
         Category("Scripts"),
         Description(
             "Script that is executed once at the start of the game loop before input has been requested and before any game engine processing has been done.")]
        public Script GameLoopPreProcessCode
        {
            get => _gameLoopPreProcessCode;
            set
            {
                _gameLoopPreProcessCode = value;
                OnPropertyChanged();
            }
        }

        [PropertyOrder(3), DisplayName("Game Loop Post Script"), Editor(typeof(ScriptEditor), typeof(ScriptEditor)),
         DataMember, Category("Scripts"),
         Description(
             "Script that is executed once at the end of the game loop after input has been requested and after all game engine processing.")]
        public Script GameLoopPostProcessCode
        {
            get => _gameLoopPostProcessCode;
            set
            {
                _gameLoopPostProcessCode = value;
                OnPropertyChanged();
            }
        }
        [PropertyOrder(4), DisplayName("Command Common Code"), Editor(typeof(ScriptEditor), typeof(ScriptEditor)),
         DataMember, Category("Scripts"),
         Description(
             "Common code for all commands is stored hear. Note that this is a Python Script only.")]
        public Script CommonCode
        {
            get => _commonCode;
            set
            {
                _commonCode = value;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(4), DisplayName("Maximum Score"), NoneZero, ValidationItem(
             Description = "You must specify a non-zero Maximum Score",
             Severity = Severities.Error, ValidationType = ValidationTypes.MaximumScoreNotSet),
          Description("The score that needs to be reached for the player to win the game.")]
        [Category("Startup")]
        public int MaximumScore
        {
            get => _maximumScore;
            set
            {
                if (value >= 0)
                {
                    _maximumScore = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember]
        [Browsable(false)]
        public Guid? StartRoomId { get; set; }
        
        [PropertyOrder(1)]
        [Category("Startup")]
        [DisplayName("Start Room")]
        [ItemsSource(typeof(RoomItemsSource))]
        [ValidationItem(Description = "You must specify a room to start the game in",
             Severity = Severities.Error, ValidationType = ValidationTypes.StartRoomNotSet)]
        [NotNull]
        [Description("Identifies a start location for the player when the game commences.")]
        public Room StartRoom
        {
            get => _startRoom;
            set
            {
                _startRoom = value;
                StartRoomId = _startRoom?.ControlId;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(6), DisplayName("Inventory Size"), ValidationItem(
             Description = "You must specify a non-zero Maximum Inventory Size",
             Severity = Severities.Error, ValidationType = ValidationTypes.InventorySizeNotSet)]
        [Description("Maximum number of objects that can be held by the player at any one time.")]
        [Category("Startup")]
        public int MaximumInventorySize
        {
            get => _maximumInventorySize;
            set
            {
                if (value >= 1)
                {
                    _maximumInventorySize = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember, DisplayName("Stop Words"), PropertyOrder(2), Category("Language Processing"), Editor(typeof(TextEditor), typeof(TextEditor))]
        [Description("List of Stop Words that are removed from player input. English defaults can be added to or replaced with other languages.")]
        public string StopWords
        {
            get => string.IsNullOrWhiteSpace(_stopWords) ? (_stopWords = LanguageHelper.GetStopWords()) : _stopWords;
            set
            {
                _stopWords = value;

                OnPropertyChanged();
            }
        }

        [DataMember, DisplayName("Direction Mappings"), PropertyOrder(2), Category("Language Processing"), Editor(typeof(CompassEditor), typeof(CompassEditor))]
        [Description("The directions that are used in the game for movement between locations. Each direction has an abbreviation and a full name for example S and SOUTH.")]
        public Dictionary<int, Direction> DirectionMappings
        {
            get => _directionMappings;
            set
            {
                _directionMappings = value ?? LanguageHelper.GetDirections();

                OnPropertyChanged();
            }
        }

        [DataMember, Browsable(false)]
        public ObservableItemCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                if (value != null)
                {
                    value.CollectionChanged -= CollectionChanged;
                }

                _rooms = value;

                if (_rooms != null)
                {
                    _rooms.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged();
            }
        }

        [DataMember]
        [Browsable(false)] public ObservableItemCollection<Exit> Exits
        {
            get => _exits;
            set
            {
                if (value != null)
                {
                    value.CollectionChanged -= CollectionChanged;
                }

                _exits = value;

                if (_exits != null)
                {
                    _exits.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged();
            }
        }

        [DataMember]
        [Browsable(false)]
        public ObservableItemCollection<PlaceableObject> PlaceableObjects
        {
            get => _placeableObjects;
            set
            {
                if (value != null)
                {
                    value.CollectionChanged -= CollectionChanged;
                }

                _placeableObjects = value;

                if (_placeableObjects != null)
                {
                    _placeableObjects.CollectionChanged += CollectionChanged;
                }

                OnPropertyChanged();
            }
        }
        
        [Browsable(false)]
        public List<Room> RoomsList => Rooms.ToList();

        [Browsable(false)]
        public List<Exit> ExitList => Exits.ToList();

        [Browsable(false)]
        public List<PlaceableObject> PlaceableObjectsList => PlaceableObjects.ToList();

        [DisplayName("Game Name")]
        [PropertyOrder(2)]
        [DataMember]
        [ValidationItem(Description = "You must specify a name for your game",
            Severity = Severities.Error, ValidationType = ValidationTypes.GameNameNotSet)]
        [NotNull]
        [Category("Publishing")]
        [Description("A publishing name for the game like a book name not used by the engine during play.")]
        public string GameName
        {
            get => _gameName;
            set
            {
                _gameName = value;
                OnPropertyChanged();
            }
        }

        [DisplayName("Game Description")]
        [PropertyOrder(3)]
        [Editor(typeof(TextEditor), typeof(TextEditor))]
        [DataMember]
        [ValidationItem(Description = "You must specify a description for your game",
            Severity = Severities.Error, ValidationType = ValidationTypes.GameDescriptionNotSet)]
        [NotNull]
        [Category("Publishing")]
        [Description("A publishing description for the game, for example a Store listing description.")]
        public string GameDescription
        {
            get => _gameDescription;
            set
            {
                _gameDescription = value;
                OnPropertyChanged();
            }
        }

        [DisplayName("Enable Debug Command")]
        [PropertyOrder(1)]
        [DataMember]
        [Category("Debugging")]
        [Description("Determines whether the published game will support debug commands. Can be switched off in release builds of the game, or enabled for troubleshooting purposes.")]
        public bool EnableDebug
        {
            get => _enableDebug;
            set
            {
                _enableDebug = value;
                OnPropertyChanged();
            }
        }


        [DisplayName("Log Game")]
        [PropertyOrder(2)]
        [DataMember]
        [Category("Debugging")]
        [Description("Determines whether the published game console will automatically be logged to the specified file. If true all commands and output will be logged.")]
        public bool LogGameConsole
        {
            get => _logGameConsole;
            set
            {
                _logGameConsole = value;
                OnPropertyChanged();
            }
        }

        [DisplayName("Game Log File Path")]
        [PropertyOrder(3)]
        [DataMember]
        [Category("Debugging")]
        [Editor(typeof(DirectoryEditor), typeof(DirectoryEditor))]
        [Description("If LogGameConsole this field specifies the path to use in which the console log will be created.")]
        public string GameLogPath
        {
            get
            {
                var directory = string.IsNullOrWhiteSpace(_gameLogPath)
                    ? Options.Instance.LastProjectDirectory
                    : _gameLogPath;

                Directory.CreateDirectory(directory);

                return directory;
            }
            set
            {
                _gameLogPath = value;
                OnPropertyChanged();
            }
        }

        [DisplayName("Game Log File Name")]
        [PropertyOrder(4)]
        [DataMember]
        [Category("Debugging")]
        [Description("If LogGameConsole this field specifies the file name to use for the console log.")]
        public string GameLogFileName
        {
            get => _gameLogFileName ??  $"{GameName}Log.txt";
            set
            {
                _gameLogFileName = value;
                OnPropertyChanged();
            }
        }

        private AdventureGameDesignerViewModel()
        {
            Rooms = new ObservableItemCollection<Room>();
            Exits = new ObservableItemCollection<Exit>();
            PlaceableObjects = new ObservableItemCollection<PlaceableObject>();

            _commonCode = new Script { Source = Assembly.GetExecutingAssembly().GetCommonCodeTemplate() };
            _initialisationCode = new Script { Source = Assembly.GetExecutingAssembly().GetControlCodeTemplate() };
            _gameLoopPostProcessCode = new Script { Source = Assembly.GetExecutingAssembly().GetControlCodeTemplate() };
            _gameLoopPreProcessCode = new Script { Source = Assembly.GetExecutingAssembly().GetControlCodeTemplate() };

            SetRootForExplorer();
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (GameRoot == null)
            {
                return;
            }

            GameRoot.PlaceableObjectsOnly.Clear();

            foreach (var placeableObject in PlaceableObjects.Where( obj => obj.GetType() == typeof(PlaceableObject)))
            {
                GameRoot.PlaceableObjectsOnly.Add(placeableObject);
            }

            GameRoot.ContainersOnly.Clear();

            foreach (var container in PlaceableObjects.Where(obj => obj.GetType() == typeof(Container)))
            {
                GameRoot.ContainersOnly.Add(container as Container);
            }

            GameRoot.NpcsOnly.Clear();

            foreach (var npc in PlaceableObjects.Where(obj => obj.GetType() == typeof(Npc)))
            {
                GameRoot.NpcsOnly.Add(npc as Npc);
            }

            BuildScriptObjects();

            AdventureDesigner.Instance.RefreshEntityExplorer();
        }

        internal void BuildScriptObjects()
        {
            EnsureScriptData();

            ScriptContainers.Add(new ScriptContainer { Name = "Game Initialisation", Script = InitialisationCode, Type = ScriptTypes.Control });
            ScriptContainers.Add(new ScriptContainer { Name = "Pre-process for Game Loop", Script = GameLoopPreProcessCode, Type = ScriptTypes.Control });
            ScriptContainers.Add(new ScriptContainer { Name = "Post-process for Game Loop", Script = GameLoopPostProcessCode, Type = ScriptTypes.Control });
            ScriptContainers.Add(new ScriptContainer { Name = "Command Library", Script = CommonCode, Type = ScriptTypes.Common });

            foreach (var room in Rooms)
            {
                ScriptContainers.Add(new ScriptContainer { Name = $"{room.BaseName} pre-process", Script = room.ObjectPreprocessScript, Type = ScriptTypes.Room });
            }

            foreach (var placeableObject in PlaceableObjects)
            {
                ScriptContainers.Add(new ScriptContainer { Name = $"{placeableObject.BaseName} pre-process", Script = placeableObject.ObjectPreprocessScript, Type = ScriptTypes.PlaceableObject });
            }

            foreach (var exit in Exits)
            {
                ScriptContainers.Add(new ScriptContainer { Name = $"{exit} pre-process", Script = exit.ObjectPreprocessScript, Type = ScriptTypes.Exit });
            }

            foreach (var commandMapping in AdventureCommandMappings)
            {
                ScriptContainers.Add(new ScriptContainer { Name = commandMapping.VerbName, Script = commandMapping.ScriptCommand, Type = ScriptTypes.Command });
            }

            MessageBus.Current.SendMessage(ScriptContainers, MessageBusContractConstants.MainScriptContract);
        }

        private void EnsureScriptData()
        {
            if (ScriptContainers == null)
            {
                ScriptContainers = new ObservableCollection<ScriptContainer>();
            }

            ScriptContainers.Clear();
        }
        
        public void ResetCommand(AdventureCommandMapping adventureCommandMapping)
        {
            var builtInCommand = CommandServices.GetAdventureCommandMappings()
                .First(acm => acm.ControlId == adventureCommandMapping.ControlId);

            adventureCommandMapping.CopyFrom(builtInCommand);
        }

        public void SetRootForExplorer()
        {
            GameRoot = new GameRoot
            {
                Rooms = Rooms,
                PlaceableObjects = PlaceableObjects,
                Exits = Exits,
                InitialisationCode = InitialisationCode,
                GameLoopPreProcessCode = GameLoopPreProcessCode,
                GameLoopPostProcessCode = GameLoopPostProcessCode,
                CommonCode = CommonCode,
                AdventureCommandMappings = AdventureCommandMappings,
            };

            GameRootList = new ReactiveList<GameRoot>() {GameRoot};
        }

        private AdventureGameSetup PopulateSummaryData()
        {
            return new AdventureGameSetup
            {
                Id = ControlId.ToString(),
                Title = Title,
                Introduction = Introduction,
                InventorySize = MaximumInventorySize,
                EnablePlayerLost = EnablePlayerLost,
                StopWords = StopWords.SplitString(),
                StartRoom = StartRoom?.BaseName,
                InitialisationCode = InitialisationCode,
                GameLoopPreProcessCode = GameLoopPreProcessCode,
                GameLoopPostProcessCode =  GameLoopPostProcessCode,
                CommonCode = CommonCode,
                MaximumScore = MaximumScore,
                WonGameMessage = WonGame,
                PlayerLostMessage = PlayerLostMessage,
                CommandPromptText = CommandPromptText,
                GameName = GameName,
                EnableTitles = EnableTitles,
                EnableScore = EnableScore,
                EnableShowItemsInRoom = EnableShowItemsInRoom,
                EnableDebug = EnableDebug,
                EnableInventorySize = EnableInventorySize,
                EnableExitDescriptions = EnableExitDescriptions,
                CommandMappings = PopulateMappingsHelper(GetActiveAdventureCommandMappings().ToList(), EnableScore),
                DirectionMappings =  DirectionMappings,
                AnotherGameYesResponse = AnotherGameYesResponse,
                AnotherGameText = AnotherGameText,
                ConsoleLogFile = LogGameConsole ? Path.Combine(GameLogPath, GameLogFileName) : null 

            };
        }

        private List<CommandMapping> PopulateMappingsHelper(List<AdventureCommandMapping> adventureCommandMappings,
            bool enableScore)
        {
            var commandMappings = new List<CommandMapping>();

            // If the score feature toggle is switched off the built-in score command is disabled automatically
            foreach (var adventureCommandMapping in adventureCommandMappings.Where(acm => (enableScore || !acm.IsBuiltInCommand || !acm.VerbName.IsEqualTo("SCORE"))))
            {
                commandMappings.Add(new CommandMapping
                {
                    VerbName = adventureCommandMapping.VerbName,
                    AliasList = adventureCommandMapping.AliasList.TextToList(),
                    ScriptCommand = adventureCommandMapping.ScriptCommand,
                    OneWordSubstitutionList = adventureCommandMapping.OneWordSubstitutionList.TextToList(),
                    HelpText = adventureCommandMapping.HelpText,
                });
            }

            return commandMappings;
        }

        private void PopulateRooms(AdventureGameSetup adventureGameSetup)
        {
            adventureGameSetup.Rooms = new List<AdventureLandCore.Domain.Room>(RoomsList.Select(room => RoomMapper.MapOne(room, ExitList)));
        }

        private void PopulateObjects(AdventureGameSetup adventureGameSetup)
        {
            adventureGameSetup.PlaceableObjects = new List<AdventureLandCore.Domain.PlaceableObject>();
           
            var placeableObjects = PlaceableObjectsList.Where(obj => obj.GetType() == typeof(PlaceableObject)).Select(PlaceableObjectMapper.MapOne).ToList();
            var containers = PlaceableObjectsList.Where(obj => obj.GetType() == typeof(Container)).OfType<Container>().Select(ContainerMapper.MapOne).ToList();
            var npcs = PlaceableObjectsList.Where(obj => obj.GetType() == typeof(Npc)).OfType<Npc>().Select(NpcMapper.MapOne).ToList();

            adventureGameSetup.PlaceableObjects.AddRange(placeableObjects);
            adventureGameSetup.PlaceableObjects.AddRange(containers);
            adventureGameSetup.PlaceableObjects.AddRange(npcs);
        }

        public static AdventureGameDesignerViewModel Instance => _instance;

        [Browsable(false)]
        public GameRoot GameRoot { get; private set; }
        
        public AdventureObjectBase FindObjectByGuid(Guid id)
        {
            AdventureObjectBase objectToFind = RoomsList.FirstOrDefault(m => m.ControlId == id);

            if (objectToFind != null)
            {
                return objectToFind;
            }

            objectToFind = ExitList.FirstOrDefault(m => m.ControlId == id);

            if (objectToFind != null)
            {
                return objectToFind;
            }

            objectToFind = PlaceableObjectsList.FirstOrDefault(m => m.ControlId == id);

            return objectToFind;
        }
    

        public string GetNextGenericName(string nameStub, IEnumerable<string> collectionToSearch)
        {
            var matchNameExpress = new Regex("^" + nameStub + @"(\d+)", RegexOptions.IgnoreCase);

            var queryMatching =
                (from baseName in collectionToSearch
                    let matchTerm = matchNameExpress.Match(baseName)
                    where matchTerm.Success
                    select Convert.ToInt32(matchTerm.Groups[1].Captures[0].Value)).ToArray();


            return nameStub + queryMatching.FindFirstMissingNumber();
        }

        public void DeleteRoomByGuid(Guid id)
        {
            var roomToDelete = Rooms.FirstOrDefault(m => m.ControlId == id);

            if (roomToDelete != null)
            {
                roomToDelete.Dispose();
                Rooms.Remove(roomToDelete);
            }
        }

        public void DeleteObjectByGuid(Guid id)
        {
            var objectToDelete = PlaceableObjects.FirstOrDefault(m => m.ControlId == id);

            if (objectToDelete != null)
            {
                objectToDelete.Dispose();
                PlaceableObjects.Remove(objectToDelete);
            }
        }

        public void DeleteExitByGuid(Guid id)
        {
            var exitToGuid = Exits.FirstOrDefault(m => m.ControlId == id);

            if (exitToGuid != null)
            {
                exitToGuid.Dispose();
                Exits.Remove(exitToGuid);
            }
        }
        
        public void Clear()
        {
            _instance = new AdventureGameDesignerViewModel();
        }

        public ObservableCollection<Exit> GetExitsForRoom(Guid roomId)
        {
            var exitList = Exits.Where(m => m.FromRoom.ControlId == roomId).ToList();
            return new ObservableCollection<Exit>(exitList);
        }

        public ObservableCollection<PlaceableObject> GetObjectsForRoom(Guid controlId)
        {
            var objectList = PlaceableObjects.Where(m => m.Parent != null && m.Parent.ControlId == controlId).ToList();

            return new ObservableCollection<PlaceableObject>(objectList);
        }

        public AdventureGameSetup Map()
        {
            var adventureGameSetup = PopulateSummaryData();
            PopulateObjects(adventureGameSetup);
            PopulateRooms(adventureGameSetup);

            // Convert the parent ids into proper objects
            foreach (var placeableObject in adventureGameSetup.PlaceableObjects.Where(go => go.ParentId != null))
            {
                placeableObject.Parent = GetParentObjectById(adventureGameSetup, placeableObject.ParentId.Value);
            }

            return adventureGameSetup;
        }

        private AdventureLandCore.Domain.AdventureObjectBase GetParentObjectById(AdventureGameSetup adventureGameSetup, Guid parentId)
        {
            var room = adventureGameSetup.Rooms.FirstOrDefault(rm => rm.Id == parentId);

            if (room != null)
            {
                return room;
            }

            return adventureGameSetup.PlaceableObjects.FirstOrDefault(rm => rm.Id == parentId);
        }

        public static string SerializeToString()
        {
            return SerializationExtensions.SerializeToString(_instance, new[] {typeof(Npc), typeof(PlaceableObject), typeof(Container), typeof(Room), typeof(AdventureObjectBase) });
        }

        public static void CreateInstance(string xmlData)
        {
            _instance = SerializationExtensions.CreateInstance<AdventureGameDesignerViewModel>(xmlData, new[] {typeof(Npc), typeof(PlaceableObject), typeof(Container), typeof(Room), typeof(AdventureObjectBase) });
            
            // Workaround for property grid not reflecting this properly when deserialized
            // from object
            if (_instance.StartRoomId.HasValue)
            {
                _instance.StartRoom = _instance.FindObjectByGuid(_instance.StartRoomId.Value) as Room;
            }

            foreach (var placeableObject in _instance.PlaceableObjectsList)
            {
                if(placeableObject.StartParentId.HasValue)
                {
                    placeableObject.Parent = _instance.FindObjectByGuid(placeableObject.StartParentId.Value);
                }
            }
        }

        public bool ShouldValidate(PropertyInfo property)
        {
            if ((property.Name == nameof(Introduction) || property.Name == nameof(Title)) && !EnableTitles)
            { 
                return false;
            }

            if ((property.Name == nameof(MaximumScore) || property.Name == nameof(WonGame)) && !EnableScore)
            {
                return false;
            }

            if (property.Name == nameof(MaximumInventorySize) && !EnableInventorySize)
            {
                return false;
            }

            if (property.Name == nameof(PlayerLostMessage) && !EnablePlayerLost)
            {
                return false;
            }

            return true;
        }
    }
}