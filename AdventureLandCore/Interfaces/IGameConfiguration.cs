using System.Collections.Generic;
using AdventureLandCore.Domain;
using AdventureLandCore.Services.Data;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Interfaces
{
    [IgnoreInObjectBrowser]
    public interface IGameConfiguration
    {
        string AnotherGameText { get;  }
        string AnotherGameYesResponse { get; }
        string CommandPromptText { get; }
        string NoItemsInRoomText { get; }
        string Title { get; }
        string Introduction { get; }
        string WonGameMessage { get; }
        bool EnablePlayerLost { get; }
        string PlayerLostMessage { get; }
        string Id { get; }
        int MaximumScore { get; }
        int InventorySize { get; }
        string[] StopWords { get; }
        List<CommandMapping> CommandMappings { get; }
        bool EnableTitles { get; }
        bool EnableScore { get; }
        bool EnableShowItemsInRoom { get; }
        bool EnableInventorySize { get; }
        bool EnableExitDescriptions { get; }
        bool EnableDebug { get; }
        string StartRoom { get; }
        List<PlaceableObject> PlaceableObjects { get; }
        List<Room> Rooms { get; }
        string GameName { get; }
        string FullFilePath { get; }
        Dictionary<int, Direction> DirectionMappings { get; }
        Script GetScriptFromIdHelper(MessageIds id);
        string ConsoleLogFile { get; }
    }
}