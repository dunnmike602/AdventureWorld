using System.Linq;

namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class PlaceableObjectMapper
    {
        public static AdventureLandCore.Domain.PlaceableObject MapOne(PlaceableObject source)
        {
            return new AdventureLandCore.Domain.PlaceableObject
            {
                Id = source.ControlId,
                IsLightSource = source.IsLightSource,
                IsLit = source.IsLit,
                Fixed = source.Fixed,
                HideFromAutoList = source.HideFromAutoList,
                Descriptions = source.Descriptions.ToArray(),
                Name = source.BaseName,
                ParentId = source.Parent?.ControlId,
                IsHeld = source.IsHeld,
                PreProcessScript = source.ObjectPreprocessScript,
                Visible = source.Visible,
                InventoryDescription = source.InventoryDescription,
                Category = source.Category,
                ShowRandomDescription = source.ShowRandomDescription,
            };
        }
    }
}
