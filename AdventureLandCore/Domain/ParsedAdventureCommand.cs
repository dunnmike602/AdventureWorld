using System;
using System.Collections.Generic;
using System.Linq;
using AdventureLandCore.Services.Data;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Represents an adventure command that has been parsed from player input text.
    /// </summary>
    public class ParsedAdventureCommand
    {
        /// <summary>
        /// Indicates if the command is valid
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// If command is not valid indicates the error message.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// The command that was parsed as a GameWord.
        /// </summary>
        public GameWord ParsedCommand { get; set; }

        /// <summary>
        /// Parameters passed to the command.
        /// </summary>
        public IList<GameWord> Parameters { get; set; } =  new List<GameWord>();
        
        /// <summary>
        /// The adventure game command that has been identified as being related to the text input by the player.
        /// </summary>
        public CommandMapping CommandMapping { get; set; }

        /// <summary>
        /// Retrieves the command itself and its parameters as a single list.
        /// </summary>
        /// <returns>A list containing the command itself and its parameters.</returns>
        public IList<GameWord> GetCommandAndParameters()
        {
            return new List<GameWord>{ ParsedCommand }.Concat(Parameters).ToList();
        }

        /// <summary>
        /// Retrieves the list of the command parameters filtering out stop words.
        /// </summary>
        /// <param name="additionalWords">Optional list of words to filter by. Can be null or a list of strings. Any value in this list will also be
        /// excluded from the returned list.</param>
        /// <returns>A list of parameters without stop words.</returns>
        public IList<GameWord> GetParametersWithoutStopWords(List<string> additionalWords = null)
        {
            return Parameters.Where(parm => parm.Type != WordType.StopWord && (additionalWords == null || 
                additionalWords.All(addWord => !string.Equals(addWord, parm.Word, StringComparison.CurrentCultureIgnoreCase)))).ToList();
        }


        /// <summary>
        /// Retrieves the list of the original words sent as command parameters filtering out stop words.
        /// </summary>
        /// <returns>A list of original words without stop words.</returns>
        public IList<string> GetOriginalWordsWithoutStopWords()
        {
            return Parameters.Where(parm => parm.Type != WordType.StopWord).Select(parm => parm.OriginalWord).ToList();
        }

        /// <summary>
        /// Retrieves the list of the original words sent as command parameters.
        /// </summary>
        /// <returns>A list of the originalWords.</returns>
        public IList<string> GetOriginalWords()
        {
            return Parameters.Select(parm => parm.OriginalWord).ToList();
        }

        /// <summary>
        /// Retrieves the list of the words sent as command parameters.
        /// </summary>
        /// <returns>A list of the originalWords.</returns>
        public IList<string> GetWords()
        {
            return Parameters.Select(parm => parm.Word).ToList();
        }

        /// <summary>
        /// Retrieves the command itself and its parameters as a single space delimited string.
        /// </summary>
        /// <returns>String containing the command and parameters.</returns>
        public string JoinCommandAndParameters()
        {
            return string.Join(" ", GetCommandAndParameters().Select(cmd => cmd.Word)).Trim();
        }

        /// <summary>
        /// Retrieves the command itself and its parameters as a single space delimited string..
        /// </summary>
        /// <returns>String containing the original word and parameters.</returns>
        public string JoinOriginalWordAndParameters()
        {
            return string.Join(" ", GetCommandAndParameters().Select(cmd => cmd.OriginalWord)).Trim();
        }

        /// <summary>
        /// Retrieves the command  parameters as a single space delimited string..
        /// </summary>
        /// <returns>String containing the command parameters.</returns>
        public string JoinParameters()
        {
            return string.Join(" ", Parameters).Trim();
        }
    }
}