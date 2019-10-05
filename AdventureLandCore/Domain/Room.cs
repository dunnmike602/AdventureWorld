using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Represents a location in the adventure world. The player will be on one and only one room.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Room : AdventureObjectBase
    {
        private List<Exit> _exits;
        private bool _isDark;
        private bool _visited;
        private bool _isCurrentRoom;

        /// <summary>
        /// List of Exits leading from this room.
        /// </summary>
        [DataMember]
        [Description("List of Exits leading from this room.")]
        public List<Exit> Exits
        {
            get => _exits;
            set
            {
                _exits = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Flag that indicates whether the room is currently dark.
        /// </summary>
        [DataMember]
        [Description("Flag that indicates whether the room is currently dark.")]
        public bool IsDark
        {
            get => _isDark;
            set
            {
                _isDark = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Flag that indicates whether the room is the one the player is in.
        /// </summary>
        [DataMember]
        [Description("Flag that indicates whether the room is the one the player is in.")]
        public bool IsCurrentRoom
        {
            get => _isCurrentRoom;
            set
            {
                _isCurrentRoom = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Flag that indicates whether the room has been visited.
        /// </summary>
        [DataMember]
        [Description("Flag that indicates whether the room has been visited.")]
        public bool Visited
        {
            get => _visited;
            set
            {
                _visited = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Base implementation of all Adventure Objects for ToString
        /// </summary>
        /// <returns>The name of the Adventure game object.</returns>
        public override string ToString()
        {
            var text = Name;

            if (IsCurrentRoom)
            {
                text += " <-- You are here!";
            }

            return text;
        }

        public override bool IsContainer => false;

        public override bool IsNpc => false;

        public override bool IsPlaceableObject => false;

        public override bool IsRoom => true;

        public override bool IsExit => false;

        /// <summary>
        /// Returns only exits that can be unlocked, that is visible and locked.
        /// </summary>
        /// <returns>List of active unlockable exits.</returns>
        public List<Exit> GetUnlockableExits()
        {
            return Exits.Where(exit => exit.Visible && exit.IsLocked).ToList();
        }
    }
}