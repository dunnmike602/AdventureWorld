using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventureLandCore.Extensions
{
    /// <summary>
    /// Extends the string class with useful utilities.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines if the specified string is a valid GUID
        /// </summary>
        /// <param name="source">Source string to test.</param>
        /// <param name="value">Parsed GUID value if successful.</param>
        /// <returns></returns>
        public static bool TryStrToGuid(this string source, out Guid value)
        {
            try
            {
                value = new Guid(source);
                return true;
            }
            catch (FormatException)
            {
                value = Guid.Empty;
                return false;
            }
        }

        /// <summary>
        /// Extended replace method
        /// </summary>
        /// <param name="source">String in which the replacements are made.</param>
        /// <param name="oldValue">Value to replace</param>
        /// <param name="newValue">New value</param>
        /// <param name="comparisonType">StringComparison enumeration that defines the comparison type.</param>
        /// <returns>The string with the replacements done.</returns>
        public static string ReplaceEx(this string source, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (source == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(oldValue))
            {
                return source;
            }

            var result = new StringBuilder(Math.Min(4096, source.Length));
            var pos = 0;

            while (true)
            {
                var i = source.IndexOf(oldValue, pos, comparisonType);
                if (i < 0)
                {
                    break;
                }

                result.Append(source, pos, i - pos);
                result.Append(newValue);

                pos = i + oldValue.Length;
            }
            result.Append(source, pos, source.Length - pos);

            return result.ToString();
        }

        /// <summary>
        /// Ensure all lines breaks are replaces with single \n character.
        /// </summary>
        /// <param name="source">String in which the replacements are made.</param>
        /// <returns>String with normalised line breaks.</returns>
        public static string NormaliseLineBreaks(this string source)
        {
            return source?.Replace(Environment.NewLine, "\n");
        }

        /// <summary>
        /// Splits a sentence into words based on spaces.
        /// </summary>
        /// <param name="source">String to be split.</param>
        /// <returns>A list of words.</returns>
        public static List<string> SplitSentence(this string source)
        {
            return source?.Split(' ').ToList();
        }

        public static List<string> TextToList(this string source)
        {
           return source?.NormaliseLineBreaks()?.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
        }

        public static bool MatchesPercent(this string source, string target, double pc = 100)
        {
            var charsToMatch = (int)(source.Length * pc / 100);

            charsToMatch = Math.Min(Math.Max(charsToMatch, target.Length), source.Length);

            return source.Substring(0, charsToMatch).IsEqualTo(target);
        }

        public static string StripPunctuation(this string source, bool preserveQuotes)
        {
            var sb = new StringBuilder();
            foreach (var chr in source)
            {
                if (!char.IsPunctuation(chr) || (preserveQuotes && chr == '"'))
                {
                    sb.Append(chr);
                }
            }
            return sb.ToString();
        }

        public static string[] SplitString(this string source)
        {
            return source?.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string AddLineBreaks(this string source, int count = 1)
        {
            return (source ?? string.Empty) + string.Concat(Enumerable.Repeat(Environment.NewLine, count));
        }

        public static string PrefixLineBreaks(this string source, int count = 1)
        {
            return string.Concat(Enumerable.Repeat(Environment.NewLine, count)) + (source ?? string.Empty);
        }

        public static string ToAdventureCommand(this string source, string argument)
        {
            var returnValue = source;

            if (!string.IsNullOrWhiteSpace(argument))
            {
                returnValue += (" " + argument);
            }

            return returnValue;
        }

        public static bool IsEqualTo(this string source, string argument)
        {
            return string.Compare(source, argument, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool IsEqualToAny(this string source, IList<string> arguments)
        {
            return source.IsEqualToAny(arguments.ToArray());
        }

        public static bool IsEqualToAny(this string source, params string[] arguments)
        {
            return arguments.Any(argument => string.Compare(source, argument, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public static string ToSentenceCase(this string message)
        {
            int capturePosition;
            Capture capture;
            var messageArray = message.ToCharArray();

            const string pattern = @"[^\s]\.[\s\\n\\r]+[a-z]";
            var ms = Regex.Matches(message, pattern);

            for (var i = 0; i < ms.Count; i++)
            {
                capture = ms[i].Captures[0];
                capturePosition = capture.Index + capture.Value.Length - 1;
                messageArray[capturePosition] = char.ToUpper(messageArray[capturePosition]);
            }

            ms = Regex.Matches(message, @"^[\s\\r\\n]*[a-z]");

            if (ms.Count > 0)
            {
                capture = ms[0].Captures[0];
                capturePosition = capture.Index + capture.Value.Length - 1;
                messageArray[capturePosition] = char.ToUpper(messageArray[capturePosition]);
            }
            return new string(messageArray);
        }
    }
}