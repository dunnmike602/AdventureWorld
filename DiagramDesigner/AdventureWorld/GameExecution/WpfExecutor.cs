using System.Diagnostics;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Services.Data;
using DiagramDesigner.AdventureWorld.GameExecution.Interfaces;

namespace DiagramDesigner.AdventureWorld.GameExecution
{
    public class WpfExecutor : IGameExecutor
    {
        public void Execute(GameInformation information)
        {
            var consoleGameInformation = (ConsoleGameInformation)information;

            IGameConfigurationPersistance gameConfigurationPersistance = new ConsoleGameConfigurationPersister();

            gameConfigurationPersistance.SaveGameData(
                gameConfigurationPersistance.SerializeToString(consoleGameInformation.ModelData),
                consoleGameInformation.GameFullPath);
            
            var exeName = consoleGameInformation.ClientExe;

            var startInfo = new ProcessStartInfo
            {
                FileName = consoleGameInformation.ClientExe,
                Arguments = "\"" +  consoleGameInformation.GameFullPath + "\"",
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = false,
                CreateNoWindow = false,
                
            };
           
            using (var game = new Process())
            {
                game.StartInfo = startInfo;
                game.Start();
            }
        }
    }
}