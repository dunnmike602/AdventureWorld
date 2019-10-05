using System.Linq;

namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class ContainerMapper
    {
        public static AdventureLandCore.Domain.Container MapOne(Container source)
        {
            return new AdventureLandCore.Domain.Container
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
                IsOpen = source.IsOpen,
                IsLocked = source.IsLocked,
                Category = source.Category,
                ShowRandomDescription = source.ShowRandomDescription,
            };
        }
    }
}