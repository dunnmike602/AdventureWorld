using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramDesigner.AdventureWorld.Domain.Mappers
{
    internal static class RoomMapper
    {
        private static List<AdventureLandCore.Domain.Exit> GetDomainExitsForRoom(Guid roomId, List<Exit> exitList)
        {
            return exitList.Where(exit => exit.FromRoom.ControlId == roomId).Select(ExitMapper.MapOne).ToList();
        }
        
        public static AdventureLandCore.Domain.Room MapOne(Room source, List<Exit> exitList)
        {
            return new AdventureLandCore.Domain.Room
            {
                Id = source.ControlId,
                Name = source.BaseName,
                Descriptions = source.Descriptions.ToArray(),
                PreProcessScript = source.ObjectPreprocessScript,
                Exits = GetDomainExitsForRoom(source.ControlId, exitList),
                IsDark = source.IsDark,
                Category = source.Category,
                ShowRandomDescription = source.ShowRandomDescription,
            };
        }
    }
}