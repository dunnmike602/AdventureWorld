using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using SharedControls;
using Syncfusion.UI.Xaml.Grid;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace DiagramDesigner.Wizards
{
    public class PlaceableObjectsViewModel : ValidatableViewModelBase
    {
       private const string ObjectNameStub = "OBJECT";

        private readonly string[] _names = new[] { "Key", "Wand", "Gun","Dressing Gown", "Spacesuit", "Sponge" };

        private readonly string[] _descriptions = new[] { "A golden key with ornate writing on it.", "A wooden beautifully crafted wand.", "A six-shooter with a wooden handle.", "A large spacesuit with and oxygen tank.", "A dirty sponge." };

        private ReactiveList<AdventureObjectViewModel> _placeableObjects = new ReactiveList<AdventureObjectViewModel>();
        private PlaceableObjectViewModel _selectedPlaceableObject;
        private bool _placeableObjectInfoIsValid = true;
        private PlaceableObjectViewModel _gridSelectedPlaceableObject;
        private bool _hideAdditionalInformation = true;
        private double _descriptionWidth = double.NaN;
        private GridLengthUnitType _descriptionSizer = GridLengthUnitType.AutoWithLastColumnFill;

        public ReactiveCommand<PlaceableObjectViewModel, Unit> PopulatePlaceableObjectWithLorem { get; set; }
        public ReactiveCommand<PlaceableObjectViewModel, Unit> PopulatePlaceableObject { get; set; }
        public ReactiveCommand<PlaceableObjectViewModel, Unit> Delete { get; set; }
        public ReactiveCommand<PlaceableObjectViewModel, Unit> OpenTextEditorForPlaceableObjectDescriptions { get; set; }
        public ReactiveCommand<object, Unit> RowClickedCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PopulateAllFieldsWithLorem { get; set; }

        public ReactiveCommand AddNewRow { get; set; }

        private void CheckScreenValid()
        {
            PlaceableObjectInfoIsValid = !HasErrors &&
                                         (PlaceableObjects.All(placeableObject => !placeableObject.HasErrors) || PlaceableObjects.Count == 0);
        }

        private void PopulateAllFieldsWithLoremHandler()
        {
            foreach (var placeableObject in PlaceableObjects)
            {
                if (string.IsNullOrWhiteSpace(placeableObject.BaseName))
                {
                    placeableObject.BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames());
                }

                ((PlaceableObjectViewModel) placeableObject).InventoryDescription = Faker.Lorem.Sentence();

                placeableObject.Descriptions[0] = Faker.Lorem.Paragraph();

                // Force a refresh
                placeableObject.Descriptions = placeableObject.Descriptions;
            }
        }

        public bool PlaceableObjectInfoIsValid
        {
            get => _placeableObjectInfoIsValid;
            set => this.RaiseAndSetIfChanged(ref _placeableObjectInfoIsValid, value);
        }

        public bool HideAdditionalInformation
        {
            get => _hideAdditionalInformation;
            set => this.RaiseAndSetIfChanged(ref _hideAdditionalInformation, value);
        }

        public double DescriptionWidth
        {
            get => _descriptionWidth;
            set => this.RaiseAndSetIfChanged(ref _descriptionWidth, value);
        }

        public GridLengthUnitType DescriptionSizer
        {
            get => _descriptionSizer;
            set => this.RaiseAndSetIfChanged(ref _descriptionSizer, value);
        }

        public PlaceableObjectsViewModel()
        {
            OpenTextEditorForPlaceableObjectDescriptions =
                ReactiveCommand.Create<PlaceableObjectViewModel>(OpenDescriptionsEditorHandler);
            PopulatePlaceableObjectWithLorem =
                ReactiveCommand.Create<PlaceableObjectViewModel>(PopulatePlaceableObjectWithLoremHandler);
            PopulatePlaceableObject = ReactiveCommand.Create<PlaceableObjectViewModel>(PopulatePlaceableObjectHandler);
            Delete = ReactiveCommand.Create<PlaceableObjectViewModel>(DeleteHandler);
            AddNewRow = ReactiveCommand.Create(AddNewRowHandler);
            RowClickedCommand = ReactiveCommand.Create<object>(RowClickedCommandHandler);
            PopulateAllFieldsWithLorem = ReactiveCommand.Create(PopulateAllFieldsWithLoremHandler);


            PlaceableObjects.ChangeTrackingEnabled = true;
            Disposables.Add(PlaceableObjects.Changed.Subscribe(_ => CheckScreenValid()));

            Disposables.Add(PlaceableObjects.ItemChanged.Subscribe(_ => CheckScreenValid()));

            Disposables.Add(
                this.WhenAnyValue(x => x.HideAdditionalInformation).Subscribe(_ => SetWidthForDescription()));
            
            Disposables.Add(Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => PropertyChanged += h,
                    h => PropertyChanged -= h
                )
                .Subscribe(_ => CheckScreenValid()));
        }
        
        private void SetWidthForDescription()
        {
            if (HideAdditionalInformation)
            {
                DescriptionWidth = double.NaN;
                DescriptionSizer = GridLengthUnitType.AutoWithLastColumnFill;
            }
            else
            {
                DescriptionWidth = 200.0;
                DescriptionSizer = GridLengthUnitType.None;
            }
        }

        private void RowClickedCommandHandler(object parameters)
        {
            if (GridSelectedPlaceableObject != null)
            {
                OpenDescriptionsEditorHandler(GridSelectedPlaceableObject);
            }
        }

        private void OpenDescriptionsEditorHandler(PlaceableObjectViewModel currentPlaceableObject)
        {
            var editor = new MultiTextEditor(currentPlaceableObject.Descriptions, false, "Descriptions")
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            editor.ShowDialog();

            if (!editor.OkPressed)
            {
                return;
            }

            currentPlaceableObject.Descriptions = new ReactiveList<string>(editor.TextValues);
        }

        private void DeleteHandler(PlaceableObjectViewModel selectedPlaceableObject)
        {
            PlaceableObjects.Remove(selectedPlaceableObject);
        }

        private void PopulatePlaceableObjectHandler(PlaceableObjectViewModel selectedPlaceableObject)
        {
            selectedPlaceableObject.BaseName = _names[NextIndex];
            selectedPlaceableObject.Descriptions[0] = _descriptions[NextIndex];
            selectedPlaceableObject.InventoryDescription = _descriptions[NextIndex];
          
            // Force a refresh
            selectedPlaceableObject.Descriptions = selectedPlaceableObject.Descriptions;
            IncrementIndex();
        }

        private void AddNewRowHandler()
        {
            PlaceableObjects.Add(new PlaceableObjectViewModel(PlaceableObjects)
                { BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames()) });

            PlaceableObjects.Last().Descriptions.Add(string.Empty);
        }

        private IEnumerable<string> GetBaseNames()
        {
            return PlaceableObjects.Select(placeableObject => placeableObject.BaseName);
        }

        private void PopulatePlaceableObjectWithLoremHandler(PlaceableObjectViewModel selectedPlaceableObject)
        {
            if (string.IsNullOrWhiteSpace(selectedPlaceableObject.BaseName))
            {
                selectedPlaceableObject.BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames());
            }

            selectedPlaceableObject.InventoryDescription = Faker.Lorem.Sentence();

            selectedPlaceableObject.Descriptions[0] = Faker.Lorem.Paragraph();
     
            // Force a refresh
            selectedPlaceableObject.Descriptions = selectedPlaceableObject.Descriptions;
        }

        public PlaceableObjectViewModel SelectedPlaceableObject
        {
            get => _selectedPlaceableObject;
            set => this.RaiseAndSetIfChanged(ref _selectedPlaceableObject, value);
        }

        public PlaceableObjectViewModel GridSelectedPlaceableObject
        {
            get => _gridSelectedPlaceableObject;
            set => this.RaiseAndSetIfChanged(ref _gridSelectedPlaceableObject, value);
        }

        public ReactiveList<AdventureObjectViewModel> PlaceableObjects
        {
            get => _placeableObjects;
            set => this.RaiseAndSetIfChanged(ref _placeableObjects, value);
        }
    }
}