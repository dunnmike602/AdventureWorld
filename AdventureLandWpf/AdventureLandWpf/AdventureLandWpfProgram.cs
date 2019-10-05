using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using AdventureLandCore.Domain;
using AdventureLandCore.Exceptions;
using AdventureLandCore.Extensions;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Scripting;
using AdventureLandCore.Scripting.Interfaces;
using AdventureLandCore.Services;
using AdventureLandCore.Services.CoreApi;
using AdventureLandCore.Services.CoreApi.Interfaces;
using AdventureLandCore.Services.Data;
using AdventureLandCore.Services.Persistance;
using AdventureLandWpf.AdventureLandWpf.Consoles;
using RichTextBox = Xceed.Wpf.Toolkit.RichTextBox;

namespace DiagramDesigner.AdventureLandWpf
{
    public class AdventureLandWpfProgram
    {
        public IAdventureGameEngine AdventureGameEngine { get; set; }
        public IAdventureApi AdventureApi { get; set; }

        private IConsole _console;
        private IAdventureLexicalAnalyser _lexicalAnalyser;
        private IAdventureSyntaxAnalyser _syntaxAnalyser;
        private IGameConfiguration _gameConfiguration;
     
        private IExecutionEngine _executionEngine;
        private ILanguageApi _languageApi;
        private CancellationTokenSource _splashScreenCancellationToken;
        private Task _waitingToRunTask;
        private bool _stopTheGame;

        private void GameStateChanged(object sender, GameStateEventArgs e)
        {
            if (e.State == GameState.ScriptsCompiled)
            {
                _splashScreenCancellationToken.Cancel();
            }
        }

        private async Task ShowSplashScreen(string gameName)
        {
            _splashScreenCancellationToken = new CancellationTokenSource();

            await Task.Run(() => { ShowSplashScreenText(gameName, _splashScreenCancellationToken.Token); });

            _console.ClearScreen();

            AdventureGameEngine.InitialiseTheGame();
        }

        private void ShowSplashScreenText(string gameName, CancellationToken ctToken)
        {
            _console.WriteLine(string.Empty.AddLineBreaks(3));
            _console.WriteBanner("ADVENTURE", ConsoleColor.Red);
            _console.WriteBanner("    WORLD", ConsoleColor.Red);
            _console.WriteLine(string.Empty.AddLineBreaks());
            _console.WriteLine("    (c) MLD Computing Limited 2018", ConsoleColor.White);
            _console.WriteLine(string.Empty.AddLineBreaks(2));

            var startTime = DateTime.Now;

            ShowPleaseWaitMessage(gameName);

            var spinner = new SpinWait();
            
            while (!ctToken.IsCancellationRequested || DateTime.Now.Subtract(startTime).Seconds < 5)
            {
                spinner.SpinOnce();
            }
        }

        private void ShowPleaseWaitMessage(string gameName)
        {
            var textToAnimate = $"   Please wait while we get {gameName} ready ";

            _console.WriteLine(textToAnimate, ConsoleColor.Blue);
        }

        public void Initialise(AdventureGameSetup adventureGameSetup, TextBox title, RichTextBox richTextBox,
            TextBox input, TextBox prompt)
        {
            _console = new RichTextConsole(title, richTextBox, input, prompt);

            _gameConfiguration = new GameConfiguration(adventureGameSetup);

            AdventureApi = new AdventureApi(_gameConfiguration, new FilePersister(), _console);

            _waitingToRunTask = ShowSplashScreen(adventureGameSetup.GameName);

            _languageApi = new LanguageApi(AdventureApi, _gameConfiguration);

            _executionEngine = new PythonExecutionEngine(AdventureApi, _console, _languageApi);

            ((LanguageApi) _languageApi).ExecutionEngine = _executionEngine;

            _syntaxAnalyser = new AdventureSyntaxAnalyser(_gameConfiguration, _languageApi);
            _lexicalAnalyser = new AdventureLexicalAnalyser(_languageApi);

            AdventureGameEngine = new AdventureGameEngine(_console, _lexicalAnalyser, _syntaxAnalyser,
                _gameConfiguration,
                AdventureApi, _executionEngine);

            AdventureGameEngine.StateChanged += GameStateChanged;
        }

        public void InitialiseGameCore()
        {
            AdventureGameEngine.InitialiseCore();
        }

        public void Execute()
        {
            try
            {
                RunGame();
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
        
        private void RunGame()
        {
            string response;

            _stopTheGame = false;
            do
            {
                if (_stopTheGame)
                {
                    return;
                }

                _waitingToRunTask.Wait();

                AdventureGameEngine.RunTheGameLoop();

                _console.Write("Would you like another game? ");
                response = _console.ReadLine(_gameConfiguration.CommandPromptText);

            } while (response?.ToUpper()[0] == 'Y' && !_stopTheGame);
        }
    }
 }
