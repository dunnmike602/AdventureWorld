using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using AdventureLandCore.Domain;
using AdventureLandCore.Interfaces;

namespace AdventureLandCore.Services.Data
{
    public abstract class GameConfigurationPersister : IGameConfigurationPersistance
    {
        public abstract void SaveGameData(string data, string fullFilePath);

        public abstract AdventureGameSetup LoadGameModel(string fullFilePath);

        public abstract AdventureGameSetup LoadGameModelFromString(string data);

        public AdventureGameSetup DeserializeFromString(string xmlData)
        {
            xmlData = StringCompressor.DecompressString(xmlData);

            using (Stream stream = new MemoryStream())
            {
                var data = Encoding.UTF8.GetBytes(xmlData);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                var deserializer = new DataContractSerializer(typeof(AdventureGameSetup), new[] { typeof(PlaceableObject), typeof(Container), typeof(Npc),
                    typeof(Room), typeof(AdventureObjectBase) });

                return deserializer.ReadObject(stream) as AdventureGameSetup;
            }
        }

        public string SerializeToString(AdventureGameSetup modelData)
        {
            var serializer = new DataContractSerializer(typeof(AdventureGameSetup),
                new[] {typeof(PlaceableObject), typeof(Room), typeof(Container), typeof(AdventureObjectBase), typeof(Npc) });

            using (var stringWriter = new StringWriter())
            using (var writer = new XmlTextWriter(stringWriter))
            {
                serializer.WriteObject(writer, modelData);
                var text = stringWriter.ToString();

                return StringCompressor.CompressString(text);
            }
        }
    }
}