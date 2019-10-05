using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AdventureLandCore.Domain;

namespace AdventureLandCore.Services.Data
{
    [DataContract(IsReference = true)]
    public class CommandMapping
    {
        [DataMember] public bool IsBuiltInCommand { get; set; } = true;

        [DataMember]
        public string VerbName { get; set; }

        [DataMember]
        public List<string> AliasList{ get; set; } = new List<string>();

        [DataMember]
        public List<string> OneWordSubstitutionList { get; set; } = new List<string>();

        [DataMember]
        public Script ScriptCommand { get; set; }

        [DataMember]
        public string HelpText { get; set; }

        public List<string> GetAllAliases()
        {
            return AliasList.Union(new List<string> {VerbName}).ToList();
        }
    }
}