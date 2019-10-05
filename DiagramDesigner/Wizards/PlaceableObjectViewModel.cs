using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public class PlaceableObjectViewModel : AdventureObjectViewModel
    {
        private string _inventoryDescription;
        private bool _isVisible = true;
        private bool _isLightSource;
        private bool _isLit;
        private bool _fixed;
        private bool _hideFromAutoList;

        public string InventoryDescription
        {
            get => _inventoryDescription;
            set => this.RaiseAndSetIfChanged(ref _inventoryDescription, value);
        }

        public bool IsLightSource
        {
            get => _isLightSource;
            set => this.RaiseAndSetIfChanged(ref _isLightSource, value);
        }

        public bool IsLit
        {
            get => _isLit;
            set => this.RaiseAndSetIfChanged(ref _isLit, value);
        }

        public bool HideFromAutoList
        {
            get => _hideFromAutoList;
            set => this.RaiseAndSetIfChanged(ref _hideFromAutoList, value);
        }
        
        public bool Fixed
        {
            get => _fixed;
            set => this.RaiseAndSetIfChanged(ref _fixed, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => this.RaiseAndSetIfChanged(ref _isVisible, value);
        }

        public PlaceableObjectViewModel(ReactiveList<AdventureObjectViewModel> itemsForDuplicateCheck) : base(itemsForDuplicateCheck)
        {
        }
    }
}