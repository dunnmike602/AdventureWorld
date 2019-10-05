using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AdventureLandCore.Services.Data;
using DiagramDesigner.AdventureWorld.CustomCollections;

namespace DiagramDesigner.AdventureWorld.Domain.Interfaces
{
    public interface IAdventureGameDesignerApi
    {
        AdventureObjectBase FindObjectByGuid(Guid id);
        string GetNextGenericName(string nameStub, IEnumerable<AdventureObjectBase> collectionToSearch);
        ObservableItemCollection<Room> Rooms { get; set; }
        ObservableItemCollection<Exit> Exits { get; set; }
        ObservableItemCollection<PlaceableObject> PlaceableObjects { get; set; }
        List<Room> RoomsList { get; }
        List<Exit> ExitList { get;  }
        List<PlaceableObject> PlaceableObjectsList { get; }
        void DeleteRoomByGuid(Guid id);
        void DeleteObjectByGuid(Guid id);
        void DeleteExitByGuid(Guid id);
        void Clear();
        ObservableCollection<Exit> GetExitsForRoom(Guid roomId);
        AdventureGameSetup Map();
    }
}