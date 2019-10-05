using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace AdventureLandCore.WCFCommon
{
    [DataContract(IsReference = true)]
    public class AdventureWordPublishingResponse
    {
        [DataMember]
        private readonly List<Error> _errors = new List<Error>();

        private bool _hasSucceeded = true;

        [DataMember]
        public bool HasSucceeded
        {
            get => _hasSucceeded;
            set { _hasSucceeded = value; }
        }
        
        public ReadOnlyCollection<Error> Errors => _errors.AsReadOnly();

        public void AddError(string message, string originalException = null)
        {
            _hasSucceeded = false;
            _errors.Add(new Error { Message = message, OriginalException = originalException });
        }
    }
}