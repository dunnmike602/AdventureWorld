using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Services.Helpers;
using DiagramDesigner.AdventureWorld.Domain.Interfaces;
using DiagramDesigner.Controls;
using DiagramDesigner.Symbols.Helpers;
using SharedControls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using BitmapEditor = DiagramDesigner.Controls.BitmapEditor;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    [CategoryOrder("Identification", 0)]
    [CategoryOrder("Container Options", 1)]
    [CategoryOrder("NPC Options", 2)]
    [CategoryOrder("Dark Room", 3)]
    [CategoryOrder("Light Source Options", 4)]
    [CategoryOrder("Misc Flags", 5)]
    [CategoryOrder("Scripts", 6)]
    [CategoryOrder("Movement Types", 7)]
    [CategoryOrder("Conversation Options", 8)]
    public class AdventureObjectBase : IdentifableObjectBase, IValidationFilter
    {
        private ObservableCollection<string> _descriptions  = new ObservableCollection<string>();
        private string _baseName;
        private Script _objectPreprocessScript = new Script { Source = Assembly.GetExecutingAssembly().GetPreprocessCodeTemplate() };
        private bool _disposed;

        [DataMember]
        private bool _raiseNotifications = true;

        private bool _showRandomDescription;
        private string _imagePath;
        private string _lastImagePath;

        [Browsable(false)]
        public string Name => "Definition";

        [DisplayName("Preprocess Script")]
        [PropertyOrder(10)]
        [Editor(typeof(ScriptEditor), typeof(ScriptEditor))]
        [DataMember]
        [Category("Scripts")]
        [Description(@"This script is run as a pre-processing step by some engine commands. Examine, Drop and Take will run this before processing an object, Inventory and Look cause this to be run for the current room and Move runs the Exit script.")]
        public Script ObjectPreprocessScript
        {
            get => _objectPreprocessScript;
            set
            {
                _objectPreprocessScript = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasScript));
            }
        }

        [Browsable(false)]
        public bool HasScript => ObjectPreprocessScript.HasCompiledScript();

        [DataMember, Category("Identification"), DisplayName("Image Path"), PropertyOrder(2)]
        [Description("Optionally specify an image to use as a background for the items in the designer windows, to aid with identification.")]
        [Editor(typeof(BitmapEditor), typeof(BitmapEditor))]
        public virtual string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Image));
            }
        }


        [IgnoreDataMember]
        [Browsable(false)]
        public  BitmapImage Image => FileHelper.GetBitmapImage(ImagePath);

        [DataMember, Category("Identification"), DisplayName("Name"), PropertyOrder(1), ValidationItem(Description = "You must specify a name for this object",
            Severity = Severities.Error, ValidationType = ValidationTypes.NameNotSet), NotNull]
        [Description("Identifier of the object that can be used by the player to interact with it.")]
        public virtual string BaseName
        {
            get => _baseName;
            set
            {
                _baseName = value;
                OnPropertyChanged();
                OnPropertyChanged(ObjectFriendlyDescription);
            }
        }

        [DataMember, Category("Misc Flags"), DisplayName("Show Random Description"), PropertyOrder(1)]
        [Description("If set to true, randomizes the display of the Description property, a new random Description is picked on every display.")]
        public virtual bool ShowRandomDescription
        {
            get => _showRandomDescription;
            set
            {
                _showRandomDescription = value;
                OnPropertyChanged();
            }
        }

        [DataMember, PropertyOrder(2), Category("Identification"), Editor(typeof(CollectionEditor), typeof(CollectionEditor)), ValidationItem(Description = "You must specify at least one description",
            Severity = Severities.Error, ValidationType = ValidationTypes.DescriptionNotSet), NonEmptyCollection]
        [Description("List of possible descriptions for an game entity. By default the game engine will show the first one, they can be set programatically to represent different states.")]
        public virtual ObservableCollection<string> Descriptions
        {
            get => _descriptions;
            set
            {
                _descriptions = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public virtual string Category => string.Empty;

        [Browsable(false)]
        public virtual string ObjectFriendlyDescription => string.Empty;
        
        [Browsable(false)]
        public virtual string FriendlyDescription => BaseName;

        public virtual bool ShouldValidate(PropertyInfo property)
        {
            if ((property.Name == nameof(Descriptions) && GetType() == typeof(Exit)) && !AdventureGameDesignerViewModel.Instance.EnableExitDescriptions)
            {
                return false;
            }
            
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
            }

            base.Dispose(disposing);
        }

        public void SetNotifications(bool state)
        {
            _raiseNotifications = state;
        }

        public void SuppressImagePath()
        {
            _lastImagePath = ImagePath;
            ImagePath = null;
        }


        public void RestoreImagePath()
        {
            ImagePath = _lastImagePath;
        }

        [Browsable(false)]
        [IgnoreDataMember]
        public ObjectType ObjectType => AdventureObjectHelper.ConvertName(GetType().Name);

        [Browsable(false)]
        public virtual System.Windows.Media.Brush ObjectBrush { get; }

        [Browsable(false)]
        public virtual bool IsContainer { get; }

        [Browsable(false)]
        public virtual bool IsNpc { get; }

        [Browsable(false)]
        public virtual bool IsRoom { get; }

        [Browsable(false)]
        public virtual bool IsPlaceableObject { get; }

        [Browsable(false)]
        public virtual bool IsExit { get; }
    }
}
