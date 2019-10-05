using AdventureLandCore.Services.Data;

namespace AdventureLandCore.Interfaces
{
    public interface IGameConfigurationPersistance
    {
        void SaveGameData(string data, string fullFilePath);

        AdventureGameSetup DeserializeFromString(string xmlData);

        AdventureGameSetup LoadGameModel(string fullFilePath);

        AdventureGameSetup LoadGameModelFromString(string data);

        string SerializeToString(AdventureGameSetup modelData);
    }
}