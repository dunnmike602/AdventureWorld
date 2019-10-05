using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using AdventureLandCore.Domain;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract(IsReference = true)]
    public abstract class IdentifableObjectBase : ReactiveObjectBase
    {
        [DataMember]
        [DisplayName("Unique Id")]
        [Description("This is a unique identifier allocated to every object in the game. It is allocated automatically and cannot be changed.")]
        [ReadOnly(true)]
        public Guid ControlId { get; set; } = Guid.NewGuid();
        
        public override int GetHashCode()
        {
            return ControlId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return EntityEquals(obj as IdentifableObjectBase);
        }

        private bool EntityEquals(IdentifableObjectBase other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return ControlId == other.ControlId;
            }
        }
    }
}