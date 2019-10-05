using AdventureLandCore.Domain;

namespace AdventureLandCore.Scripting.Interfaces
{
    public interface IExecutionEngine
    {
        void ExecuteMethod(Script script, string methodName);

        bool ExecuteLoopScript(Script script, string name);

        void Compile(Script script, string name, string globalCode);

        bool ExecuteObjectScript(Script script,  AdventureObjectBase placeableObject, ParsedAdventureCommand adventureCommand);

        bool ExecuteCommandFunction(Script script, ParsedAdventureCommand adventureCommand);

        ParsedAdventureCommand ExecuteInputProcessor(Script script, string playerInput);

        void MoveAutoFollowNpcs(Script script);

        bool ExecuteConversationScript(Script script, Npc npc, ConversationObjectBase conversationObjectBase,
            ConversationStage conversationStage);
    }
}