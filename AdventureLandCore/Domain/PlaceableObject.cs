using System;
using System.Runtime.Serialization;
using MLDComputing.ObjectBrowser.Attributes;

namespace AdventureLandCore.Domain
{
    [DataContract(IsReference = true)]
    public class PlaceableObject : AdventureObjectBase
    {
        private bool _isHeld;
        private string _inventoryDescription;
        private bool _isLightSource;
        private bool _isLit;
        private bool _fixed;
        private AdventureObjectBase _parent;
        private Guid? _parentId;
        private bool _visible;
        private bool _hideFromAutoList;

        [DataMember]
        public bool IsLit
        {
            get => _isLit;
            set
            {
                _isLit = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool Fixed
        {
            get => _fixed;
            set
            {
                _fixed = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool IsLightSource
        {
            get => _isLightSource;
            set
            {
                _isLightSource = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool IsHeld
        {
            get => _isHeld;
            set
            {
                _isHeld = value;

                // If the item is held it cannot be in a room or container
                if (IsHeld)
                {
                    Parent = null;
                }

                OnPropertyChanged();
            }
        }

        [DataMember]
        public AdventureObjectBase Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        [IgnoreInObjectBrowser]
        internal Guid? ParentId
        {
            get => _parentId;
            set
            {
                _parentId = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool HideFromAutoList
        {
            get => _hideFromAutoList;
            set
            {
                _hideFromAutoList = value;
                OnPropertyChanged();
            }
        }

       
        [DataMember]
        public string InventoryDescription
        {
            get => string.IsNullOrWhiteSpace(_inventoryDescription) ? Name : _inventoryDescription;
            set
            {
                _inventoryDescription = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Returns the PlaceableObject's current location, taking in to account whether it is a container, in a room 
        /// or held by the player
        /// </summary>
        /// <param name="currentRoom">Supplies the current room, this is passed in in case the object is held.</param>
        /// <returns>The location (Room) of the object.</returns>
        public Room GetCurrentLocation(Room currentRoom)
        {
            // If item is held it is in the current room
            if (IsHeld)
            {
                return currentRoom;
            }

            if (Parent.IsRoom)
            {
                return Parent as Room;
            }

            var currentParent = Parent;

            while (currentParent is Container container)
            {
                // If ultimate parent is held then the item if effectively in the current room
                if (container.Parent == null && container.IsHeld)
                {
                    return currentRoom;
                }

                currentParent = container.Parent;
            }

            if (currentParent == null)
            {
                return null;
            }

            return currentParent.IsRoom ? currentParent as Room : null;
        }

        /// <summary>
        /// Determines if the current object is visible from the outside. Rules are different for items in rooms or items in containers
        /// </summary>
        /// <returns>True if visible, false otherwise.</returns>
        public bool GetIsVisibleFromOutside()
        {
            bool itemIsVisible;

            if (Parent is Room)
            {
                itemIsVisible = Visible || IsHeld;
            }
            else
            {
                itemIsVisible = Visible && AllParentsOpen(this);
            }

            return itemIsVisible;
        }

        private bool AllParentsOpen(PlaceableObject placeableObject)
        {
            var currentPlaceableObject = placeableObject;

            var allParentsOpen = true;

            while (currentPlaceableObject.Parent is Container container)
            {
                if (!container.IsOpen)
                {
                    allParentsOpen = false;
                    break;
                }

                currentPlaceableObject = container;
            }

            return allParentsOpen;
        }

        public override bool IsContainer => false;

        public override bool IsNpc => false;

        public override bool IsPlaceableObject => true;

        public override bool IsRoom => true;

        public override bool IsExit => false;
    }
}