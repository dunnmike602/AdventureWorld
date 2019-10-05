using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    [DataContract(IsReference = true)]
    public class Exit : AdventureObjectBase
    {
        private string _roomName;
        private string _direction;
        private bool _visible;
        private bool _isLocked;
        private bool _canCrawl;
        private bool _canRun;
        private bool _canWalk;
        private bool _canSwim;

        [DataMember]
        public string RoomName
        {
            get => _roomName;
            set
            {
                _roomName = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string Direction
        {
            get => _direction;
            set
            {
                _direction = value;
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
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool CanSwim
        {
            get => _canSwim;
            set
            {
                _canSwim = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool CanWalk
        {
            get => _canWalk;
            set
            {
                _canWalk = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool CanRun
        {
            get => _canRun;
            set
            {
                _canRun = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool CanCrawl
        {
            get => _canCrawl;
            set
            {
                _canCrawl = value;
                OnPropertyChanged();
            }
        }
        
        public override string ToString()
        {
            return $"{Direction} to {RoomName}";
        }

        public override bool IsContainer => false;

        public override bool IsNpc => false;

        public override bool IsPlaceableObject => false;

        public override bool IsRoom => false;

        public override bool IsExit => true;
    }
}