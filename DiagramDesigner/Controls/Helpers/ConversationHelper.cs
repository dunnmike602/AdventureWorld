using System.Linq;
using System.Windows;
using DiagramDesigner.AdventureWorld.Domain;

namespace DiagramDesigner.Controls.Helpers
{
    public static class ConversationHelper
    {
        public static void OpenEditor(Npc currentNpc)
        {
            var conversationDesigner = Application.Current.Windows.OfType<ConversationDesigner.ConversationDesigner>().FirstOrDefault(win => win.CurrentNpc.ControlId == currentNpc.ControlId);

            if (conversationDesigner == null)
            {
                conversationDesigner = new ConversationDesigner.ConversationDesigner { CurrentNpc = currentNpc };
                conversationDesigner.Show();
                conversationDesigner.LoadData(currentNpc.ControlId);
            }
            else
            {
                conversationDesigner.Focus();
            }
        }
    }
}
