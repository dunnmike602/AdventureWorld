using System.Runtime.Serialization;

namespace AdventureLandCore.WCFCommon
{
    [DataContract(IsReference = true)]
    public class AdventureWordDeleteRequest
    {
        [DataMember]
        public string Guid { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}