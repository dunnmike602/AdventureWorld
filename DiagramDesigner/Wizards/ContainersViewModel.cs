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
    public class ContainersViewModel : ValidatableViewModelBase
    {
        private const string ObjectNameStub = "CONTAINER";

        private readonly string[] _names = new[] { "Crate", "Box", "Statue","Drum", "Bottle" };

        private readonly string[] _descriptions = 
            new[] { "An old packing case.", "An old cardboard box.", "A ornate golden statue.", "A dirty old oil drum.", "A brown beer bottle." };

        private ReactiveList<AdventureObjectViewModel> _containers = new ReactiveList<AdventureObjectViewModel>();
        private ContainerViewModel _selectedContainer;
        private bool _containerInfoIsValid = true;
        private ContainerViewModel _gridSelectedContainer;
        private bool _hideAdditionalInformation = true;
        private double _descriptionWidth = double.NaN;
        private GridLengthUnitType _descriptionSizer = GridLengthUnitType.AutoWithLastColumnFill;

        public ReactiveCommand<ContainerViewModel, Unit> PopulateContainerWithLorem { get; set; }
        public ReactiveCommand<ContainerViewModel, Unit> PopulateContainer { get; set; }
        public ReactiveCommand<ContainerViewModel, Unit> Delete { get; set; }
        public ReactiveCommand<ContainerViewModel, Unit> OpenTextEditorForContainerDescriptions { get; set; }
        public ReactiveCommand RowClickedCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PopulateAllFieldsWithLorem { get; set; }

        public ReactiveCommand AddNewRow { get; set; }

        private void CheckScreenValid()
        {
            ContainerInfoIsValid = !HasErrors &&
                                         (Containers.All(container => !container.HasErrors) || Containers.Count == 0);
        }

        private void PopulateAllFieldsWithLoremHandler()
        {
            foreach (var container in Containers)
            {
                if (string.IsNullOrWhiteSpace(container.BaseName))
                {
                    container.BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames());
                }

                ((ContainerViewModel) container).InventoryDescription = Faker.Lorem.Sentence();

                container.Descriptions[0] = Faker.Lorem.Paragraph();

                // Force a refresh
                container.Descriptions = container.Descriptions;
            }
        }

        private IEnumerable<string> GetBaseNames()
        {
            return Containers.Select(placeableObject => placeableObject.BaseName);
        }

        public bool ContainerInfoIsValid
        {
            get => _containerInfoIsValid;
            set => this.RaiseAndSetIfChanged(ref _containerInfoIsValid, value);
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

        public ContainersViewModel()
        {
            OpenTextEditorForContainerDescriptions =
                ReactiveCommand.Create<ContainerViewModel>(OpenDescriptionsEditorHandler);
            PopulateContainerWithLorem =
                ReactiveCommand.Create<ContainerViewModel>(PopulateContainerWithLoremHandler);
            PopulateContainer = ReactiveCommand.Create<ContainerViewModel>(PopulateContainerHandler);
            Delete = ReactiveCommand.Create<ContainerViewModel>(DeleteHandler);
            AddNewRow = ReactiveCommand.Create(AddNewRowHandler);
            RowClickedCommand = ReactiveCommand.Create(RowClickedCommandHandler);
            PopulateAllFieldsWithLorem = ReactiveCommand.Create(PopulateAllFieldsWithLoremHandler);


            Containers.ChangeTrackingEnabled = true;
            Disposables.Add(Containers.Changed.Subscribe(_ => CheckScreenValid()));

            Disposables.Add(Containers.ItemChanged.Subscribe(_ => CheckScreenValid()));

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

        private void RowClickedCommandHandler()
        {
            if (GridSelectedContainer != null)
            {
                OpenDescriptionsEditorHandler(GridSelectedContainer);
            }
        }

        private void OpenDescriptionsEditorHandler(ContainerViewModel currentContainer)
        {
            var editor = new MultiTextEditor(currentContainer.Descriptions, false, "Descriptions")
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            editor.ShowDialog();

            if (!editor.OkPressed)
            {
                return;
            }

            currentContainer.Descriptions = new ReactiveList<string>(editor.TextValues);
        }

        private void DeleteHandler(ContainerViewModel selectedContainer)
        {
            Containers.Remove(selectedContainer);
        }

        private void PopulateContainerHandler(ContainerViewModel selectedContainer)
        {
            selectedContainer.BaseName = _names[NextIndex];
            selectedContainer.Descriptions[0] = _descriptions[NextIndex];
            selectedContainer.InventoryDescription = _descriptions[NextIndex];
          
            // Force a refresh
            selectedContainer.Descriptions = selectedContainer.Descriptions;
            IncrementIndex();
        }

        private void AddNewRowHandler()
        {
            Containers.Add(new ContainerViewModel(Containers)
            {
                BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames())
            });

            Containers.Last().Descriptions.Add(string.Empty);
        }

        private void PopulateContainerWithLoremHandler(ContainerViewModel selectedContainer)
        {
            if (string.IsNullOrWhiteSpace(selectedContainer.BaseName))
            {
                selectedContainer.BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames());
            }

            selectedContainer.InventoryDescription = Faker.Lorem.Sentence();

            selectedContainer.Descriptions[0] = Faker.Lorem.Paragraph();
     
            // Force a refresh
            selectedContainer.Descriptions = selectedContainer.Descriptions;
        }

        public ContainerViewModel SelectedContainer
        {
            get => _selectedContainer;
            set => this.RaiseAndSetIfChanged(ref _selectedContainer, value);
        }

        public ContainerViewModel GridSelectedContainer
        {
            get => _gridSelectedContainer;
            set => this.RaiseAndSetIfChanged(ref _gridSelectedContainer, value);
        }

        public ReactiveList<AdventureObjectViewModel> Containers
        {
            get => _containers;
            set => this.RaiseAndSetIfChanged(ref _containers, value);
        }
    }
}