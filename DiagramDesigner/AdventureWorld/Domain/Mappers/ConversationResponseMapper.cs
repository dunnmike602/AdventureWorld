using System.Collections.Generic;
using System.Linq;

namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class ConversationResponseMapper
    {
        public static List<AdventureLandCore.Domain.ConversationResponse> MapMany(List<ConversationResponse> conversationResponses)
        {
            return conversationResponses.Select(MapOne).ToList();
        }

        public static AdventureLandCore.Domain.ConversationResponse MapOne(ConversationResponse conversationResponse)
        {
           return new AdventureLandCore.Domain.ConversationResponse
                {
                    Id = conversationResponse.ControlId,
                    Response = conversationResponse.Response,
                    SortOrder = conversationResponse.SortOrder,
                    ConversationText = ConversationTextMapper.MapOne(conversationResponse.ConversationText),
                    ConversationPreprocessScript = conversationResponse.ConversationPreprocessScript,
            };
        }
    }
}