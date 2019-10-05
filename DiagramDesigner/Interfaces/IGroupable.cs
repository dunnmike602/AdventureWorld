using System;

namespace DiagramDesigner.Interfaces
{
    public interface IGroupable
    {
        Guid ID { get; }
        Guid ParentId { get; set; }
        bool IsGroup { get; set; }
    }
}
