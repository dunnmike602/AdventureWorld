using System.Runtime.Serialization;

namespace AdventureLandCore.WCFCommon
{
    [DataContract(IsReference = true)]
    public class Error
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string OriginalException { get; set; }
    }
}