using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using DiagramDesigner.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    public abstract class ConversationObjectBase : AdventureObjectBase
    {
        private Script _conversationPreprocessScript = new Script { Source = Assembly.GetExecutingAssembly().GetConversationCodeTemplate() };

        [IgnoreDataMember]
        [Browsable(false)]
        public override string ImagePath { get; set; }

        [IgnoreDataMember]
        [Browsable(false)]
        public override ObservableCollection<string> Descriptions { get; set; }

        [IgnoreDataMember]
        [Browsable(false)]
        public override bool ShowRandomDescription { get; set; }

        [DisplayName("Preprocess Script")]
        [PropertyOrder(10)]
        [Editor(typeof(ConversationScriptEditor), typeof(ConversationScriptEditor))]
        [DataMember]
        [Category("Scripts")]
        [Description(@"This script will run before the conversation starts (in the case of a Conversation) or before text is displayed. It allows an action to be performed as part of the conversation.")]
        public Script ConversationPreprocessScript
        {
            get => _conversationPreprocessScript;
            set
            {
                _conversationPreprocessScript = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasScript));
            }
        }

        [Browsable(false)]
        [IgnoreDataMember]
        public new Script ObjectPreprocessScript { get; set; }

        public abstract void AddChild(ConversationObjectBase conversationObject);

        public abstract void RemoveChild(ConversationObjectBase conversationObject);
    }
}