using System.Runtime.Serialization;

namespace AdventureLandCore.WCFCommon
{
    [DataContract(IsReference = true)]
    public class AdventureWordPublishingRequest
    {
        [DataMember]
        public string Guid { get; set; }
        
        [DataMember]
        public string GameName { get; set; }

        [DataMember]
        public string GameDescription { get; set; }

        [DataMember]
        public string GameModel { get; set; }

        [DataMember]
        public string GameDesign { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}