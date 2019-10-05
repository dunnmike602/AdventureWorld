using System.Linq;

namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class ConversationTextMapper
    {
        public static AdventureLandCore.Domain.ConversationText MapOne(ConversationText sourceConversationText)
        {
            if (sourceConversationText == null)
            {
                return null;
            }
            
            var existingConversationText = ConversationTreeMapper.ConversationTexts.FirstOrDefault(conversationText =>
                conversationText.Id == sourceConversationText.ControlId);

            if (existingConversationText != null)
            {
                return existingConversationText;
            }

            var newConversationText = new AdventureLandCore.Domain.ConversationText
            {
                Id = sourceConversationText.ControlId,
                Text = sourceConversationText.Text,
                ConversationPreprocessScript = sourceConversationText.ConversationPreprocessScript,
            };

            ConversationTreeMapper.ConversationTexts.Add(newConversationText);

            if (sourceConversationText.ConversationResponses != null && sourceConversationText.ConversationResponses.Any())
            {
                newConversationText.ConversationResponses = ConversationResponseMapper.MapMany(sourceConversationText.ConversationResponses);
            }

            return newConversationText;
        }
    }
}