using AdventureLandCore.Services.CoreApi.Interfaces;

namespace AdventureLandCore.Scripting
{
    public class Globals
    {
        public IAdventureApi AWApi { get; set; }

        public IConsole ConsoleApi { get; set; }

        public ILanguageApi LanguageApi { get; set; }
    }
}