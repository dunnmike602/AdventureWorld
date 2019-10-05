using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public class RoomViewModel : AdventureObjectViewModel
    {
        private bool _isDark;

        public bool IsDark
        {
            get => _isDark;
            set => this.RaiseAndSetIfChanged(ref _isDark, value);
        }

        public RoomViewModel(ReactiveList<AdventureObjectViewModel> itemsForDuplicateCheck) : base(itemsForDuplicateCheck)
        {
        }
    }
}