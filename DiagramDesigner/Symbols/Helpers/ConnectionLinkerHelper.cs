using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Extensions;

namespace DiagramDesigner.Symbols.Helpers
{
    internal static class ConnectionLinkerHelper
    {
        public static void ProcessConversationLink(Connector sourceConnector, Connector targetConnector)
        {
            var sourceConversationItem = ConversationDesigner.ConversationDesigner.Instance.GetConversationObject(sourceConnector
                .ParentDesignerItem.ID);

            var targetConversationItem = ConversationDesigner.ConversationDesigner.Instance.GetConversationObject(targetConnector
                .ParentDesignerItem.ID);

            sourceConversationItem.AddChild(targetConversationItem);
        }

        public static void ProcessExit(Connector sourceConnector, Connector targetConnector, Connection newConnection)
        {
            var exit = new Exit
            {
                ControlId = newConnection.ID,
                FromRoom = (Room)AdventureGameDesignerViewModel.Instance.FindObjectByGuid(sourceConnector.
                    ParentDesignerItem.ID),
                ToRoom = (Room)AdventureGameDesignerViewModel.Instance.FindObjectByGuid(targetConnector.
                    ParentDesignerItem.ID)
            };

            StencilHelpers.BindConnectorToObject(newConnection, exit, "Direction");

            AdventureGameDesignerViewModel.Instance.Exits.Add(exit);
            newConnection.ApplyTemplate();
            AdventureDesigner.ApplyExitTemplate(newConnection);
            newConnection.SetPropertyGridToCurrentSelectedItem();
            AdventureDesigner.Instance.ValidateExitItem(exit);
        }
    }
}