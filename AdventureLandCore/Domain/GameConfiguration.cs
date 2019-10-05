using System.Collections.Generic;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Services.Data;

namespace AdventureLandCore.Domain
{
    public class GameConfiguration : IGameConfiguration
    {
        private readonly AdventureGameSetup _adventureGameSetup;

        public GameConfiguration(AdventureGameSetup adventureGameSetup)
        {
            _adventureGameSetup = adventureGameSetup;
        }

        public string FullFilePath => _adventureGameSetup.FullFilePath;

        public string Id => _adventureGameSetup.Id;

        public string AnotherGameText => _adventureGameSetup.AnotherGameText;

        public string AnotherGameYesResponse => _adventureGameSetup.AnotherGameYesResponse;

        public string WonGameMessage => _adventureGameSetup.WonGameMessage;

        public string PlayerLostMessage => _adventureGameSetup.PlayerLostMessage;

        public string CommandPromptText => _adventureGameSetup.CommandPromptText;

        public string NoItemsInRoomText => _adventureGameSetup.NoItemsInRoomText;

        public string GameName => _adventureGameSetup.GameName;

        public Script InitialisationCode => _adventureGameSetup.InitialisationCode;

        public Script GameLoopPreProcessCode => _adventureGameSetup.GameLoopPreProcessCode;

        public Script GameLoopPostProcessCode => _adventureGameSetup.GameLoopPostProcessCode;

        public Script CommonCode => _adventureGameSetup.CommonCode;

        public string Title => _adventureGameSetup.Title;

        public string Introduction => _adventureGameSetup.Introduction;

        public int MaximumScore => _adventureGameSetup.MaximumScore;

        public int InventorySize => _adventureGameSetup.InventorySize;

        public string[] StopWords => _adventureGameSetup.StopWords;

        public List<CommandMapping> CommandMappings => _adventureGameSetup.CommandMappings;

        public bool EnableTitles => _adventureGameSetup.EnableTitles;

        public bool EnablePlayerLost => _adventureGameSetup.EnablePlayerLost;

        public bool EnableScore => _adventureGameSetup.EnableScore;

        public bool EnableShowItemsInRoom => _adventureGameSetup.EnableShowItemsInRoom;

        public Dictionary<int, Direction> DirectionMappings => _adventureGameSetup.DirectionMappings;

        public bool EnableInventorySize => _adventureGameSetup.EnableInventorySize;

        public bool EnableExitDescriptions => _adventureGameSetup.EnableExitDescriptions;
        
        public bool EnableDebug => _adventureGameSetup.EnableDebug;

        public string ConsoleLogFile => _adventureGameSetup.ConsoleLogFile;

        public string StartRoom => _adventureGameSetup.StartRoom;

        public List<Room> Rooms => _adventureGameSetup.Rooms;

        public List<PlaceableObject> PlaceableObjects => _adventureGameSetup.PlaceableObjects;
        
        public Script GetScriptFromIdHelper(MessageIds id)
        {
            switch (id)
            {
                case MessageIds.GameLoopPreProcessCode:
                    return GameLoopPreProcessCode;

                case MessageIds.GameLoopPostProcessCode:
                    return GameLoopPostProcessCode;

                case MessageIds.InitialisationCode:
                    return InitialisationCode;

                case MessageIds.CommonCode:
                    return CommonCode;

                default:
                    return null;
            }
        }
    }
}