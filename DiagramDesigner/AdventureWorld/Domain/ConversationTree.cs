using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows.Data;
using AdventureLandCore.Domain;
using ReactiveUI;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    public class ConversationTree : ReactiveObjectBase
    {
        private ObservableCollection<ValidationItemAttribute> _validationItems;
        private ReactiveList<Conversation> _conversations;

        private static readonly object Lock = new object();

        [DataMember]
        public ReactiveList<Conversation> Conversations
        {
            get => _conversations;
            set
            {
                _conversations = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public ObservableCollection<ValidationItemAttribute> ValidationItems
        {
            get => _validationItems ??  new ObservableCollection<ValidationItemAttribute>();
            set
            {
                _validationItems = value;
                BindingOperations.EnableCollectionSynchronization(ValidationItems, Lock);
                OnPropertyChanged();
            }
        }
    }
}