using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using SharedControls;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace DiagramDesigner.Wizards
{
    public class RoomsViewModel : ValidatableViewModelBase
    {
        private const string RoomNameStub = "ROOM";

        private readonly string[] _names = new[] { "Cave",  "Jungle Clearing", "Bridge", "Conservatory", "Kitchen", "Tunnel" };

        private readonly string[] _descriptions = 
            new[] { "You are standing in a dank cave, the walls are covered with luminous fungus, stalactites hang from the ceiling.",
                 "You are standing in a clearing in a hot and steaming jungle. Sweat pours off you, all around is danger.",
                "You are on the Bridge of a Pirate ship. It is deserted apart from the dead body lashed to the wheel.",
            "You are in a sunny conservatory, there are signs of a struggle with a broken table and spilled tea.",
                "You are in the kitchen from hell, strange smells enanate from large pots bubbling away on stoves.",
                "You are standing in a dark, dank tunnel which runs further in to the mountain and seems to go on for ever." };

        private ReactiveList<AdventureObjectViewModel> _rooms = new ReactiveList<AdventureObjectViewModel>();
        private RoomViewModel _selectedRoom;
        private bool _roomInfoIsValid;
        private RoomViewModel _gridSelectedRoom;

        public ReactiveCommand<RoomViewModel, Unit> PopulateRoomWithLorem { get; set; }
        public ReactiveCommand<RoomViewModel, Unit> PopulateRoom { get; set; }
        public ReactiveCommand<RoomViewModel, Unit> Delete { get; set; }
        public ReactiveCommand<RoomViewModel, Unit> OpenTextEditorForRoomDescriptions { get; set; }
        public ReactiveCommand RowClickedCommand { get; set; }
        public ReactiveCommand PopulateAllFieldsWithLorem { get; set; }
        public ReactiveCommand AddNewRow { get; set; }

        private void CheckScreenValid()
        {
            RoomInfoIsValid = !HasErrors && Rooms.All(room => !room.HasErrors);
        }

        public bool RoomInfoIsValid
        {
            get => _roomInfoIsValid;
            set => this.RaiseAndSetIfChanged(ref _roomInfoIsValid, value);
        }
        
        public RoomsViewModel()
        {
            OpenTextEditorForRoomDescriptions = ReactiveCommand.Create<RoomViewModel>(OpenDescriptionsEditorHandler);
            PopulateRoomWithLorem = ReactiveCommand.Create<RoomViewModel>(PopulateRoomWithLoremHandler);
            PopulateRoom = ReactiveCommand.Create<RoomViewModel>(PopulateRoomHandler);
            Delete = ReactiveCommand.Create<RoomViewModel>(DeleteHandler);
            AddNewRow = ReactiveCommand.Create(AddNewRowHandler);
            RowClickedCommand = ReactiveCommand.Create(RowClickedCommandHandler);
            PopulateAllFieldsWithLorem = ReactiveCommand.Create(PopulateAllFieldsWithLoremHandler);

            Rooms.ChangeTrackingEnabled = true;

            SetARoom();

            Disposables.Add(Rooms.Changed.Subscribe(_ => CheckScreenValid()));

            Disposables.Add(Rooms.ItemChanged.Subscribe(_ => CheckScreenValid()));

            Disposables.Add(Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => PropertyChanged += h,
                    h => PropertyChanged -= h
                )
                .Subscribe(_ => CheckScreenValid()));
        }

        private void SetARoom()
        {
            AddNewRowHandler();

            SelectedRoom = Rooms[0] as RoomViewModel;
        }

        private void PopulateAllFieldsWithLoremHandler()
        {
            foreach (var room in Rooms)
            {
                if (string.IsNullOrWhiteSpace(room.BaseName))
                {
                    room.BaseName = PopulateBaseName(RoomNameStub, GetBaseNames());
                }

                room.Descriptions[0] = Faker.Lorem.Paragraph();

                // Force a refresh
                room.Descriptions = room.Descriptions;
            }
        }

        private void RowClickedCommandHandler()
        {
            if (GridSelectedRoom != null)
            {
                OpenDescriptionsEditorHandler(GridSelectedRoom);
            }
        }

        private IEnumerable<string> GetBaseNames()
        {
            return Rooms.Select(room => room.BaseName);
        }

        private void OpenDescriptionsEditorHandler(RoomViewModel currentRoom)
        {
            var editor = new MultiTextEditor(currentRoom.Descriptions, false, "Descriptions")
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            editor.ShowDialog();

            if (!editor.OkPressed)
            {
                return;
            }

            currentRoom.Descriptions = new ReactiveList<string>(editor.TextValues);
        }

        private void DeleteHandler(RoomViewModel selectedRoom)
        {
            Rooms.Remove(selectedRoom);
        }

        private void PopulateRoomHandler(RoomViewModel selectedRoom)
        {
            selectedRoom.BaseName = _names[NextIndex];
            selectedRoom.Descriptions[0] = _descriptions[NextIndex];

            // Force a refresh
            selectedRoom.Descriptions = selectedRoom.Descriptions;
            IncrementIndex();
        }

        private void AddNewRowHandler()
        {
            Rooms.Add(new RoomViewModel(Rooms)
                { BaseName = PopulateBaseName(RoomNameStub, GetBaseNames())});

            Rooms.Last().Descriptions.Add(string.Empty);
        }

        private void PopulateRoomWithLoremHandler(RoomViewModel selectedRoom)
        {
            if (string.IsNullOrWhiteSpace(selectedRoom.BaseName))
            {
                selectedRoom.BaseName = PopulateBaseName(RoomNameStub, GetBaseNames());
            }

            selectedRoom.Descriptions[0] = Faker.Lorem.Paragraph();
     
            // Force a refresh
            selectedRoom.Descriptions = selectedRoom.Descriptions;
        }

        [Required(ErrorMessage="You must specify the location where the game starts.")]
        public RoomViewModel SelectedRoom
        {
            get => _selectedRoom;
            set => this.RaiseAndSetIfChanged(ref _selectedRoom, value);
        }

        public RoomViewModel GridSelectedRoom
        {
            get => _gridSelectedRoom;
            set => this.RaiseAndSetIfChanged(ref _gridSelectedRoom, value);
        }

        public ReactiveList<AdventureObjectViewModel> Rooms
        {
            get => _rooms;
            set => this.RaiseAndSetIfChanged(ref _rooms, value);
        }
    }
}