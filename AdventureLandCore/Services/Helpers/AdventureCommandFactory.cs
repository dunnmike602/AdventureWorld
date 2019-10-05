using AdventureLandCore.Domain;
using AdventureLandCore.Scripting.Interfaces;

namespace AdventureLandCore.Services.Helpers
{
    public static class AdventureCommandHelper
    {
        public static bool Execute(ExecutionParameters executionParameters, IExecutionEngine pythonExecutionEngine)
        {
            return pythonExecutionEngine.ExecuteCommandFunction(
                executionParameters.TriggeringCommand.CommandMapping.ScriptCommand,
                executionParameters.TriggeringCommand);
        }
    }
}