using System.Collections.Generic;
using AdventureLandCore.Domain;

namespace AdventureLandCore.Services.CoreApi.Interfaces
{
    /// <summary>
    /// This class is a helper class that has some useful methods if you want to write your own text process routines. Access to this object is via the variable LanguageApi. 
    /// </summary>
    public interface ILanguageApi
    {
        /// <summary>
        /// Executes a conversation script with the require parameters.
        /// </summary>
        /// <param name="script">Script with the Execute method in.</param>
        /// <param name="npc">The NPC in the conversation.</param>
        /// <param name="conversationObjectBase">The current conversation object.</param>
        /// <param name="conversationStage">The current conversation stage.</param>
        /// <returns>Return value of the script. Can be set to False to signal to caller that future operations should be skipped.</returns>
        bool ExecuteConversationScript(Script script, Npc npc, ConversationObjectBase conversationObjectBase,
            ConversationStage conversationStage);

        /// <summary>
        /// Move all the auto-follow Npcs to the same room as the Player.
        /// </summary>
        /// <param name="script">Script object encapsulating the script to be executed.</param>
        void MoveAutoFollowNpcs(Script script);

        /// <summary>
        /// Checks if the specified sentence appears in the subsequent list of sentences.
        /// </summary>
        /// <param name="sourceSentence">String containing the sentence to check.</param>
        /// <param name="sentencesToCheck">List of sentences to check against.</param>
        /// <returns>True if the sentence exists, false otherwise.</returns>
        bool CheckSentenceAgainstList(string sourceSentence, IList<string> sentencesToCheck);

        /// <summary>
        /// Determines if the specified word is a stop word.
        /// </summary>
        /// <param name="wordToCheck">String containing the word to be check.</param>
        /// <returns>True if the word is a stop word and false otherwise.</returns>
        bool IsStopWord(string wordToCheck);

        /// <summary>
        /// Splits a sentence into a list (by the spaces in it)
        /// </summary>
        /// <param name="source">String to be split.</param>
        /// <returns>List of the constituent words.</returns>
        IList<string> SplitSentence(string source);

        /// <summary>
        /// Retrieves all the names of all rooms and placeable objects in the game. This includes short descriptions, long descriptions and inventory descriptions.
        /// </summary>
        /// <returns>A string array containing all the names in the game.</returns>
        IList<string> GetAllNamesInGame();
        
        /// <summary>
        /// Executes the a specifc method in the supplied script.
        /// </summary>
        /// <param name="script">Script object encapsulating the script to be executed.</param>
        /// <param name="methodName">Name of the method to execute.</param>
        void ExecuteMethod(Script script, string methodName);

        /// <summary>
        /// Executes the supplied script, passing in the object been acted on and the command being processed.
        /// </summary>
        /// <param name="script">Script object encapsulating the script to be executed.</param>
        /// <param name="adventureObject">Adventure object that is currently being pre-processed.</param>
        /// <param name="adventureCommand">The command that was executed that caused the object to be processed.</param>
        /// <returns>Return value of the script. Can be set to False to signal to caller that future operations should be skipped.</returns>
        bool ExecuteObjectScript(Script script, AdventureObjectBase adventureObject, ParsedAdventureCommand adventureCommand);

        /// <summary>
        /// Executes the input processor against the supplied text input by the user. The supplied script must contain a python function
        /// def ProcessInput(playerInputText): This takes a string and returns a ParsedAdventureCommand.
        /// </summary>
        /// <param name="script">Python script containing the ProcessInput function.</param>
        /// <param name="playerInput">String containing the text input by the user.</param>
        /// <returns>The text converted to ParsedAdventureCommand ParsedAdventureCommand.</returns>
        ParsedAdventureCommand ExecuteInputProcessor(Script script, string playerInput);

        /// <summary>
        /// Retrieves the current list of stop (also known as noise) words, used in the game.
        /// </summary>
        /// <returns>A sorted list of the game Stop Words.</returns>
        SortedList<string, string> GetStopWords();

        /// <summary>
        /// Give a source string returns it back with punctuation characters removed.
        /// </summary>
        /// <param name="source">String to be processed.</param>
        /// <param name="preserveQuotes">Boolean flag, if set to true quotes will not be removed.</param>
        /// <returns>A string containing the source string minus punctuation characters.</returns>
        string RemovePunctuation(string source, bool preserveQuotes);

        /// <summary>
        /// Processes a sentence and uses data associated with AdventureCommandMappings to categorise them as GameWords that then allows each one to
        /// be converted to an actual AdventurCommandMapping.
        /// </summary>
        /// <param name="wordList">A string array containing the list of words to process.</param>
        /// <returns>A List of GameWord objects.</returns>
        IList<GameWord> ConvertSentence(IList<string> wordList);

        /// <summary>
        /// Determines if the target string is equal to the source, optionally allowing a partial match.
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="target">The target string</param>
        /// <param name="pc">A decimal number representing the percentage of the string to match before returning true.</param>
        /// <returns>True if the strings match, false otherwise.</returns>
        bool MatchesPercent(string source, string target, double pc = 100);
    }
}