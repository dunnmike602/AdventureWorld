using System.Linq;

namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class ExitMapper
    {
        public static AdventureLandCore.Domain.Exit MapOne(Exit source)
        {
            return new AdventureLandCore.Domain.Exit
            {
                Id = source.ControlId,
                RoomName = source.ToRoom.BaseName,
                Direction = source.Direction,
                Descriptions = source.Descriptions?.ToArray() ?? new string[0],
                PreProcessScript = source.ObjectPreprocessScript,
                Visible = source.Visible,
                IsLocked = source.IsLocked,
                CanWalk = source.CanWalk,
                CanRun = source.CanRun,
                CanCrawl = source.CanCrawl,
                CanSwim = source.CanSwim,
                Category = source.Category,
                ShowRandomDescription = source.ShowRandomDescription,
            };
        }
    }
}