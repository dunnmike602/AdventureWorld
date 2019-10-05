using System.Linq;

namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class NpcMapper
    {
        public static AdventureLandCore.Domain.Npc MapOne(Npc source)
        {
            var target = new AdventureLandCore.Domain.Npc
            {
                Id = source.ControlId,
                IsLightSource = source.IsLightSource,
                IsLit = source.IsLit,
                Fixed = source.Fixed,
                HideFromAutoList = source.HideFromAutoList,
                Descriptions = source.Descriptions.ToArray(),
                Name = source.BaseName,
                ParentId = source.Parent?.ControlId,
                IsHeld = source.IsHeld,
                PreProcessScript = source.ObjectPreprocessScript,
                Visible = source.Visible,
                InventoryDescription = source.InventoryDescription,
                IsOpen = source.IsOpen,
                IsLocked = source.IsLocked,
                Category = source.Category,
                ShowRandomDescription = source.ShowRandomDescription,
                AutoFollow = source.AutoFollow,
                CurrentConversation = source.CurrentConversation,
                Conversations = ConversationTreeMapper.MapMany(source.ConversationTree)
            };

            return target;
        }
    }
}