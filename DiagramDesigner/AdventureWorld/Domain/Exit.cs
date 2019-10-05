using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract]
    [DefaultProperty("ExitName")]
    public class Exit : AdventureObjectBase
    {
        private string _direction;
        private bool _visible = true;
        private bool _isLocked;
        private bool _canWalk = true;
        private bool _canRun = true;
        private bool _canCrawl = true;
        private bool _canSwim;
     
        [Browsable(false)] public new string BaseName { get; set; }

        [Browsable(false)]
        public override string ImagePath { get; set; }

        [DataMember]
        [Category("Identification")]
        [PropertyOrder(4)]
        [ItemsSource(typeof(DirectionItemsSource))]
        [ValidationItem(Description = "You must specify a direction for the Exit",
            Severity = Severities.Error, ValidationType = ValidationTypes.NoExitDirection)]
        [NotNull]
        [Description("The direction of this exit, this indicates how the player trigger it. For example NORTH.")]
        public string Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Description));
            }
        }

        [Category("Identification")]
        [Browsable(false)]
        public override string Category => "Exits";
        
        [Browsable(false)]
        public override string ObjectFriendlyDescription => "Definition";

        [Category("Identification")]
        [PropertyOrder(0)]
        [DisplayName("Exit Description")]
        [Description("Full description of the exit, including the room it leads to.")]
        public string Description => $"{FriendlyDescription}";
        
        [Browsable(false)]
        public override string FriendlyDescription
        {
            get
            {
                var direction = string.IsNullOrWhiteSpace(_direction)
                    ? "<Not Set>"
                    : _direction;

                return "Exit: " + direction + " to " + ToRoom.BaseName;
            }
        }

        [DataMember]
        [Browsable(false)]
        public Room FromRoom { get; set; }

        [DataMember]
        [Browsable(false)]
        public Room ToRoom { get; set; }

        [ReadOnly(true)]
        [PropertyOrder(5)]
        [Category("Identification")]
        [Description("The room this exit starts from.")]
        public string Start => FromRoom == null ? string.Empty : FromRoom.BaseName;

        [ReadOnly(true)]
        [PropertyOrder(6)]
        [Category("Identification")]
        [Description("The room this exit leads to.")]
        public string To => ToRoom == null ? string.Empty : ToRoom.BaseName;

        [DataMember]
        [PropertyOrder(7)]
        [Category("Misc Flags")]
        [Description("Indicates whether or not the exit is currently visible.")]
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
        [PropertyOrder(1)]
        [Category("Movement Types")]
        [DisplayName("Can Walk")]
        [Description("Indicates whether or not the exit can be walked through.")]
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
        [PropertyOrder(2)]
        [Category("Movement Types")]
        [DisplayName("Can Run")]
        [Description("Indicates whether or not the exit can be run through.")]
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
        [PropertyOrder(3)]
        [Category("Movement Types")]
        [DisplayName("Can Crawl")]
        [Description("Indicates whether or not the exit can be crawled through.")]
        public bool CanCrawl
        {
            get => _canCrawl;
            set
            {
                _canCrawl = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        [PropertyOrder(4)]
        [Category("Movement Types")]
        [DisplayName("Can Swim")]
        [Description("Indicates whether or not the exit can be swum through.")]
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
        [Category("Misc Flags")]
        [PropertyOrder(8)]
        [DisplayName("Locked")]
        [Description("Indicates whether or not the exit is currently locked.")]
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                OnPropertyChanged();
            }
        }
        
        public void Init()
        {
            if (Descriptions == null)
            {
                Application.Current.Dispatcher.Invoke(() => { Descriptions = new ObservableCollection<string>(); });
            }
        }

        public override string ToString()
        {
            return $"{Direction ?? "MISSING"} to {ToRoom.BaseName}";
        }

        [Browsable(false)]
        public override bool IsContainer => false;

        [Browsable(false)]
        public override bool IsNpc => false;

        [Browsable(false)]
        public override bool IsRoom => false;

        [Browsable(false)]
        public override bool IsPlaceableObject => false;

        [Browsable(false)]
        public override bool IsExit => true;
    }
}