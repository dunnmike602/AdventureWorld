namespace DiagramDesigner.AdventureWorld.Domain
{
    public class Client : DropDownItem
    {
        private ClientType _clientType;

        public ClientType ClientType
        {
            get => _clientType;
            set
            {
                _clientType = value;
                OnPropertyChanged();
            }
        }
    }
}