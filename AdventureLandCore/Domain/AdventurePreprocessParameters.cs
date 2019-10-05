using AdventureLandCore.Interfaces;
using AdventureLandCore.Services.CoreApi.Interfaces;

namespace AdventureLandCore.Domain
{
    public class AdventurePreprocessParameters
    {
        public IAdventureGameEngine GameContext { get; set; }
        public IConsole GameConsole { get; set; }
        public IGameConfiguration Configuration { get; set; }
        public IAdventureApi AdventureApi { get; set; }

        public ParsedAdventureCommand CurrentCommand { get; set; }
        public bool CanContinue { get; set; }
        public bool HasRun { get; set; }
    }
}