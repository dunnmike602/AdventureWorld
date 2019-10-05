using System;
using System.IO;
using AdventureLandCore.Domain;
using AdventureLandCore.Services.Data;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.AdventureWorld.GameExecution.Interfaces;

namespace DiagramDesigner.AdventureWorld.GameExecution
{
    public static class ExecutorFactory
    {
        private static IGameExecutor CreateInstance(ClientType clientType)
        {
            switch (clientType)
            {
                case ClientType.ConsoleApplication:
                    return new ConsoleExecutor();
                case ClientType.GameExplorer:
                    return new WpfExecutor();
                default:
                    throw new ArgumentOutOfRangeException(nameof(clientType));
            }
        }

        private static GameInformation CreateParameters(ClientType clientType, AdventureGameSetup adventureGameSetup, Script testScript)
        {
            switch (clientType)
            {
                case ClientType.GameExplorer:
                    return new ConsoleGameInformation
                    {
                        ClientExe = Options.Instance.GameExplorerClientExe,
                        GameFullPath = Path.ChangeExtension(
                            Path.Combine(Options.Instance.TempGameDirectory, adventureGameSetup.GameName ?? "TEMPGAME"),
                            GlobalConstants.GameExtension),
                        ModelData = adventureGameSetup,
                    };
                case ClientType.ConsoleApplication:
                    return new ConsoleGameInformation
                    {
                        ClientExe = Options.Instance.ConsoleClientExe,
                        GameFullPath = Path.ChangeExtension(
                            Path.Combine(Options.Instance.TempGameDirectory, adventureGameSetup.GameName ?? "TEMPGAME"),
                            GlobalConstants.GameExtension),
                        ModelData = adventureGameSetup,
                        TestScript = testScript,
                        TestScriptFullPath =
                            testScript == null
                                ? string.Empty
                                : Path.Combine(Options.Instance.TempGameDirectory,
                                    "TEMP.py")

                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(clientType));
            }
        }

        public static void ExecuteGame(AdventureGameSetup adventureGameSetup, ClientType clientType, Script testScript)
        {
            var info = CreateParameters(clientType, adventureGameSetup, testScript);
            var executor = CreateInstance(clientType);

            executor.Execute(info);
        }
    }
}