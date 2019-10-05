using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    [DataContract(IsReference = true)]
    public class Player : AdventureObjectBase
    {
        private bool _isDestroyed;

        [DataMember]
        public bool IsDestroyed
        {
            get => _isDestroyed;
            set
            {
                _isDestroyed = value;
                OnPropertyChanged();
            }
        }

        public override bool IsContainer => false;

        public override bool IsNpc => false;

        public override bool IsPlaceableObject => false;

        public override bool IsRoom => false;

        public override bool IsExit => false;
    }
}