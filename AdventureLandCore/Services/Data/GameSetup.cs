using System.Collections.Generic;
using System.Runtime.Serialization;
using AdventureLandCore.Domain;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Services.Data
{
    [IgnoreInObjectBrowser]
    [DataContract(IsReference = true)]
    public class AdventureGameSetup
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string GameName { get; set; }

        [DataMember]
        public string WonGameMessage { get; set; }

        [DataMember]
        public string PlayerLostMessage { get; set; }

        [DataMember]
        public string AnotherGameText { get; set; }
        
        [DataMember]
        public string AnotherGameYesResponse { get; set; }

        [DataMember]
        public string CommandPromptText { get; set; }

        [DataMember]
        public string AssemblyName { get; set; }

        [DataMember]
        public Script InitialisationCode { get; set; }
        
        [DataMember]
        public Script GameLoopPreProcessCode { get; set; }

        [DataMember]
        public Script GameLoopPostProcessCode { get; set; }

        [DataMember]
        public Script CommonCode { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Introduction { get; set; }

        [DataMember]
        public int InventorySize { get; set; }

        [DataMember]
        public bool EnablePlayerLost { get; set; }

        [DataMember]
        public string[] StopWords { get; set; }

        [DataMember]
        public int MaximumScore { get; set; }

        [DataMember]
        public string StartRoom { get; set; }

        [DataMember]
        public bool EnableTitles { get; set; }

        [DataMember]
        public bool EnableScore { get; set; }

        [DataMember]
        public Dictionary<int, Direction> DirectionMappings { get; set; }

        [DataMember]
        public bool EnableShowItemsInRoom { get; set; }

        [DataMember]
        public bool EnableInventorySize { get; set; }

        [DataMember]
        public bool EnableExitDescriptions { get; set; }
        
        [DataMember]
        public List<Room> Rooms { get; set; }

        [DataMember]
        public List<PlaceableObject> PlaceableObjects { get; set; }

        [DataMember]
        public List<CommandMapping> CommandMappings { get; set; }

        [IgnoreDataMember]
        public string FullFilePath { get; set; }

        [DataMember]
        public bool EnableDebug { get; set; }

        [DataMember]
        public string ConsoleLogFile { get; set; }

        [DataMember]
        public string NoItemsInRoomText { get; set; }

        public void ClearAllScripts()
        {
            InitialisationCode.ClearScript();
            GameLoopPostProcessCode.ClearScript();
            GameLoopPreProcessCode.ClearScript();
        }
    }
}
