using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public class NpcViewModel : ContainerViewModel
    {
        private bool _autoFollow;
      
        public bool AutoFollow
        {
            get => _autoFollow;
            set => this.RaiseAndSetIfChanged(ref _autoFollow, value);
        }

        public NpcViewModel(ReactiveList<AdventureObjectViewModel> itemsForDuplicateCheck) : base(itemsForDuplicateCheck)
        {
        }
    }
}