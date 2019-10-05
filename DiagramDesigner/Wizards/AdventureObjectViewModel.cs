using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DiagramDesigner.Wizards.Attributes;
using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public class AdventureObjectViewModel : ValidatableViewModelBase
    {
        private ReactiveList<string> _descriptions = new ReactiveList<string>();
        private string _baseName;
        protected ReactiveList<AdventureObjectViewModel> ItemsForDuplicateCheck { get; private set; }
        
        public AdventureObjectViewModel(ReactiveList<AdventureObjectViewModel>  itemsForDuplicateCheck)
        {
            ItemsForDuplicateCheck = itemsForDuplicateCheck;
            _descriptions.ChangeTrackingEnabled = true;
        }

        [RequiredStringCollection(ErrorMessage = "You must specify at least one description.")]
        public ReactiveList<string> Descriptions
        {
            get => _descriptions;
            set
            {
                this.RaiseAndSetIfChanged(ref _descriptions, value); 
                this.RaisePropertyChanged();
            }
        }

        [Required]
        [CustomValidation(typeof(AdventureObjectViewModel), "ValidateDuplicates")]
        public string BaseName
        {
            get => _baseName;
            set => this.RaiseAndSetIfChanged(ref _baseName, value);
        }

        public static ValidationResult ValidateDuplicates(object obj, ValidationContext context)
        {
            var adventureObjectViewModel = (AdventureObjectViewModel)context.ObjectInstance;

            var duplicates = adventureObjectViewModel.ItemsForDuplicateCheck.GroupBy(room => room.BaseName).Where(g => g.Count() > 1).ToList();

            if (duplicates.Any(duplicate => duplicate.Key == adventureObjectViewModel.BaseName))
            {
                return new ValidationResult($"The name of a room, npc or object must be unique.",
                    new List<string> { "BaseName"});
            }

            return ValidationResult.Success;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            ItemsForDuplicateCheck = null;
        }
    }
}