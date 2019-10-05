using System;
using AdventureLandCore.Extensions;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Event arguments for the GameStateEventChanged event delegate. This is fired at
    /// </summary>
    public class GameStateEventArgs : EventArgs
    {
        /// <summary>
        /// Current state of the game when the event was raised.
        /// </summary>
        public GameState State { get; set; }

        /// <summary>
        /// The Player's current location in the game, when the event was raised, if appropriate.
        /// </summary>
        public Room  CurrentLocation { get; set; }

        /// <summary>
        /// Provides more information on the state change that just happened if appropriate
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Returns a description of the state change that just occurred
        /// </summary>
        /// <returns>String containing diagnostic information.</returns>
        public override string ToString()
        {
            return $"Game has just entered State: {State}.{string.Empty.AddLineBreaks()}{Description}";
        }
    }
}