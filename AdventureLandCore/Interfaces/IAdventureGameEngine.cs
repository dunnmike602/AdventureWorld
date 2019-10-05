using AdventureLandCore.Domain;
#pragma warning disable 1591

namespace AdventureLandCore.Interfaces
{
    public interface IAdventureGameEngine 
    {
        event StateChangedEventHandler StateChanged;
        Player Player { get; set; }
        int CurrentScore { get; set; }
        Room Location { get; set; }
        GameState GetGameState();
        void RunTheGameLoop();
        void InitialiseTheGame();
        void InitialiseCore();
        GameData GameData { get; }
    }
}