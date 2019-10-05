using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Represents a line of dialog dialog by the player in the conversation tree.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ConversationResponse : ConversationObjectBase
    {
        private string _response;
        private int _sortOrder;
        private ConversationText _conversationText;

        /// <summary>
        /// NPC's response to this dialog, may be null and the conversation ends.
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
        /// Actual line of dialog picked by the player
        /// </summary>
        [DataMember]
        public string Response
        {
            get => _response;
            set
            {
                _response = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Order that the response will be diplayed on the screen. Default is 0, if omitted responses will be sorted alphabetically.
        /// </summary>
        [DataMember]
        public int SortOrder
        {
            get => _sortOrder;
            set
            {
                _sortOrder = value;
                OnPropertyChanged();
            }
        }
    }
}