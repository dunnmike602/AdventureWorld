using AdventureLandCore.Domain;
using AdventureLandCore.Services.Data;

namespace DiagramDesigner.AdventureWorld.GameExecution
{
    public class GameInformation
    {
        public AdventureGameSetup ModelData { get; set; }

        public Script TestScript { get; set; }
    }
}