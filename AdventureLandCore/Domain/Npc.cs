using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using AdventureLandCore.Extensions;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Represents a Non-player character that can be manipulated and interacted with.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Npc : Container
    {
        private bool _autoFollow;
        private Guid? _currentConversation;
        private List<Conversation> _conversations;

        /// <summary>
        /// If set to true, this NPC will always go to the same room as the player.
        /// </summary>
        [DataMember]
        [Description("If set to true, this NPC will always go to the same room as the player.")]
        public bool AutoFollow
        {
            get => _autoFollow;
            set
            {
                _autoFollow = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// List of possible conversations that this NPC can have, use in conjunction with CurrentConversation to set the interaction that can be had with the NPC.
        /// </summary>
        [DataMember]
        [Description("List of possible conversations that this NPC can have, use in conjunction with CurrentConversation to set the interaction that can be had with the NPC.")]
        public List<Conversation> Conversations
        {
            get => _conversations;
            set
            {
                _conversations = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Currently selected conversation for the NPC. May be null if the NPC does not speak.
        /// </summary>
        [DataMember]
        [Description("Currently selected conversation for the NPC. May be null if the NPC does not speak.")]
        public Guid? CurrentConversation
        {
            get => _currentConversation;
            set
            {
                _currentConversation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sets the current conversation that will be triggered when interacting with this NPC.
        /// </summary>
        /// <param name="name">Name of the conversation.</param>
        /// <returns>The Conversation object representing the current conversation.</returns>
        public Conversation SetCurrentConversation(string name)
        {
            CurrentConversation = Conversations.FirstOrDefault(con => con.Name.IsEqualTo(name))?.Id;

            return Conversations.FirstOrDefault(con => con.Name.IsEqualTo(name));
        }

        /// <summary>
        /// Retrieves the current Conversation object.
        /// </summary>
        /// <returns>The Conversation object representing the current conversation.</returns>
        public Conversation GetCurrentConversation()
        {
            return Conversations.FirstOrDefault(con => con.Id == CurrentConversation);
        }

        public override bool IsContainer => true;

        public override bool IsNpc => true;

        public override bool IsPlaceableObject => true;

        public override bool IsRoom => false;

        public override bool IsExit => false;
    }
}