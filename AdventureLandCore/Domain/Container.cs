using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    [DataContract(IsReference = true)]
    public class Container : PlaceableObject
    {
        private bool _isOpen;
        private bool _isLocked;
        
        [DataMember]
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                OnPropertyChanged();
            }
        }
        
        [DataMember]
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;

                OnPropertyChanged();
            }
        }

        public override bool IsContainer => true;

        public override bool IsNpc => false;

        public override bool IsPlaceableObject => true;

        public override bool IsRoom => false;

        public override bool IsExit => false;
    }
}