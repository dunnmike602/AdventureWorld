using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Media;
using DiagramDesigner.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    [DisplayName("Placeable Object")]
    public class PlaceableObject : AdventureObjectBase
    {
        private AdventureObjectBase _parent;
        private bool _isHeld;
        private string _inventoryDescription;
        private bool _visible = true;
        private bool _isLightSource;
        private bool _isLit;
        private bool _fixed;
        private bool _hideFromAutoList;
        private bool _parentIsContainer;

        [DataMember]
        [Browsable(false)]
        public Guid? StartParentId { get; set; }

        [Browsable(false)]
        public override string Category => "Placeable Objects";

        [Browsable(false)]
        public override string ObjectFriendlyDescription => "Placeable Object";

        [Browsable(false)]
        public bool ParentIsContainer
        {
            get => _parentIsContainer;
            set
            {
                _parentIsContainer = value;
                OnPropertyChanged();
            }
        }

        [PropertyOrder(5)]
        [DisplayName("Parent")]
        [Category("Identification")]
        [NotNull]
        [Editor(typeof(ParentObjectEditor), typeof(ParentObjectEditor))]
        [ValidationItem(Description = "This object in not placed in a room or a container.",
         Severity = Severities.Warning, ValidationType = ValidationTypes.ObjectNotInRoom)]
        [Description("A Placeable Object can be in a room, a container object or held by and NPC. If IsHeld is true, Parent will be null.")]
        [DataMember]
        public AdventureObjectBase Parent
        {
            get => _parent;
            set
            {
                _parent = value;

                if (value != null)
                {
                    IsHeld = false;
                    StartParentId = _parent.ControlId;
                }
                else
                {
                    StartParentId = null;
                }

                OnPropertyChanged();

                if (value != null)
                {
                    AdventureDesigner.Instance.RefreshParentBinding(this);
                }
                else
                {
                    AdventureDesigner.Instance.RemoveParentBinding(this);
                    AdventureDesigner.Instance.UpdatePropertyGrid();
                }

                ParentIsContainer = value is Container;
            }
        }

        [DataMember]
        [Category("Identification")]
        [PropertyOrder(6)]
        [Editor(typeof(TextEditor), typeof(TextEditor))]
        [DisplayName("Inventory Description")]
        [Description("Description used when the player inventory is listed by the game engine. If not specified then the name of the object is used.")]
        public string InventoryDescription
        {
            get => _inventoryDescription;
            set
            {
                _inventoryDescription = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        [DisplayName("Held")]
        [Category("Misc Flags")]
        [PropertyOrder(3)]
        [Description("Indicates whether the object is currently held by the player.")]
        public bool IsHeld
        {
            get => _isHeld;
            set
            {
                _isHeld = value;

                if (_isHeld)
                {
                    Parent = null;
                }

                OnPropertyChanged();
            }
        }

        [DataMember]
        [Category("Misc Flags")]
        [PropertyOrder(7)]
        [Description("Indicates whether the object is visible or not by default.")]
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
        [Category("Light Source Options")]
        [PropertyOrder(1)]
        [DisplayName("Is Light Source")]
        [Description("Indicates whether the object can be used as a light source.")]
        public bool IsLightSource
        {
            get => _isLightSource;
            set
            {
                _isLightSource = value;

                if (!_isLightSource)
                {
                    IsLit = false;
                }

                OnPropertyChanged();
            }
        }

        [DataMember]
        [Category("Light Source Options")]
        [PropertyOrder(2)]
        [DisplayName("Is Lit")]
        [Description("Indicates whether the object is currently lit.")]
        public bool IsLit
        {
            get => _isLit;
            set
            {
                _isLit = IsLightSource && value;

                OnPropertyChanged();
            }
        }
        
        [DataMember]
        [Category("Misc Flags")]
        [PropertyOrder(10)]
        [Description("Indicates whether the object can be picked up. For example a lamp may be fixed to the wall.")]
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
        [Category("Misc Flags")]
        [PropertyOrder(11)]
        [DisplayName("Hide From Auto List")]
        [Description("Indicates whether the object will be automatically listed when a room is described. Useful for Fixed objects, that can be described as part of the room description and not shown in the list.")]
        public bool HideFromAutoList
        {
            get => _hideFromAutoList;
            set
            {
                _hideFromAutoList = value;
                OnPropertyChanged();
            }
        }

        public Room GetParentRoom()
        {
            var currentParent = Parent;
         
            while (currentParent is PlaceableObject )
            {
                currentParent = (currentParent as PlaceableObject).Parent;
            }

            return currentParent as Room;
        }


        [Browsable(false)]
        public override Brush ObjectBrush => new SolidColorBrush(Colors.Red);

        [Browsable(false)]
        public override bool IsContainer => false;

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