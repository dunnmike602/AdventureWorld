using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Media;
using DiagramDesigner.AdventureWorld.CustomCollections;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    [DisplayName("Container")]
    public class Container : PlaceableObject
    {
        private bool _isOpen;
        private bool _isLocked;
        private ObservableItemCollection<PlaceableObject> _placeableObjects = new ObservableItemCollection<PlaceableObject>();
      
        [Browsable(false)] public override string Category => "Containers";

        [Browsable(false)] public override string ObjectFriendlyDescription => "Container";

        [DataMember]
        [Category("Container Options")]
        [PropertyOrder(1)]
        [DisplayName("Is Open")]
        [Description("Indicates whether the container is Open or Closed.")]
        [Browsable(true)]
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;

                // Can't be open and locked at the same time
                if (_isOpen)
                {
                    IsLocked = false;
                }

                OnPropertyChanged();
            }
        }
        
        [DataMember]
        [Category("Container Options")]
        [PropertyOrder(2)]
        [DisplayName("Is Locked")]
        [Description("Indicates whether the container it is Locked.")]
        [Browsable(true)]
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;

                // Can't be open and locked at the same time
                if (_isLocked)
                {
                    IsOpen = false;
                }
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        [DataMember]
        public ObservableItemCollection<PlaceableObject> PlaceableObjects
        {
            get => _placeableObjects ?? new ObservableItemCollection<PlaceableObject>();
            set
            {
                _placeableObjects = value;

                OnPropertyChanged();
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
        public override Brush ObjectBrush => new SolidColorBrush(Colors.DarkOliveGreen);

        [Browsable(false)]
        public override bool IsContainer => true;

        [Browsable(false)]
        public override bool IsNpc => false;

        [Browsable(false)]
        public override bool IsRoom => false;

        [Browsable(false)]
        public override bool IsPlaceableObject => true;

        [Browsable(false)]
        public override bool IsExit => false;
    }
}