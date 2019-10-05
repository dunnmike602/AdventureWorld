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
    public class NpcsViewModel : ValidatableViewModelBase
    {
        private const string ObjectNameStub = "NPC";

        private readonly string[] _names = new[] { "Dog", "Dwarf", "A Wizard","Old Man", "Warrior" };

        private readonly string[] _descriptions = 
            new[] { "A small sad looking dog.", "A Dwarf with a long beard and a sharp axe.", "A Wizard in a long grey cloak.", "An old man in a dirty robe.", "A proud warrior." };

        private ReactiveList<AdventureObjectViewModel> _npcs = new ReactiveList<AdventureObjectViewModel>();
        private NpcViewModel _selectedNpc;
        private bool _npcInfoIsValid = true;
        private NpcViewModel _gridSelectedNpc;
        private bool _hideAdditionalInformation = true;
        private double _descriptionWidth = double.NaN;
        private GridLengthUnitType _descriptionSizer = GridLengthUnitType.AutoWithLastColumnFill;

        public ReactiveCommand<NpcViewModel, Unit> PopulateNpcsWithLorem { get; set; }
        public ReactiveCommand<NpcViewModel, Unit> PopulateNpc { get; set; }
        public ReactiveCommand<NpcViewModel, Unit> Delete { get; set; }
        public ReactiveCommand<NpcViewModel, Unit> OpenTextEditorForNpcDescriptions { get; set; }
        public ReactiveCommand RowClickedCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PopulateAllFieldsWithLorem { get; set; }

        public ReactiveCommand AddNewRow { get; set; }

        private void CheckScreenValid()
        {
            NPCInfoIsValid = !HasErrors && (Npcs.All(npc => !npc.HasErrors) || Npcs.Count == 0);
        }

        private void PopulateAllFieldsWithLoremHandler()
        {
            foreach (var npc in Npcs)
            {
                if (string.IsNullOrWhiteSpace(npc.BaseName))
                {
                    npc.BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames());
                }

                ((NpcViewModel) npc).InventoryDescription = Faker.Lorem.Sentence();

                npc.Descriptions[0] = Faker.Lorem.Paragraph();

                // Force a refresh
                npc.Descriptions = npc.Descriptions;
            }
        }

        private IEnumerable<string> GetBaseNames()
        {
            return Npcs.Select(placeableObject => placeableObject.BaseName);
        }

        public bool NPCInfoIsValid
        {
            get => _npcInfoIsValid;
            set => this.RaiseAndSetIfChanged(ref _npcInfoIsValid, value);
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

        public NpcsViewModel()
        {
            OpenTextEditorForNpcDescriptions =
                ReactiveCommand.Create<NpcViewModel>(OpenDescriptionsEditorHandler);
            PopulateNpcsWithLorem =
                ReactiveCommand.Create<NpcViewModel>(PopulateNpcWithLoremHandler);
            PopulateNpc = ReactiveCommand.Create<NpcViewModel>(PopulateNpcHandler);
            Delete = ReactiveCommand.Create<NpcViewModel>(DeleteHandler);
            AddNewRow = ReactiveCommand.Create(AddNewRowHandler);
            RowClickedCommand = ReactiveCommand.Create(RowClickedCommandHandler);
            PopulateAllFieldsWithLorem = ReactiveCommand.Create(PopulateAllFieldsWithLoremHandler);


            Npcs.ChangeTrackingEnabled = true;
            Disposables.Add(Npcs.Changed.Subscribe(_ => CheckScreenValid()));

            Disposables.Add(Npcs.ItemChanged.Subscribe(_ => CheckScreenValid()));

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
            if (GridSelectedNpc != null)
            {
                OpenDescriptionsEditorHandler(GridSelectedNpc);
            }
        }

        private void OpenDescriptionsEditorHandler(NpcViewModel selectedNpc)
        {
            var editor = new MultiTextEditor(selectedNpc.Descriptions, false, "Descriptions")
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            editor.ShowDialog();

            if (!editor.OkPressed)
            {
                return;
            }

            selectedNpc.Descriptions = new ReactiveList<string>(editor.TextValues);
        }

        private void DeleteHandler(NpcViewModel selectedNpc)
        {
            Npcs.Remove(selectedNpc);
        }

        private void PopulateNpcHandler(NpcViewModel selectedNpc)
        {
            selectedNpc.BaseName = _names[NextIndex];
            selectedNpc.Descriptions[0] = _descriptions[NextIndex];
            selectedNpc.InventoryDescription = _descriptions[NextIndex];

            // Force a refresh
            selectedNpc.Descriptions = selectedNpc.Descriptions;
            IncrementIndex();
        }

        private void AddNewRowHandler()
        {
            Npcs.Add(new NpcViewModel(Npcs)
            {
                BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames())
            });

            Npcs.Last().Descriptions.Add(string.Empty);
        }

        private void PopulateNpcWithLoremHandler(NpcViewModel selectedNpc)
        {
            if (string.IsNullOrWhiteSpace(selectedNpc.BaseName))
            {
                selectedNpc.BaseName = PopulateBaseName(ObjectNameStub, GetBaseNames());
            }

            selectedNpc.InventoryDescription = Faker.Lorem.Sentence();

            selectedNpc.Descriptions[0] = Faker.Lorem.Paragraph();
     
            // Force a refresh
            selectedNpc.Descriptions = selectedNpc.Descriptions;
        }

        public NpcViewModel SelectedNpc
        {
            get => _selectedNpc;
            set => this.RaiseAndSetIfChanged(ref _selectedNpc, value);
        }

        public NpcViewModel GridSelectedNpc
        {
            get => _gridSelectedNpc;
            set => this.RaiseAndSetIfChanged(ref _gridSelectedNpc, value);
        }

        public ReactiveList<AdventureObjectViewModel> Npcs
        {
            get => _npcs;
            set => this.RaiseAndSetIfChanged(ref _npcs, value);
        }
    }
}