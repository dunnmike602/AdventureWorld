using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Media;
using DiagramDesigner.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    [DisplayName("Non Player Character")]
    public class Npc : Container
    {
        private bool _autoFollow;
        private ConversationTree _conversationTree;
        private Guid? _currentConversation;

        [Browsable(false)] public override string Category => "Npcs";

        [Browsable(false)] public override string ObjectFriendlyDescription => "Npc";


        [DataMember]
        [Category("NPC Options")]
        [PropertyOrder(1)]
        [DisplayName("Follow Automatically")]
        [Description("This Npc will always follow the player.")]
        [Browsable(true)]
        public bool AutoFollow
        {
            get => _autoFollow;
            set
            {
                _autoFollow = value;

                OnPropertyChanged();
            }
        }

        [DataMember]
        [Category("NPC Options")]
        [PropertyOrder(2)]
        [DisplayName("Conversation")]
        [Description("Open the conversation designer and design conversation(s) for this NPC.")]
        [Browsable(true)]
        [Editor(typeof(ConversationEditor), typeof(ConversationEditor))]
        public ConversationTree ConversationTree
        {
            get => _conversationTree;
            set
            {
                _conversationTree = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentConversation));
            }
        }

        [DataMember]
        [Category("NPC Options")]
        [PropertyOrder(3)]
        [DisplayName("Current Conversation")]
        [Description("Currently selected conversation for the NPC. May be null if the NPC does not speak.")]
        [Browsable(true)]
        [ItemsSource(typeof(ConversationItemsSource))]
        public Guid? CurrentConversation
        {
            get => _currentConversation;
            set
            {
                _currentConversation = value;

                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public override Brush ObjectBrush => new SolidColorBrush(Colors.Goldenrod);

        [Browsable(false)]
        public override bool IsContainer => true;

        [Browsable(false)]
        public override bool IsNpc => true;

        [Browsable(false)]
        public override bool IsRoom => false;

        [Browsable(false)]
        public override bool IsPlaceableObject => true;

        [Browsable(false)]
        public override bool IsExit => false;
    }
}