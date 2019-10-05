using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Represents an entire conversation tree between an NPC and a player
    /// </summary>
    [DataContract(IsReference = true)]
    public class Conversation : ConversationObjectBase
    {
        private string _escapeText;
        private ConversationText _conversationText;
        private string _name;

        /// <summary>
        /// Unique name for the conversation.
        /// </summary>
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The initial dialog that the NPC will say when prompted by a command. May be null and the conversation will end
        /// </summary>
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

        /// <summary>
        /// If specified the player can bail out of the conversation at any point. If omitted the player cannot leave the script until a natural termination.
        /// </summary>
        [DataMember]
        public string EscapeText
        {
            get => _escapeText;
            set
            {
                _escapeText = value;
                OnPropertyChanged();
            }
        }
    }
}