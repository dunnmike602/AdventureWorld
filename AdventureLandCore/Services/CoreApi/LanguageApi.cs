using System;
using System.Collections.Generic;
using System.Linq;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Interfaces;
using AdventureLandCore.Scripting.Interfaces;
using AdventureLandCore.Services.CoreApi.Interfaces;
using AdventureLandCore.Services.Data;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Services.CoreApi
{
    [IgnoreInObjectBrowser]
    public class LanguageApi : ILanguageApi
    {
        private readonly IAdventureApi _adventureApi;
        private readonly IGameConfiguration _gameConfiguration;
        private List<Tuple<CommandMapping, string>> _commandAliases;
        private List<Tuple<CommandMapping, string>> _commandSubsitutions;

        public IExecutionEngine ExecutionEngine { get; set; }

        public LanguageApi(IAdventureApi adventureApi, IGameConfiguration gameConfiguration)
        {
            _adventureApi = adventureApi;
            _gameConfiguration = gameConfiguration;
        }

        public IList<string> SplitSentence(string source)
        {
            return source.SplitSentence();
        }

        public void ExecuteMethod(Script script, string methodName)
        {
            ExecutionEngine.ExecuteMethod(script, methodName);
        }

        public ParsedAdventureCommand ExecuteInputProcessor(Script script, string playerInput)
        {
            return ExecutionEngine.ExecuteInputProcessor(script, playerInput);
        }

        public void MoveAutoFollowNpcs(Script script)
        {
            ExecutionEngine.MoveAutoFollowNpcs(script);
        }

        public bool ExecuteObjectScript(Script script, AdventureObjectBase adventureObject, ParsedAdventureCommand adventureCommand)
        {
            return ExecutionEngine.ExecuteObjectScript(script, adventureObject, adventureCommand);
        }

        public bool ExecuteConversationScript(Script script, Npc npc, ConversationObjectBase conversationObjectBase, ConversationStage conversationStage)
        {
            return ExecutionEngine.ExecuteConversationScript(script, npc, conversationObjectBase, conversationStage);
        }


        public IList<string> GetAllNamesInGame()
        {
            return _adventureApi.GameData.Rooms.Select(room => room.Name)
                .Union(_adventureApi.GameData.PlaceableObjects.Select(obj => obj.Name))
                .ToArray();
        }

        public SortedList<string, string> GetStopWords()
        {
            return _adventureApi.GameData.StopWords;
        }

        public bool IsStopWord(string wordToCheck)
        {
            return _adventureApi.GameData.StopWords.ContainsKey(wordToCheck.ToUpper());
        }

        public IList<GameWord> ConvertSentence(IList<string> wordList)
        {
            EnsureCommandAliasMapping();

            EnsureCommandSubstitutionMapping();

            return GetCommandWords(wordList);
        }

        public bool CheckSentenceAgainstList(string sourceSentence, IList<string> sentencesToCheck)
        {
            return sourceSentence.IsEqualToAny(sentencesToCheck);
        }

        private IList<GameWord> GetCommandWords(IList<string> wordList)
        {
            List<GameWord> AddReplacementCommand(Tuple<CommandMapping, string> commandReplacementMapping)
            {
                return new List<GameWord>
                {
                    new GameWord
                    {
                        Type = WordType.ReplacedCommand,
                        Word = commandReplacementMapping.Item1.VerbName,
                        OriginalWord = commandReplacementMapping.Item1.VerbName
                    },
                    new GameWord {Type = WordType.Unknown, Word = wordList[0], OriginalWord = wordList[0]}
                };
            }

            var gameWords = new List<GameWord>();

            if (wordList.Count == 1 && !IsStopWord(wordList[0]))
            {
                // Check for a single replaceable command if (and only if) the command is a single word
                var commandReplacementMapping = _commandSubsitutions.FirstOrDefault(replacement => wordList[0].IsEqualTo(replacement.Item2));

                if (commandReplacementMapping != null)
                {
                    gameWords.AddRange(AddReplacementCommand(commandReplacementMapping));

                    return gameWords;
                }
            }

            foreach (var currentWord in wordList)
            {
                var commandMapping = _commandAliases.FirstOrDefault(replacement => currentWord.IsEqualTo(replacement.Item2));
                var isStopWord = IsStopWord(currentWord);

                if (isStopWord)
                {
                    gameWords.Add(new GameWord
                    {
                        Type = WordType.StopWord,
                        Word = currentWord,
                        OriginalWord = currentWord
                    });

                    continue;
                }

                if (commandMapping == null)
                {
                    gameWords.Add(
                        new GameWord {Type = WordType.Unknown, Word = currentWord, OriginalWord = currentWord});
                    continue;
                }

                gameWords.Add(new GameWord
                {
                    Type = WordType.Command,
                    Word = commandMapping.Item1.VerbName,
                    OriginalWord = currentWord
                });

            }

            return gameWords;
        }

        private void EnsureCommandAliasMapping()
        {
            if (_commandAliases == null)
            {
                _commandAliases = _gameConfiguration.CommandMappings.SelectMany(cmds => cmds.GetAllAliases(), 
                    (cmd, alias) => new Tuple<CommandMapping, string>(cmd, alias)).ToList();
            }
        }

        private void EnsureCommandSubstitutionMapping()
        {
            if (_commandSubsitutions == null)
            {
                _commandSubsitutions = _gameConfiguration.CommandMappings.SelectMany(cmds => cmds.OneWordSubstitutionList,
                    (cmd, alias) => new Tuple<CommandMapping, string>(cmd, alias)).ToList();
            }
        }
        
        public string RemovePunctuation(string source, bool preserveQuotes)
        {
            return source.StripPunctuation(preserveQuotes);
        }

        public bool MatchesPercent(string source, string target, double pc = 100)
        {
            return source.MatchesPercent(target, pc);
        }
    }
}