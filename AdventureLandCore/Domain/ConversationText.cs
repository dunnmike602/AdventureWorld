using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Represents a line of dialog spoken by an NPC in a conversation tree.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ConversationText : ConversationObjectBase
    {
        private string _text = string.Empty;
        private List<ConversationResponse> _conversationResponses = new List<ConversationResponse>();

        /// <summary>
        /// Possible dialog responses the player can speak in response to the NPC speaking. May be empty and the conversation ends.
        /// </summary>
        [DataMember]
        public List<ConversationResponse> ConversationResponses
        {
            get => _conversationResponses;
            set
            {
                _conversationResponses = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The actual dialog of the NPC.
        /// </summary>
        [DataMember]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}