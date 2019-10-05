using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReflectionHelper.Annotations;

namespace ReflectionHelper.Core
{
    public abstract class DescriptorBase : INotifyPropertyChanged
    {
        private bool _isSelected;

        private bool _isSearchSuccessful;

        public string Image { get; set; }

        public string ImageDescription { get; set; }

        public string DisplayName { get; set; }

        public string TypeDefinition { get; set; }

        public string CodeComments { get; set; }

        public Dictionary<string, string> ParameterComments { get; set; }

        public string ReturnValue { get; set; }

        public bool HasDescription()
        {
            return !string.IsNullOrWhiteSpace(CodeComments);
        }

        public bool HasReturnValue()
        {
            return !string.IsNullOrWhiteSpace(ReturnValue);
        }

        public bool IsSearchSuccessful
        {
            get => _isSearchSuccessful;
            set
            {
                _isSearchSuccessful = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}