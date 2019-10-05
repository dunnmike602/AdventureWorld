using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using DiagramDesigner.Controls;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    [DisplayName("NPC Speaks")]
    public class ConversationText : ConversationObjectBase
    {
        private string _text = string.Empty;

        [DataMember]
        [Browsable(false)]
        public List<ConversationResponse> ConversationResponses { get; set; } = new List<ConversationResponse>();

        [DataMember]
        [DisplayName("NPC Speaks")]
        [Category("Conversation Options")]
        [Description(@"Actual text to be spoken by the NPC.")]
        [Editor(typeof(TextEditor), typeof(TextEditor))]
        [ValidationItem(Description = "You must specify the text the Npc Speaks.",
             Severity = Severities.Error, ValidationType = ValidationTypes.NpcSpeaksMissing), NotNull]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)] [IgnoreDataMember] public override string BaseName { get; set; }

        public override void AddChild(ConversationObjectBase conversationObject)
        {
            if (conversationObject is ConversationResponse conversationResponse &&
                ConversationResponses.All(cr => cr.ControlId != conversationResponse.ControlId))
            {
                ConversationResponses.Add(conversationResponse);
            }
        }

        public override void RemoveChild(ConversationObjectBase conversationObject)
        {
            if (conversationObject is ConversationResponse conversationResponse &&
                ConversationResponses.Any(cr => cr.ControlId == conversationResponse.ControlId))
            {
                ConversationResponses.Add(conversationResponse);
            }
        }

        public string GetTextSummary()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return "<NOT SET>";
            }

            return "<" + (Text.Length < 10 ? Text : Text.Substring(0, 10) + "...") + ">";
        }
    }
}