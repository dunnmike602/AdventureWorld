using System;
using System.Linq;
using System.Runtime.Serialization;
using AdventureLandCore.Exceptions;
using AdventureLandCore.Extensions;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Scripting.Interfaces;
using AdventureLandCore.Services.CoreApi.Interfaces;
using AdventureLandCore.Services.Helpers;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Domain
{
    [IgnoreInObjectBrowser]
    [DataContract(IsReference = true)]
    public class AdventureGameEngine : IAdventureGameEngine
    {
        public event StateChangedEventHandler StateChanged;

        private void InvokeStateChanged(GameState gameState, Room currentLocation, string description)
        {
            var eventHandler = StateChanged;
            eventHandler?.Invoke(this,
                new GameStateEventArgs
                {
                    State = gameState,
                    CurrentLocation = currentLocation,
                    Description = description
                });
        }

        private readonly IConsole _console;
        private readonly IGameConfiguration _gameConfiguration;
        private readonly IAdventureApi _adventureApi;
        private readonly IExecutionEngine _executionEngine;
        private readonly ILanguageApi _languageApi;
        private string _scriptErrors;

        private string Title => _adventureApi.GameData.Title ?? string.Empty;

        public Player Player
        {
            get => GameData.Player;
            set => GameData.Player = value;
        }

        public int CurrentScore
        {
            get => GameData.CurrentScore;
            set => GameData.CurrentScore = value;
        }

        public Room Location
        {
            get => GameData.Location;
            set => GameData.Location = value;
        }

        public GameData GameData => _adventureApi.GameData;

        public GameState GetGameState()
        {
            if (Player.IsDestroyed)
            {
                return GameState.PlayerLost;
            }

            if (GameData.IsQuit)
            {
                return GameState.Quit;
            }

            return CurrentScore == _gameConfiguration.MaximumScore ? GameState.Won : GameState.Running;
        }

        public AdventureGameEngine(IConsole console,
            IGameConfiguration gameConfiguration, IAdventureApi adventureApi,
            IExecutionEngine executionEngine, ILanguageApi languageApi)
        { 
            _console = console;
            _gameConfiguration = gameConfiguration;
            _adventureApi = adventureApi;
            _executionEngine = executionEngine;
            _languageApi = languageApi;

            InvokeStateChanged(GameState.Initialising, null, $"Game {gameConfiguration.GameName} is initialising.");
        }

        private void RunInitialiseScripts()
        {
            var script = _gameConfiguration.GetScriptFromIdHelper(MessageIds.InitialisationCode);

            if (script != null)
            {
                _executionEngine.ExecuteLoopScript(script, GlobalConstants.InitScriptName);
            }
        }

        private void RunPreProcessScript()
        {
            var script = _gameConfiguration.GetScriptFromIdHelper(MessageIds.GameLoopPreProcessCode);

            if (script != null)
            {
                _executionEngine.ExecuteLoopScript(script, GlobalConstants.GameLoopPreScriptName);
            }
        }

        private void RunPostProcessScript()
        {
            var script = _gameConfiguration.GetScriptFromIdHelper(MessageIds.GameLoopPostProcessCode);

            if (script != null)
            {
                _executionEngine.ExecuteLoopScript(script, GlobalConstants.GameLoopPostScriptName);
            }

            GameData.IsFirstTimeThrough = false;
        }

        private void WriteLineWhenNotNull(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
               _console.FormattedWrite(text);
            }
        }

        private void WriteLineCenteredWhenNotNull(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _console.CenteredWrite(text);
            }
        }
        
        private bool RunTheCommand(ParsedAdventureCommand adventureCommand)
        {
            _adventureApi.LastExecutedCommand = adventureCommand;

            return AdventureCommandHelper.Execute(new ExecutionParameters
            {
                ConstructorParameters = new object[]
                    {this, _console, _gameConfiguration, _adventureApi, _executionEngine},
                TriggeringCommand = adventureCommand,
                GameConfiguration = _gameConfiguration
            }, _executionEngine);
        }

        private void CheckGameWon()
        {
            if (GetGameState() != GameState.Won || !_gameConfiguration.EnableScore)
            {
                return;
            }

            var wonMessage = string.Format(_gameConfiguration.WonGameMessage, CurrentScore, _gameConfiguration.MaximumScore);

            _console.FormattedWrite(wonMessage);

            ShowScoreHelper();
        }

        private void ShowScoreHelper()
        {
            if (_gameConfiguration.EnableScore)
            {
                _languageApi.ExecuteMethod(_gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode), "ShowScoreHelper");
            }
        }

        private void ExecuteShowItemsInRoomCommand()
        {
            if (Location == null)
            {
                throw new Exception("No Current Location Set.");
            }

            if (GameData.EnableShowItemsInRoom)
            {
                _adventureApi.ShowRoomInformation();

                _console.Write(string.Empty.AddLineBreaks());
            }
        }

        private void CheckPlayerLost()
        {
            if (GetGameState() != GameState.PlayerLost || !_gameConfiguration.EnablePlayerLost)
            {
                return;
            }

            _console.FormattedWrite(_gameConfiguration.PlayerLostMessage.PrefixLineBreaks().AddLineBreaks());

            ShowScoreHelper();
        }

        private void PerformInitialStorySetup()
        {
            WriteTheTitle();

            WriteTheIntroduction();

            ExecuteShowItemsInRoomCommand();
        }

        private void WriteTheIntroduction()
        {
            if (_gameConfiguration.EnableTitles)
            {
                WriteLineWhenNotNull(_gameConfiguration.Introduction.AddLineBreaks());
            }
        }

        private void WriteTheTitle()
        {
            if (_gameConfiguration.EnableTitles)
            {
                _console.WriteLine(string.Empty.AddLineBreaks(2));
                WriteLineCenteredWhenNotNull(Title);
                _console.WriteLine(string.Empty.AddLineBreaks(2));
            }
        }

        public void RunTheGameLoop()
        {
            do
            {
                InvokeStateChanged(GameState.Running, Location, $"Current location is {Location.Name}");

                RunPreProcessScript();

                _adventureApi.LastPlayerInput = _console.ReadLine(_gameConfiguration.CommandPromptText).Trim().ToUpper();

                InvokeStateChanged(GameState.Running, null, $"Player entered: {_adventureApi.LastPlayerInput}");

                if (GetGameState() != GameState.Running)
                {
                    continue;
                }

                var startRoom = Location.Name;
                var startDarkState = Location.IsDark;

                _adventureApi.LastParsedCommand = _languageApi.ExecuteInputProcessor(_gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode),
                    _adventureApi.LastPlayerInput);
                
                if (_adventureApi.LastParsedCommand?.ParsedCommand == null || !RunTheCommand(_adventureApi.LastParsedCommand))
                {
                    continue;
                }

                RunPostProcessScript();

                if (startRoom != Location.Name || (startRoom == Location.Name && startDarkState != Location.IsDark))
                {
                    _languageApi.MoveAutoFollowNpcs(_gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode));

                    ExecuteShowItemsInRoomCommand();
                }

            } while (GetGameState() == GameState.Running);

            CheckPlayerLost();

            CheckGameWon();
        }
        
        public void InitialiseCore()
        {
            _adventureApi.GameData.IsFirstTimeThrough = true;

            GameData.IsQuit = false;

            Player = new Player();

            CompileAllScripts();
        }

        public void InitialiseTheGame()
        {
            if (!string.IsNullOrWhiteSpace(_gameConfiguration.ConsoleLogFile))
            {
                _console.TurnLoggingOn(_gameConfiguration.ConsoleLogFile);
            }

            RunInitialiseScripts();

            PerformInitialStorySetup();

            InvokeStateChanged(GameState.InitialisationComplete, null, "All scripts and setup is now complete.");
        }

        private void CompileAllScripts()
        {
            _scriptErrors = string.Empty;

            CompileInitialisationScript();

            InvokeStateChanged(GameState.ScriptsCompiled, null, string.IsNullOrWhiteSpace(_scriptErrors) ? "Init Scripts compiled successfully" : "Init Scripts compiled with errors.");

            CompilePreProcessScript();

            InvokeStateChanged(GameState.ScriptsCompiled, null, string.IsNullOrWhiteSpace(_scriptErrors) ? "Pre Process Scripts compiled successfully" : "Pre Process Scripts compiled with errors.");

            CompilePostProcessScript();

            InvokeStateChanged(GameState.ScriptsCompiled, null, string.IsNullOrWhiteSpace(_scriptErrors) ? "Post Process Scripts compiled successfully" : "Post Process Scripts compiled with errors.");

            CompileRoomScripts();

            InvokeStateChanged(GameState.ScriptsCompiled, null, string.IsNullOrWhiteSpace(_scriptErrors) ? "Room Scripts compiled successfully" : "Room compiled with errors.");

            CompileObjectScripts();

            InvokeStateChanged(GameState.ScriptsCompiled, null, string.IsNullOrWhiteSpace(_scriptErrors) ? "Object Scripts compiled successfully" : "Object compiled with errors.");

            CompileCommandScripts();

            InvokeStateChanged(GameState.ScriptsCompiled, null, string.IsNullOrWhiteSpace(_scriptErrors) ? "Command Scripts compiled successfully" : "Command Scripts compiled with errors.");

            CompileCommonScript();

            InvokeStateChanged(GameState.ScriptsCompiled, null, string.IsNullOrWhiteSpace(_scriptErrors) ? "Common Scripts compiled successfully" : "Common Scripts compiled with errors.");

            CompileConversationsScripts();

            InvokeStateChanged(GameState.ScriptsCompiled, null, string.IsNullOrWhiteSpace(_scriptErrors) ? "Conversation Scripts compiled successfully" : "Conversation Scripts compiled with errors.");

            if (!string.IsNullOrWhiteSpace(_scriptErrors))
            {
                throw new ScriptCompileException(("FATAL SCRIPT COMPILATION ERRORS:".AddLineBreaks() + _scriptErrors).Trim());
            }
        }

        private void CompileConversationsScripts()
        {
            var allNpcs = _gameConfiguration.PlaceableObjects.Where(obj => obj.IsNpc).Cast<Npc>();

            var allConversations = allNpcs.SelectMany(npc => npc.Conversations).ToList();

            foreach (var conversation in allConversations)
            {
                ProcessConversationScriptHelper(conversation.ConversationPreprocessScript);

                ProcessConversationText(conversation.ConversationText);
            }
        }

        private void ProcessConversationText(ConversationText conversationText)
        {
            if (conversationText == null || conversationText.ConversationPreprocessScript.HasCompileErrors ||
                conversationText.ConversationPreprocessScript.CompiledSource != null)
            {
                return;
            }

            ProcessConversationScriptHelper(conversationText.ConversationPreprocessScript);

            foreach (var response in conversationText.ConversationResponses)
            {
                ProcessConversationScriptHelper(response.ConversationPreprocessScript);

                ProcessConversationText(response.ConversationText);
            }
        }

        private void ProcessConversationScriptHelper(Script script)
        {
            CompileScriptHelper(script, $"Conversation Pre-process script Id:{script.Id}", _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).
                Source);
        }

        private void CompileCommonScript()
        {
            CompileScriptHelper(_gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode),
                GlobalConstants.CommonCodeScriptName, string.Empty);
        }

        private void CompileCommandScripts()
        {
            foreach (var command in _gameConfiguration.CommandMappings)
            {
                CompileScriptHelper(command.ScriptCommand, $"Object {command.VerbName} Processing Script script",
                    _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).Source);

                command.ScriptCommand.ValidateCommandScript();
            }
        }

        private void CompileObjectScripts()
        {
            foreach (var placeableObject in GameData.PlaceableObjects)
            {
                CompileScriptHelper(placeableObject.PreProcessScript,
                    $"Object {placeableObject.Name} Pre-process script", _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).Source);
            }
        }

        private void CompileRoomScripts()
        {
            foreach (var room in GameData.Rooms)
            {
                CompileScriptHelper(room.PreProcessScript, $"Room {room.Name} Pre-process script", _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).Source);

                foreach (var exit in room.Exits)
                {
                    CompileScriptHelper(exit.PreProcessScript, $"{exit} Pre-process script", _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).Source);
                }
            }
        }

        private void CompileScriptHelper(Script script, string name, string globalCode)
        {
            _executionEngine.Compile(script, name, globalCode);

            if (!string.IsNullOrWhiteSpace(script.CompileErrors))
            {
                _scriptErrors += script.CompileErrors.AddLineBreaks();
            }
        }

        private void CompileInitialisationScript()
        {
            CompileScriptHelper(_gameConfiguration.GetScriptFromIdHelper(MessageIds.InitialisationCode),
                GlobalConstants.InitScriptName, _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).Source);
        }

        private void CompilePreProcessScript()
        {
            CompileScriptHelper(_gameConfiguration.GetScriptFromIdHelper(MessageIds.GameLoopPreProcessCode),
                GlobalConstants.GameLoopPreScriptName, _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).Source);
        }

        private void CompilePostProcessScript()
        {
            CompileScriptHelper(_gameConfiguration.GetScriptFromIdHelper(MessageIds.GameLoopPostProcessCode),
                GlobalConstants.GameLoopPostScriptName, _gameConfiguration.GetScriptFromIdHelper(MessageIds.CommonCode).Source);
        }
    }
}