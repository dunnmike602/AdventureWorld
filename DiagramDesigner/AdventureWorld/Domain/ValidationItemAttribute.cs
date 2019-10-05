using System;
using System.Runtime.Serialization;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [DataContract]
    public class ValidationItemAttribute : Attribute
    {
        [DataMember]
        public ValidationTypes ValidationType { get; set; }

        [DataMember]
        public Severities Severity { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public Guid? ControlId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [IgnoreDataMember]
        public Func<Guid?, bool> Action { get; set; }

        [DataMember]
        public ValidationCategories ValidationCategory {get; set;}
 
        public string SeverityImage
        {
            get
            {
                switch (Severity)
                {
                    case Severities.Error:
                       return @"..\Resources\Images\error.png";
                    case Severities.Warning:
                      return @"..\Resources\Images\warning.png";
                    case Severities.Information:
                      return @"..\Resources\Images\info.png";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}