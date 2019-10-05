using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Enumeration that defines the current game state.
    /// </summary>
    [DataContract]
    public enum GameState
    {
        /// <summary>
        /// Game is initialising variables and state.
        /// </summary>
        Initialising = 0,
        /// <summary>
        /// All scripts are compiled.
        /// </summary>
        ScriptsCompiled = 1,
        /// <summary>
        /// Initialisation is complete but the game loop has not started.
        /// </summary>
        InitialisationComplete = 2,
        /// <summary>
        /// Game is currently running the game loop.
        /// </summary>
        [DataMember]
        Running = 3,
        /// <summary>
        /// Game is still running but player has lost the game.
        /// </summary>
        [DataMember]
        PlayerLost = 1,
        /// <summary>
        /// Player has asked to quit the game.
        /// </summary>
        [DataMember]
        Quit,
        /// <summary>
        /// Player has won the game.
        /// </summary>
        [DataMember]
        Won,
    }
}