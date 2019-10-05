using System.Collections.Generic;
using System.Runtime.Serialization;
using AdventureLandCore.Services.Data;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Stores all mutable data for the current game.
    /// </summary>
    [DataContract(IsReference = true)]
    public class GameData
    {
        private Room _location;

        // Expose the global variables collection internal as a static so there is only ever one instance
        internal static Dictionary<string, object> GlobalVariables;

        /// <summary>
        /// Flag that is true on first pass through the game loop and false all other times.
        /// </summary>
        [DataMember]
        public bool IsFirstTimeThrough { get; set; }

        /// <summary>
        /// A full list of all objects that can appear in the game.
        /// </summary>
        [DataMember]
        public List<PlaceableObject> PlaceableObjects { get; set; }
       
        /// <summary>
        /// A full list of all rooms that can appear in the game.
        /// </summary>
        [DataMember]
        public List<Room> Rooms { get; set; }

        /// <summary>
        /// The game title.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// The game Introduction.
        /// </summary>
        [DataMember]
        public string Introduction { get; set; }

        /// <summary>
        /// The players score at any given point.
        /// </summary>
        [DataMember]
        public int CurrentScore { get; set; }

        /// <summary>
        /// The maxiumum score a player can reach.
        /// </summary>
        [DataMember]
        public int MaximumScore { get; set; }

        /// <summary>
        /// Sorted List of noise words currently in use in the game.
        /// </summary>
        [DataMember]
        internal SortedList<string, string> StopWords { get; set; }

        /// <summary>
        /// Current size of the inventory.
        /// </summary>
        [DataMember]
        public int InventorySize { get; set; }

        /// <summary>
        /// List of Command Mappings used in the game.
        /// </summary>
        [DataMember]
        public List<CommandMapping> CommandMappings { get; set; }

        /// <summary>
        /// Stores a list of user-defined variables
        /// </summary>
        [DataMember]
        public Dictionary<string, object> Variables
        {
            get => GlobalVariables;
            set => GlobalVariables = value;
        }

        /// <summary>
        /// Room object in which the Player is currently located.
        /// </summary>
        [DataMember]
        public Room Location
        {
            get => _location;
            set
            {
                _location = value;
                SetupCurrentLocation();
            }
        }
        
        /// <summary>
        /// A dictionary of all the possible directions that can be used to move around in the game.
        /// </summary>
        [DataMember]
        public Dictionary<int, Direction> DirectionMappings { get; set; }

        [DataMember]
        public bool EnableTitles { get; set; }

        [DataMember]
        public bool EnableScore { get; set; }

        [DataMember]
        public bool EnableInventorySize { get; set; }

        [DataMember]
        public bool EnableShowItemsInRoom { get; set; }

        [DataMember]
        public bool EnableExitDescriptions { get; set; }

        [DataMember]
        public bool EnablePlayerLost { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public Player Player { get; set; } = new Player();

        [DataMember]
        public string GameName { get; set; }

        [DataMember]
        public string PlayerLostMessage { get; set; }

        [DataMember]
        public string AnotherGameText { get; set; }
        
        [DataMember]
        public string AnotherGameYesResponse { get; set; }

        [DataMember]
        public string NoItemsInRoomText { get; set; }

        [IgnoreDataMember]
        public bool IsQuit { get; set; }


        /// <summary>
        /// Text to be displayed for the Command Prompt.
        /// </summary>
        [DataMember]
        public string CommandPromptText { get; set; }

        private void SetupCurrentLocation()
        {
            if (Rooms == null)
            {
                return;
            }

            foreach (var room in Rooms)
            {
                room.IsCurrentRoom = false;
            }

            Location.IsCurrentRoom = true;
        }
    }
}
