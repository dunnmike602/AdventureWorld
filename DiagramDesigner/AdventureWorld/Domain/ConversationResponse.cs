using System.ComponentModel;
using System.Runtime.Serialization;
using DiagramDesigner.Controls;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    [DisplayName("Player Responds")]
    public class ConversationResponse : ConversationObjectBase
    {
        private string _response;
        private int _sortOrder;
        private ConversationText _conversationText;

        [Browsable(false)]
        [DataMember]
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
        [DisplayName("Player Responds")]
        [Category("Conversation Options")]
        [Description(@"A response that can be selected by the player.")]
        [Editor(typeof(TextEditor), typeof(TextEditor))]
        [ValidationItem(Description = "You must specify the text for Player Responds.",
            Severity = Severities.Error, ValidationType = ValidationTypes.ResponseMissing), NotNull]
        public string Response
        {
            get => _response;
            set
            {
                _response = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        [DisplayName("Sort Order")]
        [Category("Conversation Options")]
        [Description(@"Order that the response will be diplayed on the screen. Default is 0, if omitted responses will be sorted alphabetically.")]
        public int SortOrder
        {
            get => _sortOrder;
            set
            {
                _sortOrder = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        [IgnoreDataMember]
        public override string BaseName { get; set; }

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

        public string GetTextSummary()
        {
            if (string.IsNullOrWhiteSpace(Response))
            {
                return "<NOT SET>";
            }

            return "<" + (Response.Length < 10 ? Response : Response.Substring(0, 10) + "...") + ">";
        }
    }
}