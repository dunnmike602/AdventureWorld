using System.Collections.Generic;

namespace AdventureLandCore.Interfaces
{
    public interface IPersistanceOperations
    {
        void Save(string fileStem, string data);
        string Load(string fileStem);
        void Delete(string fileStem);
        List<string> List();
    }
}