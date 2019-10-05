
using System.Reflection;

namespace DiagramDesigner.AdventureWorld.Domain.Interfaces
{
    public interface IValidationFilter
    {
        bool ShouldValidate(PropertyInfo property);
    }
}
