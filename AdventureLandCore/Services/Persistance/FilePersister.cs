using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Interfaces;

namespace AdventureLandCore.Services.Persistance
{
    public class FilePersister : IPersistanceOperations
    {
        public void Save(string fileStem, string data)
        {
            var filePath = GetFileInformation(fileStem);

            using (var writer = File.CreateText(filePath))
            {
                writer.WriteLine(data);
            }
        }

        public string Load(string fileStem)
        {
            var filePath = GetFileInformation(fileStem);

            if (!File.Exists(filePath))
            {
                return null;
            }

            using (var reader = File.OpenText(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        private static string GetFileInformation(string fileStem)
        {
            var fileName = Path.GetInvalidFileNameChars().Aggregate(fileStem,
                (current, nextChar) => current.Replace(nextChar.ToString(), string.Empty));

            var dirName = Path.Combine(Environment.CurrentDirectory, GlobalConstants.SaveGameDirectory);

            Directory.CreateDirectory(dirName);

            var filePath = Path.Combine(dirName, fileName + GlobalConstants.SaveGameExtension);
            return filePath;
        }

        public void Delete(string fileStem)
        {
            var filePath = GetFileInformation(fileStem);

            File.Delete(filePath);
        }

        public List<string> List()
        {
            var dirName = Path.Combine(Environment.CurrentDirectory, GlobalConstants.SaveGameDirectory);

            Directory.CreateDirectory(dirName);

            var fileInfo = new List<string>();

            var directory = new DirectoryInfo(dirName);
            var files = directory.GetFiles().Where(file => file.Extension.IsEqualTo(GlobalConstants.SaveGameExtension))
                .OrderByDescending(p => p.LastWriteTimeUtc).ToList();

            foreach (var file in files)
            {
                var info = Path.GetFileNameWithoutExtension((file.Name)).PadRight(20) + " : " + file.LastWriteTimeUtc.ToLocalTime();
                fileInfo.Add(info);
            }

            return fileInfo;
        }
    }
}