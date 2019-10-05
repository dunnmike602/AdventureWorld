using System.Diagnostics;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Services.Data;
using DiagramDesigner.AdventureWorld.GameExecution.Interfaces;

namespace DiagramDesigner.AdventureWorld.GameExecution
{
    public class ConsoleExecutor : IGameExecutor
    {
        public void Execute(GameInformation information)
        {
            var consoleGameInformation = (ConsoleGameInformation) information;

            IGameConfigurationPersistance gameConfigurationPersistance = new ConsoleGameConfigurationPersister();

            gameConfigurationPersistance.SaveGameData(
                gameConfigurationPersistance.SerializeToString(consoleGameInformation.ModelData),
                consoleGameInformation.GameFullPath);

            var scriptArgments = string.Empty;

            if (!string.IsNullOrWhiteSpace(consoleGameInformation.TestScript?.Source))
            {
                gameConfigurationPersistance.SaveGameData(consoleGameInformation.TestScript.Source,
                    consoleGameInformation.TestScriptFullPath);

                scriptArgments += " --python ";
                scriptArgments += @" """ + consoleGameInformation.TestScriptFullPath + @""" ";
            }

            var exeName = consoleGameInformation.ClientExe;

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = @"/K  """"" + exeName + @""" " + @" """ + consoleGameInformation.GameFullPath + @""" " +
                            scriptArgments + @""""
            };

            using (var game = new Process())
            {
                game.StartInfo = startInfo;
                game.Start();
            }
        }
    }
}
