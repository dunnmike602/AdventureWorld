using System.ComponentModel;
using System.Runtime.Serialization;

namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Defines a movement direction, for example North.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Direction
    {   
        /// <summary>
        /// Abbreviation for the direction.
        /// </summary>
        [DataMember]
        [Description("Abbreviation for the direction.")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Full text for the direction.
        /// </summary>
        [DataMember]
        [Description("Full text for the direction.")]
        public string Text { get; set; }
    }
}