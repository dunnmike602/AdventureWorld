using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Defines a conversation stage.
    /// </summary>
    [DataContract]
    public enum ConversationStage
    {
        /// <summary>
        /// Start of the conversation.
        /// </summary>
        [DataMember]
        Start = 1,
        /// <summary>
        /// Npc is about to speak.
        /// </summary>
        [DataMember]
        NpcSpeaks = 2,
        /// <summary>
        /// Player is about to respond.
        /// </summary>
        [DataMember]
        PlayerResponds = 3,

    }
}