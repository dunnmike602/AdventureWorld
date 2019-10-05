namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class ConversationMapper
    {
        public static AdventureLandCore.Domain.Conversation MapOne(Conversation sourceConversation)
        {
            if (sourceConversation == null)
            {
                return null;
            }
            else
            {
                return new AdventureLandCore.Domain.Conversation
                {
                    Name = sourceConversation.BaseName,
                    Id = sourceConversation.ControlId,
                    EscapeText = sourceConversation.EscapeText,
                    ConversationText = ConversationTextMapper.MapOne(sourceConversation.ConversationText),
                    ConversationPreprocessScript = sourceConversation.ConversationPreprocessScript,
                };
            }
        }
    }
}