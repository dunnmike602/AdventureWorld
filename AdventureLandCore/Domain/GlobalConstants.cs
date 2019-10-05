namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Global constants used throughout the game engine.
    /// </summary>
    public class GlobalConstants
    {
        /// <summary>
        /// Default text for the console prompt.
        /// </summary>
        public const string DefaultPrompt = "Ready>";

        /// <summary>
        /// Name if the directory where games are saved during play.
        /// </summary>
        public const string SaveGameDirectory = "SaveGames";

        /// <summary>
        /// Save game file extension.
        /// </summary>
        public const string SaveGameExtension = ".ads";

        /// <summary>
        /// Extension of game files, this are files that define a particulare adventure game.
        /// </summary>
        public const string GameExtension = ".adl";

        /// <summary>
        /// Names of the Adventure World scripting Api.
        /// </summary>
        public const string ScriptApiName = "AWApi";
        
        /// <summary>
        /// Names of the Language scripting Api.
        /// </summary>
        public const string LanguageApiName = "LanguageApi";

        /// <summary>
        /// Name of the Console api.
        /// </summary>
        public const string ConsoleApiName = "ConsoleApi";

        /// <summary>
        /// Initialisation script name.
        /// </summary>
        public const string InitScriptName = "Initialisation Script";

        /// <summary>
        /// Game loop pre-process script name.
        /// </summary>
        public const string GameLoopPreScriptName = "Game loop Pre-Process Script";

        /// <summary>
        /// Game loop post-process script name.
        /// </summary>
        public const string GameLoopPostScriptName = "Game loop Post-Process Script";

        /// <summary>
        /// Common code name.
        /// </summary>
        public const string CommonCodeScriptName = "Common Code";
    }
}