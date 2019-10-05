using System;
using System.Threading;
using System.Threading.Tasks;
using AdventureLandCore.Domain;
using AdventureLandCore.Exceptions;
using AdventureLandCore.Extensions;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Scripting;
using AdventureLandCore.Scripting.Interfaces;
using AdventureLandCore.Services.CoreApi;
using AdventureLandCore.Services.CoreApi.Interfaces;
using AdventureLandCore.Services.Data;
using AdventureLandCore.Services.Helpers;
using AdventureLandCore.Services.Persistance;

namespace AdventureLand
{
    public class Program
    {
        private static IAdventureGameEngine _adventureGameEngine;
        private static IConsole _console;
        private static IGameConfiguration _gameConfiguration;
        private static IAdventureApi _adventureApi;
        private static IGameConfigurationPersistance _gameConfigurationPersistance;
        private static IExecutionEngine _executionEngine;
        private static ILanguageApi _languageApi;
        private static CancellationTokenSource _splashScreenCancellationToken;
        private static Task _waitingToRunTask;

        public static void InitialiseDependencies(string gamePath, bool isForFullGame)
        {
            _console = new CommandConsole();

            _gameConfigurationPersistance = new ConsoleGameConfigurationPersister();
            var adventureGameSetup = _gameConfigurationPersistance.LoadGameModel(gamePath);

            _gameConfiguration = new GameConfiguration(adventureGameSetup);

            _adventureApi = new AdventureApi(_gameConfiguration, new FilePersister(), _console);

            if (isForFullGame)
            {
                _waitingToRunTask = ShowSplashScreen(adventureGameSetup.GameName);
            }

            if (!isForFullGame)
            {
                adventureGameSetup.ClearAllScripts();

                _adventureApi.GameData.IsFirstTimeThrough = false;
                _adventureApi.GameData.EnableScore = false;
                _adventureApi.GameData.EnableShowItemsInRoom = false;
                _adventureApi.GameData.EnableInventorySize = false;
                _adventureApi.GameData.EnableTitles = false;
            }

            _languageApi = new LanguageApi(_adventureApi, _gameConfiguration);

            _executionEngine = new PythonExecutionEngine(_adventureApi, _console, _languageApi);

            ((LanguageApi) _languageApi).ExecutionEngine = _executionEngine;
            
            _adventureGameEngine = new AdventureGameEngine(_console, _gameConfiguration,
                _adventureApi, _executionEngine, _languageApi);
          
            if (isForFullGame)
            {
                _adventureGameEngine.StateChanged += GameStateChanged;
            }

            _adventureGameEngine.InitialiseCore();

            if (!isForFullGame)
            {
                _adventureGameEngine.InitialiseTheGame();
            }
        }

        private static void GameStateChanged(object sender, GameStateEventArgs e)
        {
            if (e.State == GameState.ScriptsCompiled)
            {
                _splashScreenCancellationToken.Cancel();
            }
        }

        private static async Task ShowSplashScreen(string gameName)
        {
            _splashScreenCancellationToken = new CancellationTokenSource();

            await Task.Run( () => { ShowSplashScreenText(gameName, _splashScreenCancellationToken.Token); });

            _console.ClearScreen();

            _adventureGameEngine.InitialiseTheGame();
        }

        private static void ShowSplashScreenText(string gameName, CancellationToken ctToken)
        {
            _console.WriteLine(string.Empty.AddLineBreaks(3));
            _console.WriteBanner("      ADVENTURE", ConsoleColor.Red);
            _console.WriteBanner("          WORLD", ConsoleColor.Red);
            _console.WriteLine(string.Empty.AddLineBreaks());
            _console.WriteLine("    (c) MLD Computing Limited 2018", ConsoleColor.White);
            _console.WriteLine(string.Empty.AddLineBreaks(2));

            var startTime = DateTime.Now;

            try
            {
                _console.HideCursor();

                while (!ctToken.IsCancellationRequested || DateTime.Now.Subtract(startTime).Seconds < 5)
                {
                    ShowBusySpinner(gameName);
                }
            }
            finally
            {
                _console.ShowCursor();
            }
        }

        private static void ShowBusySpinner(string gameName)
        {
            var spinWait = 90;

            var textToAnimate = $"   Please wait while we get {gameName} ready ";

            var cursorTop = _console.CursorTop;

            for (var i = 0; i < 15; i++)
            {
                _console.WriteLine(textToAnimate + new string('.', i).PadRight(15, ' '), ConsoleColor.Blue);

                _console.CursorTop = cursorTop;
                Thread.Sleep(spinWait);
            }
        }

        private static void Main(string[] args)
        {
            if (args == null || (args.Length != 3 && args.Length != 1) || (args.Length == 2 && (args[0].Trim().ToLower() != "--python")))
            {
                Console.WriteLine("Invalid Parameters, Usage:".AddLineBreaks());
                Console.WriteLine(@"AdventureLand ""<FilePath>/<FileName>.adl"" to run an Adventure Game");
                Console.WriteLine(@"AdventureLand ""<FilePath>/<FileName>.adl"" --python ""<FilePath>/<FileName>.py""  to test run a python script in the specified game");

                return;
            }

            try
            {
                if (args.Length == 1)
                {
                    RunGame(args);
                }
                else
                {
                    RunScript(args);
                }
            }
            catch (ScriptCompileException ex)
            {
                _console.ClearScreen();
                _console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                _console.ClearScreen();
                _console.WriteLine(ex.ToString());
            }
        }

        private static void RunScript(string[] args)
        {
            InitialiseDependencies(args[0], false);

            var fullFilePath = args[2];

            var script = new Script{Source = FileHelper.LoadDataFile(fullFilePath)};

            _executionEngine.Compile(script, fullFilePath, _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).Source);

            if (script.HasCompileErrors)
            {
                _console.WriteLine(script.CompileErrors, ConsoleColor.Red);
            }
            else
            {
                // TODO Needs to be context sensitive to type of script
                _executionEngine.ExecuteLoopScript(script, "Test Script");
            }
        }

        private static void RunGame(string[] args)
        {
            string response;

            do
            {
                InitialiseDependencies(args[0], true);

                _waitingToRunTask.Wait();

                _adventureGameEngine.RunTheGameLoop();

                _console.WriteLine(_gameConfiguration.AnotherGameText);

                response = _console.ReadLine(_gameConfiguration.CommandPromptText);

                _console.ClearScreen();

            } while (response?.ToUpper().Substring(0, _gameConfiguration.AnotherGameYesResponse.Length) == _gameConfiguration.AnotherGameYesResponse.ToUpper());
        }
    }
}