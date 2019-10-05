using System.ComponentModel;
using System.Runtime.Serialization;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    public class Conversation : ConversationObjectBase
    {
        private string _escapeText;
        private ConversationText _conversationText;

        [DataMember]
        [Browsable(false)]
        public ConversationText ConversationText
        {
            get => _conversationText;
            set
            {
                _conversationText = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        [DisplayName("Escape Text")]
        [Category("Conversation Options")]
        [Description(@"If specified the player can bail out of the conversation at any point. If omitted the player cannot leave the script until a natural termination.")]
        public string EscapeText
        {
            get => _escapeText;
            set
            {
                _escapeText = value;
                OnPropertyChanged();
            }
        }

        public override void AddChild(ConversationObjectBase conversationObject)
        {
            if (conversationObject is ConversationText conversationText)
            {
                ConversationText = conversationText;
            }
        }

        public override void RemoveChild(ConversationObjectBase conversationObject)
        {
            if (ConversationText.ControlId == conversationObject.ControlId)
            {
                ConversationText = null;
            }
        }
    }
}