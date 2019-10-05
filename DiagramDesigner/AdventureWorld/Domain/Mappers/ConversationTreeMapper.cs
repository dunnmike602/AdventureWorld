using System.Collections.Generic;
using System.Linq;

namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class ConversationTreeMapper
    {
        // Avoid recursion by keeping a list of already created ConversationTexts
        internal static List<AdventureLandCore.Domain.ConversationText> ConversationTexts { get; set; } = new List<AdventureLandCore.Domain.ConversationText>();

        public static List<AdventureLandCore.Domain.Conversation> MapMany(ConversationTree sourceConversationTree)
        {
            ConversationTexts.Clear();
       
            return sourceConversationTree?.Conversations.Select(ConversationMapper.MapOne).ToList() ?? new List<AdventureLandCore.Domain.Conversation>();
        }
    }
}