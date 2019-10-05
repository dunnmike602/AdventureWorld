using AdventureLandCore.Interfaces;

namespace AdventureLandCore.Domain
{
    public class ExecutionParameters
    {
        public object[] ConstructorParameters { get; set; }
        public ParsedAdventureCommand TriggeringCommand { get; set; }
        public IGameConfiguration GameConfiguration { get; set;}
    }
}