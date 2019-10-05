using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Services.CoreApi.Interfaces;
using AdventureLandCore.Services.Data;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Services.CoreApi
{
    [IgnoreInObjectBrowser]
    public class AdventureApi : IAdventureApi
    {
        private const string EverythingWildCard = "*";

        private Dictionary<string, string> _directionAbbreviations;

        private readonly IPersistanceOperations _persistanceOperations;
        private readonly IConsole _console;
        
        public GameData GameData { get; set; }

        public IGameConfiguration Configuration { get; set; }

        internal AdventureApi(){}

        internal AdventureApi(IGameConfiguration configuration, IPersistanceOperations persistanceOperations, IConsole console)
        {
            Configuration = configuration;
            _persistanceOperations = persistanceOperations;
            _console = console;

            SetGameDataFromConfiguration();
        }
        
        public Room GetCurrentLocation()
        {
            return GameData.Location;
        }

        public void SetCurrentLocation(Room newRoom)
        {
            // Ensure the old location has been visited
            GameData.Location.Visited = true;

            GameData.Location = newRoom;
        }

        public bool CanContinue { get; set; }

        public string LastPlayerInput { get; set; }

        public void SuppressAutomaticInputProcessing()
        {
            // Remove the last input so there is nothing for the game engine to do.
            LastPlayerInput = string.Empty;
        }

        public ParsedAdventureCommand LastParsedCommand { get; set ; }

        public ParsedAdventureCommand LastExecutedCommand { get; set; }

        private void ShowItemsInRoom()
        {
            var objectsInRoom = GetVisibleAndListablePaceableObjectsInRoom(GameData.Location.Name);

            if (objectsInRoom.Count == 0)
            {
                _console.FormattedWrite(GameData.NoItemsInRoomText);
            }

            foreach (var nextObject in objectsInRoom)
            {
                _console.FormattedWrite(nextObject.CurrentDescription);
            }
        }

        public List<Container> GetAllContainers()
        {
            return GameData.PlaceableObjects.Where(nextObject => nextObject.GetType() == typeof(Container)).OfType<Container>().ToList();
        }

        public List<Npc> GetAllNpcs()
        {
            return GameData.PlaceableObjects.Where(nextObject => nextObject.GetType() == typeof(Npc)).OfType<Npc>().ToList();
        }

        public List<Npc> GetAllAutoFollowNpcs()
        {
            return GetAllNpcs().Where(npc => npc.AutoFollow).ToList();
        }

        public List<PlaceableObject> GetAllPlaceableObjects()
        {
            return GameData.PlaceableObjects.Where(nextObject => nextObject.GetType() == typeof(PlaceableObject)).ToList();
        }

        public List<PlaceableObject> GetChildObjects(PlaceableObject placeableObject)
        {
            return GameData.PlaceableObjects.Where(nextObject => nextObject.Parent?.Id == placeableObject.Id).ToList();
        }

        public List<AdventureObjectBase> GetAllAdventureObjects()
        {
            return GameData.Rooms.Cast<AdventureObjectBase>().Concat(GameData.PlaceableObjects)
                .Concat(GameData.Rooms.SelectMany(room => room.Exits)).ToList();
        }

        public PlaceableObject GetContainerForPlaceableObject(PlaceableObject placeableObject)
        {
            return GameData.PlaceableObjects.FirstOrDefault(nextObject => nextObject.Id == placeableObject?.Parent.Id);
        }

        public void ListChildObjectDescriptions(PlaceableObject placeableObject)
        {
            foreach (var nextObject in GetChildObjects(placeableObject))
            {
                _console.FormattedWrite(nextObject.CurrentDescription);
            }
        }

        public void QuitGame()
        {
            GameData.IsQuit = true;
        }

        public string SerializeToString()
        {
            var serializer = new DataContractSerializer(typeof(GameData), new[] { typeof(PlaceableObject), typeof(Container), typeof(Room), typeof(Npc) });

            using (var stringWriter = new StringWriter())
            using (var writer = new XmlTextWriter(stringWriter))
            {
                serializer.WriteObject(writer, GameData);
                var text = stringWriter.ToString();

                return StringCompressor.CompressString(text);
            }
        }

        public GameData DeserializeFromString(string xmlData)
        {
            xmlData = StringCompressor.DecompressString(xmlData);

            using (Stream stream = new MemoryStream())
            {
                var data = Encoding.UTF8.GetBytes(xmlData);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                var deserializer = new DataContractSerializer(typeof(GameData), new[] { typeof(PlaceableObject), typeof(Container), typeof(Room), typeof(Npc) });
                
                var newEngineObject = deserializer.ReadObject(stream) as GameData;

                return newEngineObject;
            }
        }

        private void ShowCurrentLocation()
        {
            // ReSharper disable once PossibleNullReferenceException
            _console.FormattedWrite(GameData.Location.CurrentDescription);
        }

        public void ShowRoomInformation()
        {
            ShowCurrentLocation();

            if (!GameData.Location.IsDark && GameData.EnableShowItemsInRoom)
            {
                ShowItemsInRoom();
            }
        }

        public List<PlaceableObject> GetAllPlaceableObject()
        {
            return GameData.PlaceableObjects;
        }

     
        public List<PlaceableObject> GetInventory()
        {
            return GameData.PlaceableObjects.Count == 0
                ? new List<PlaceableObject>()
                : GameData.PlaceableObjects.Where(m => m.IsHeld).ToList();
        }

        public int CurrentInventoryCount => GetInventory().Count;

        public void ThrowToRoom(PlaceableObject objectToThrow, string roomName)
        {
            var room = GetRoomFromName(roomName);

            if (room == null)
            {
                throw new Exception($"The room {roomName} does not exist.");
            }
                
            objectToThrow.IsHeld = false;
            objectToThrow.Parent = room;
        }

        public void Drop(PlaceableObject objectToDrop)
        {
            objectToDrop.IsHeld = false;
            objectToDrop.Parent = GetCurrentLocation();
        }

        public void Take(PlaceableObject objectToTake)
        {
            if (!objectToTake.Fixed)
            {
                objectToTake.IsHeld = true;
                objectToTake.Parent = null;
            }
        }
        
        public void InitGameData(GameData newGameData)
        {
            GameData = newGameData;
        }

        public string LoadSaveGame(string fileStem)
        {
            return _persistanceOperations.Load(fileStem);
        }

        public void DeleteSaveGame(string fileStem)
        {
            _persistanceOperations.Delete(fileStem);
        }

        public List<string> ListSaveGames()
        {
            return _persistanceOperations.List();
        }

        public bool IsDebugEnabled()
        {
            return Configuration.EnableDebug;
        }

        private void SetGameDataFromConfiguration()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            GameData = new GameData
            {
                Version = Configuration.Id + "-" + Assembly.GetExecutingAssembly().GetName().Version.Major,
                PlaceableObjects = Configuration.PlaceableObjects,
                Rooms = Configuration.Rooms,
                Title = Configuration.Title,
                Introduction = Configuration.Introduction,
                MaximumScore = Configuration.MaximumScore,
                StopWords =  new SortedList<string, string>(Configuration.StopWords.Distinct().ToDictionary(s => s.ToUpper())),
                InventorySize = Configuration.InventorySize,
                CommandMappings = Configuration.CommandMappings,
                IsFirstTimeThrough = true,
                GameName = Configuration.GameName,
                PlayerLostMessage = Configuration.PlayerLostMessage,
                AnotherGameText = Configuration.AnotherGameText,
                AnotherGameYesResponse = Configuration.AnotherGameYesResponse,
                EnableTitles = Configuration.EnableTitles,
                EnableScore = Configuration.EnableScore,
                EnableShowItemsInRoom = Configuration.EnableShowItemsInRoom,
                EnableInventorySize = Configuration.EnableInventorySize,
                EnableExitDescriptions = Configuration.EnableExitDescriptions,
                EnablePlayerLost = Configuration.EnablePlayerLost,
                CommandPromptText = Configuration.CommandPromptText,
                DirectionMappings = Configuration.DirectionMappings,
                NoItemsInRoomText = Configuration.NoItemsInRoomText,
            };
          
            GameData.Variables = new Dictionary<string, object>();
            GameData.Location = GetStartRoom();

            _directionAbbreviations = Configuration.DirectionMappings.ToDictionary(p => p.Value.Abbreviation, p => p.Value.Text);
        }

        public void KillPlayer()
        {
            GameData.Player.IsDestroyed = true;
        }

        public void ResurrectPlayer()
        {
            GameData.Player.IsDestroyed = false;
        }

        public void Save(string fileStem, string data)
        {
            _persistanceOperations.Save(fileStem, data);
        }

        public List<Npc> GetNpcsFromNames(List<GameWord> sentenceWords, double percentOfWordToMatch = 70.0, bool allRefersToHeldItems = false)
        {
            return GetObjectsFromNames(sentenceWords, percentOfWordToMatch,allRefersToHeldItems).Where(obj => obj.IsNpc).Cast<Npc>().ToList();
        }

        public List<PlaceableObject> GetObjectsFromNames(List<GameWord> sentenceWords, double percentOfWordToMatch = 70.0, bool allRefersToHeldItems = false)
        {
            var sentenceWordsOnly = sentenceWords.Select(word => word.Word).ToList();

            // Remove All and Everything and replace with all the items in the room
            if (EverythingWildCard.IsEqualToAny(sentenceWordsOnly))
            {
                // Determine if All refers to held items
                var roomItems = allRefersToHeldItems
                    ? GetInventory().SelectMany(placeableObject => placeableObject.Name.SplitSentence())
                    : GetVisiblePlaceableObjectsInRoom().SelectMany(placeableObject => placeableObject.Name.SplitSentence());

                sentenceWordsOnly = sentenceWordsOnly.Where(word => !word.IsEqualTo(EverythingWildCard)).Union(roomItems).ToList();
            }

            return GetAdventureObjectsFromNamesHelper(sentenceWordsOnly, GameData.PlaceableObjects, percentOfWordToMatch);
        }

        public List<Room> GetRoomsFromNames(List<GameWord> sentenceWords, double percentOfWordToMatch = 70.0)
        {
            return GetAdventureObjectsFromNamesHelper(sentenceWords.Select(word => word.Word).ToList(), GameData.Rooms, percentOfWordToMatch);
        }

        private List<T> GetAdventureObjectsFromNamesHelper<T>(IList<string> sentenceWords, IList<T> adventureObjects, double percentOfWordToMatch) where T : AdventureObjectBase, new()
        {
            var matchedObjects = new List<T>();

            foreach (var word in sentenceWords)
            {
                var foundObject = adventureObjects.FirstOrDefault(placeableObject =>
                    placeableObject.Name.SplitSentence().Any(objectWord => objectWord.MatchesPercent(word, percentOfWordToMatch)));

                if (foundObject != null)
                {
                    foundObject.IsValid = true;
                    foundObject.WordThatMatchedThis = word;
                    matchedObjects.Add(foundObject);
                }
                else
                {
                    var newObject = new T
                    {
                        IsValid = false,
                        Name = word,
                    };

                    matchedObjects.Add(newObject);
                }
            }

            return matchedObjects.GroupBy(x => x.Name).Select(y => y.FirstOrDefault()).ToList();
        }

        public Room GetRoomFromName(string roomName)
        {
            return GameData.Rooms.FirstOrDefault(room => room.Name.IsEqualTo(roomName));
        }

        public bool IsNameCurrentRoom(string roomName, double percentOfWordToMatch = 70.0)
        {
            return GetCurrentLocation().Name.MatchesPercent(roomName, percentOfWordToMatch);
        }

        public PlaceableObject GetPlaceableObjectFromName(string objectName)
        {
            return GameData.PlaceableObjects.FirstOrDefault(placeableObject => placeableObject.Name.IsEqualTo(objectName));
        }

        public Npc GetNpcFromName(string npcName)
        {
            return GameData.PlaceableObjects.OfType<Npc>().FirstOrDefault(npc => npc.Name.IsEqualTo(npcName));
        }

        public Container GetContainerFromName(string containerName)
        {
            return GameData.PlaceableObjects.OfType<Container>().FirstOrDefault(npc => npc.Name.IsEqualTo(containerName));
        }

        public Exit GetExitFromName(string roomName, string direction)
        {
            var room = GetRoomFromName(roomName);

            return GetExit(room, direction);
        }

        public Exit GetExit(Room room, string direction)
        {
            return room?.Exits.FirstOrDefault(exit => exit.Direction.IsEqualTo(direction) && exit.Visible);
        }

        public string GetDefaultHelpText()
        {
            var helptext = "Commands:".AddLineBreaks();

            foreach (var mapping in GameData.CommandMappings)
            {
                var aliases = mapping.AliasList.Count == 0
                    ? ": "
                    : " (alias(es) " + mapping.AliasList.Aggregate((s1, s2) => s1 + "," + s2) + "): ";

                helptext += mapping.VerbName + aliases + mapping.HelpText.AddLineBreaks();
            }

            return helptext;
        }


        public List<string> GetMoveLimits()
        {
            return _directionAbbreviations.Keys.Union(_directionAbbreviations.Values).ToList();
        }

        public string GetDirectionFromAbbreviation(string abbreviation)
        {
            abbreviation = abbreviation.ToUpper();

            return _directionAbbreviations.ContainsKey(abbreviation) ? _directionAbbreviations[abbreviation] : null;
        }

        public bool IsItemAvailableToExamine(PlaceableObject objectToCheck)
        {
            return objectToCheck.GetIsVisibleFromOutside() && (objectToCheck.Parent == null 
                || objectToCheck.GetCurrentLocation(GetCurrentLocation())?.Id == GetCurrentLocation().Id);
        }
        
        public List<PlaceableObject> GetVisiblePlaceableObjectsInRoom(string roomName = "")
        {
            return GetPlaceableObjectsInRoom(roomName).Where(placeableObject => placeableObject.Visible).ToList();
        }

        public List<PlaceableObject> GetVisibleAndListablePaceableObjectsInRoom(string roomName = "")
        {
            return GetVisiblePlaceableObjectsInRoom(roomName).Where(placeableObject => !placeableObject.HideFromAutoList).ToList();
        }

        public List<PlaceableObject> GetPlaceableObjectsInRoom(string roomName = "")
        {
            roomName = ReplaceEmptyRoomWithCurrent(roomName);

            var room = GetRoomFromName(roomName);

            if (room == null)
            {
                throw new Exception($"The room {roomName} does not exist.");
            }

            return GameData.PlaceableObjects.Count == 0
                ? new List<PlaceableObject>()
                : GameData.PlaceableObjects.Where(placeableObject => placeableObject.Parent?.Id == room.Id && !placeableObject.IsHeld).ToList();
        }

        private string ReplaceEmptyRoomWithCurrent(string roomName)
        {
            return string.IsNullOrWhiteSpace(roomName) ? GetCurrentLocation().Name : roomName;
        }

        public PlaceableObject GetHeldObjectFromName(string name)
        {
            return GetInventory().FirstOrDefault(m => m.Name.IsEqualTo(name));
        }

        public bool DoIHaveTheObject(string name)
        {
            return GetInventory().Any(m => m.Name.IsEqualTo(name));
        }

        public Room GetStartRoom()
        {
            return GameData.Rooms?.Where(m => m.Name == Configuration?.StartRoom).FirstOrDefault();
        }
        
        public void ClearVariables()
        {
            GameData.Variables.Clear();
        }

        public void SetVariable(string name, object value)
        {
            if (GameData.Variables.ContainsKey(name))
            {
                GameData.Variables[name] = value;
            }
            else
            {
                GameData.Variables.Add(name, value);
            }
        }

        public object GetVariable(string name)
        {
            return GameData.Variables.ContainsKey(name) ?  GameData.Variables[name] : null;
        }

        public void ClearVariable(string name)
        {
            if (GameData.Variables.ContainsKey(name))
            {
                GameData.Variables.Remove(name);
            }
        }

        public bool HasVariable(string name)
        {
            return GameData.Variables.ContainsKey(name);
        }
   }
}