using System.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public class DirectionItemsSource : IItemsSource
    {
        private readonly Exit _currentExit = AdventureDesigner.Instance.GetSelectedObject() as Exit;

        private string[] GetUsedExits()
        {
            if (_currentExit == null)
            {
                return new string[0];
            }

            var containingRoom = AdventureGameDesignerViewModel.Instance.Rooms
                .FirstOrDefault(room => room.Exits != null && room.Exits.Any(exit => exit?.ControlId == _currentExit.ControlId));

            return containingRoom?.Exits.Select(exit => exit.Direction).ToArray() ?? new string[0];
        }

        public ItemCollection GetValues()
        {
            var usedExits = GetUsedExits();

            var items = new ItemCollection();

            foreach (var direction in AdventureGameDesignerViewModel.Instance.DirectionMappings)
            {
                if (usedExits.All(usedDirection => usedDirection != direction.Value.Text) || direction.Value.Text == _currentExit.Direction)
                {
                    items.Add(direction.Value.Text, direction.Value.Text);
                }
            }

            return items;
        }
    }
}