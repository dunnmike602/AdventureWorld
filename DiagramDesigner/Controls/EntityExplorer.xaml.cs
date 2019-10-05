using System.Reactive;
using System.Windows;
using DiagramDesigner.AdventureWorld.Domain;
using ReactiveUI;

namespace DiagramDesigner.Controls
{
    public partial class EntityExplorer
    {
        public ReactiveCommand PreviousRoomCommand
        {
            get => (ReactiveCommand)GetValue(PreviousRoomCommandProperty);
            set => SetValue(PreviousRoomCommandProperty, value);
        }

        public static readonly DependencyProperty PreviousRoomCommandProperty =
            DependencyProperty.Register("PreviousRoomCommand", typeof(ReactiveCommand), typeof(EntityExplorer),
                new PropertyMetadata(null));

        public ReactiveCommand NextRoomCommand
        {
            get => (ReactiveCommand)GetValue(NextRoomCommandProperty);
            set => SetValue(NextRoomCommandProperty, value);
        }

        public static readonly DependencyProperty NextRoomCommandProperty =
            DependencyProperty.Register("NextRoomCommand", typeof(ReactiveCommand), typeof(EntityExplorer),
                new PropertyMetadata(null));

        public ReactiveCommand<Container, Unit> OpenContainerCommand
        {
            get => (ReactiveCommand<Container, Unit>)GetValue(OpenContainerCommandProperty);
            set => SetValue(OpenContainerCommandProperty, value);
        }

        public static readonly DependencyProperty OpenContainerCommandProperty =
            DependencyProperty.Register("OpenContainerCommand", typeof(ReactiveCommand<Container, Unit>), typeof(EntityExplorer),
                new PropertyMetadata(null));

        public ReactiveCommand<Exit, Unit> ExitClickedCommand
        {
            get => (ReactiveCommand<Exit, Unit>)GetValue(ExitClickedCommandProperty);
            set => SetValue(ExitClickedCommandProperty, value);
        }

        public static readonly DependencyProperty ExitClickedCommandProperty =
            DependencyProperty.Register("ExitClickedCommand", typeof(ReactiveCommand<Exit, Unit>), typeof(EntityExplorer),
                new PropertyMetadata(null));
        
        public ReactiveCommand<AdventureObjectBase, Unit> EntityClickedCommand
        {
            get => (ReactiveCommand<AdventureObjectBase, Unit>)GetValue(EntityClickedCommandProperty);
            set => SetValue(EntityClickedCommandProperty, value);
        }

        public static readonly DependencyProperty EntityClickedCommandProperty =
            DependencyProperty.Register("EntityClickedCommand", typeof(ReactiveCommand<AdventureObjectBase, Unit>), typeof(EntityExplorer),
                new PropertyMetadata(null));

        public ReactiveCommand<Container, Unit> ContainerNavigateCommand
        {
            get => (ReactiveCommand<Container, Unit>)GetValue(ContainerNavigateCommandProperty);
            set => SetValue(ContainerNavigateCommandProperty, value);
        }

        public static readonly DependencyProperty ContainerNavigateCommandProperty =
            DependencyProperty.Register("ContainerNavigateCommand", typeof(ReactiveCommand<Container, Unit>), typeof(EntityExplorer),
                new PropertyMetadata(null));
        
        public Room CurrentRoom
        {
            get => (Room)GetValue(CurrentRoomProperty);
            set => SetValue(CurrentRoomProperty, value);
        }

        public static readonly DependencyProperty CurrentRoomProperty = DependencyProperty.Register("CurrentRoom", typeof(Room),
            typeof(EntityExplorer), new PropertyMetadata(null));

        public Container CurrentContainer
        {
            get => (Container)GetValue(CurrentContainerProperty);
            set => SetValue(CurrentContainerProperty, value);
        }

        public static readonly DependencyProperty CurrentContainerProperty = DependencyProperty.Register("CurrentContainer", typeof(Container),
            typeof(EntityExplorer), new PropertyMetadata(null));

        public EntityExplorer()
        {
            InitializeComponent();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EntityClickedCommand = ReactiveCommand.Create<AdventureObjectBase>(RoomClickedCommandHandler);
            ContainerNavigateCommand  = ReactiveCommand.Create<Container>(ContainerNavigateCommandHandler);
            ExitClickedCommand = ReactiveCommand.Create<Exit>(ExitClickedCommandHandler);
            OpenContainerCommand = ReactiveCommand.Create<Container>(OpenContainerCommandHandler);
            PreviousRoomCommand = ReactiveCommand.Create(CurrentRommCommandCommandHandler);
            NextRoomCommand = ReactiveCommand.Create(NextRoomCommandCommandHandler);
        }
        
        private void ContainerNavigateCommandHandler(Container container)
        {

        }

        private void CurrentRommCommandCommandHandler()
        {

        }

        private void NextRoomCommandCommandHandler()
        {

        }

        private void OpenContainerCommandHandler(Container container)
        {
           
        }

        private void ExitClickedCommandHandler(Exit exit)
        {
        }

        private void RoomClickedCommandHandler(AdventureObjectBase adventureObject)
        {
        }
    }
}
