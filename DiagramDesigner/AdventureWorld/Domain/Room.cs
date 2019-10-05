using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Media;
using ReactiveUI;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract]
    public class Room : AdventureObjectBase
    {
        private bool _isDark;
        private ReactiveList<Exit> _exits;
        private ReactiveList<PlaceableObject> _placeableObjects;

        public Room()
        {
            SetupExitCollection();
            Initialize();
        }

        private void SetupExitCollection()
        {
            Exits = new ReactiveList<Exit>();
        }
        
        public void Initialize()
        {
            SetupExitCollection();

            AdventureGameDesignerViewModel.Instance.Exits.CollectionChanged += ExitsCollectionChanged;

            PlaceableObjects = new ReactiveList<PlaceableObject>();

            // Force at least one refresh of the collections
            RefreshPlaceableObject();
            RefreshExits();
        }

        private void ExitsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshExits();
        }

        private void RefreshExits()
        {
            Exits.Clear();

            foreach (var item in AdventureGameDesignerViewModel.Instance.GetExitsForRoom(ControlId))
            {
                Exits.Add(item);
            } 
        }

        public void RefreshPlaceableObject()
        {
            PlaceableObjects.Clear();

            foreach (var item in AdventureGameDesignerViewModel.Instance.GetObjectsForRoom(ControlId))
            {
                PlaceableObjects.Add(item);
            }
        }

        [Browsable(false)]
        public override string Category => "Rooms";

        [Browsable(false)]
        public override string ObjectFriendlyDescription => "Room Definition";

        [DataMember]
        [DisplayName("Is Room Dark")]
        [Category("Dark Room")]
        [PropertyOrder(1)]
        [Description("Indicates whether the room is currently dark.")]
        public bool IsDark
        {
            get => _isDark;
            set
            {
                _isDark = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public ReactiveList<Exit> Exits
        {
            get => _exits;
            set
            {
                _exits = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public ReactiveList<PlaceableObject> PlaceableObjects
        {
            get => _placeableObjects;
            set
            {
                _placeableObjects = value;
                OnPropertyChanged();
            }
        }

        public IList<Room> GetConnectedRooms()
        {
            return Exits.Where(m => m.ToRoom != null).Select(m => m.ToRoom).ToList();
        }

        [Browsable(false)]
        public override Brush ObjectBrush => new SolidColorBrush(Colors.Blue);

        [Browsable(false)]
        public override bool IsContainer => false;

        [Browsable(false)]
        public override bool IsNpc => false;

        [Browsable(false)]
        public override bool IsRoom => true;

        [Browsable(false)]
        public override bool IsPlaceableObject => false;

        [Browsable(false)]
        public override bool IsExit => false;
    }
}
