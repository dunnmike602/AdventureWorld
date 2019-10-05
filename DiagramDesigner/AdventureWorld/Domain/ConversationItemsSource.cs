using System.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    public class ConversationItemsSource : IItemsSource
    {
        private readonly Npc _currentNpc = AdventureDesigner.Instance.GetSelectedObject() as Npc;
        
        public ItemCollection GetValues()
        {
            var items = new ItemCollection();

            if (_currentNpc?.ConversationTree?.Conversations != null)
            {
                items.Add(null, string.Empty);

                foreach (var conversation in _currentNpc.ConversationTree.Conversations.OrderBy(con => con.BaseName))
                {
                    items.Add(conversation.ControlId, conversation.BaseName);
                }
            }

            return items;
        }
    }
}