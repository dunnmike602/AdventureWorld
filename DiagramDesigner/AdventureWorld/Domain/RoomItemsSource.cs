using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public class RoomItemsSource : IItemsSource
    {
        public ItemCollection GetValues()
        {
            var items = new ItemCollection();

            if (AdventureGameDesignerViewModel.Instance.RoomsList.Count > 0)
            {
                items.Add(null, string.Empty);
            }

            foreach (var room in AdventureGameDesignerViewModel.Instance.RoomsList)
            {
                items.Add(room, room.BaseName);
            }

            return items;
        }
    }
}