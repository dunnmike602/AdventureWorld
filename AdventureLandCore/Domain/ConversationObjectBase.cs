using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Base class behaviour for all Adventure Game objects.
    /// </summary>
    [IgnoreInObjectBrowser]
    [DataContract(IsReference = true)]
    public class ConversationObjectBase : INotifyPropertyChanged
    {
        private Script _conversationPreprocessScript;

        /// <summary>
        /// Unique identifier for the Conversation object.
        /// </summary>
        [DataMember]
        [ReadOnly(true)]
        [Description("Unique identifier for the Conversation object.")]
        public Guid Id { get; set; }
    
        /// <summary>
        /// Script to run before the Conversation is processed by a command.
        /// </summary>
        [DataMember]
        [Description("Script to run before the Conversation is processed by a command.")]
        public Script ConversationPreprocessScript
        {
            get => _conversationPreprocessScript;
            set
            {
                _conversationPreprocessScript = value;
                OnPropertyChanged();
            }
        }

        [IgnoreInObjectBrowser]
        public event PropertyChangedEventHandler PropertyChanged;

        [IgnoreInObjectBrowser]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}