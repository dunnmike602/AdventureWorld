using System;
using System.IO;
using AdventureLandCore.Services.Helpers;

namespace AdventureLandCore.Services.Data
{
    public class ConsoleGameConfigurationPersister : GameConfigurationPersister
    {
        public override void SaveGameData(string data, string fullFilePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));

            FileHelper.SaveDataFile(fullFilePath, data);
        }

        public override AdventureGameSetup LoadGameModelFromString(string data)
        {
            return DeserializeFromString(data);
        }

        public override AdventureGameSetup LoadGameModel(string fullFilePath)
        {
            if (!File.Exists(fullFilePath))
            {
                throw new Exception($"Cannot Start Up with the specified game {fullFilePath}.");
            }

            var data = FileHelper.LoadDataFile(fullFilePath);
           

            var gameData = DeserializeFromString(data);
            gameData.FullFilePath = fullFilePath;

            return gameData;
        }
    }
}