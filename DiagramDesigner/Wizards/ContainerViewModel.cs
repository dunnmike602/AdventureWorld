using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public class ContainerViewModel : PlaceableObjectViewModel
    {
        private bool _isOpen;
        private bool _isLocked;

        public bool IsOpen
        {
            get => _isOpen;
            set => this.RaiseAndSetIfChanged(ref _isOpen, value);
        }

        public bool IsLocked
        {
            get => _isLocked;
            set => this.RaiseAndSetIfChanged(ref _isLocked, value);
        }

        public ContainerViewModel(ReactiveList<AdventureObjectViewModel> itemsForDuplicateCheck) : base(itemsForDuplicateCheck)
        {
        }
    }
}