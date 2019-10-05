using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Extensions;

namespace DiagramDesigner.Symbols.Helpers
{
    internal static class AdventureObjectHelper
    {
        public static bool GetIsRoomDesigner(string name)
        {
            return name == "RoomDesigner";
        }

        public static ObjectType ConvertName(string name)
        {
            return (ObjectType)Enum.Parse(typeof(ObjectType), name);
        }

        public static (string ObjectTypeName, string LabelName, IEnumerable<AdventureObjectBase> adventureObjects) GetSetupForStencil(ObjectType stencilObjectType)
        {
            var objectTypes = new Dictionary<ObjectType, string>
            {
                {ObjectType.Room, "ROOM"},
                {ObjectType.PlaceableObject, "OBJECT"},
                {ObjectType.Container, "CONTAINER"},
                {ObjectType.Npc, "NPC"}
            };

            var labelNames = new Dictionary<ObjectType, string>
            {
                {ObjectType.Room, "RoomText"},
                {ObjectType.PlaceableObject, "ObjectText"},
                {ObjectType.Container, "ContainerText"},
                {ObjectType.Npc, "NpcText"}
            };
                
            var adventureObjects = new Dictionary<ObjectType, IEnumerable<AdventureObjectBase>>
            {
                {ObjectType.Room,  AdventureGameDesignerViewModel.Instance.RoomsList},
                {ObjectType.PlaceableObject, AdventureGameDesignerViewModel.Instance.PlaceableObjectsList},
                {ObjectType.Container, AdventureGameDesignerViewModel.Instance.PlaceableObjectsList},
                {ObjectType.Npc, AdventureGameDesignerViewModel.Instance.PlaceableObjectsList}
            };

            return adventureObjects.ContainsKey(stencilObjectType) ?  (objectTypes[stencilObjectType], labelNames[stencilObjectType], adventureObjects[stencilObjectType]) : (
                null, null, null);
        }

        public static void ProcessExistingAdventureObject(DesignerItem newItem, AdventureObjectBase existingObject, ObjectType stencilObjectType)
        {
            var grid = (Grid)newItem.Content;
            var stencilSetup = GetSetupForStencil(stencilObjectType);
            
            existingObject.ControlId = newItem.ID;

            switch (stencilObjectType)
            {
                case ObjectType.Room:
                    SetupRoom(existingObject as Room, grid, stencilSetup);
                    break;

                case ObjectType.Container:
                case ObjectType.Npc:
                case ObjectType.PlaceableObject:
                    SetupPlaceableObject(grid, existingObject as PlaceableObject, stencilSetup);
                    break;
            }
        }

        public static void RebindConversationObject(DesignerItem item)
        {
            var grid = (Grid)item.Content;

            switch (item.StencilObjectType)
            {
                case ObjectType.ConversationStart:
                    var conversations = ConversationDesigner.ConversationDesigner.Instance.ConversationTree.Conversations;

                    StencilHelpers.BindStencilToObject(grid, conversations.First(con => con.ControlId == item.ID), "StartText", "BaseName");
                    break;

                case ObjectType.ConversationText:
                    var conversationTexts = ConversationDesigner.ConversationDesigner.Instance.ConversationTexts;

                    StencilHelpers.BindStencilToObject(grid, conversationTexts.First(con => con.ControlId == item.ID), "Text", "Text");
                    break;

                case ObjectType.ConversationResponse:
                    var conversationResponses = ConversationDesigner.ConversationDesigner.Instance.ConversationResponses;

                    var conversationResponse = conversationResponses.First(con => con.ControlId == item.ID);

                    StencilHelpers.BindStencilToObject(grid, conversationResponse, "Response", "Response");
                    StencilHelpers.BindStencilToObject(grid, conversationResponse, "SortOrder", "SortOrder");
                    break;
            }
        }

        public static AdventureObjectBase ProcessConversationObject(DesignerItem newItem)
        {
            var grid = (Grid) newItem.Content;
           
            switch (newItem.StencilObjectType)
            {
                case ObjectType.ConversationStart:
                    var conversations = ConversationDesigner.ConversationDesigner.Instance.ConversationTree.Conversations;

                    var objectName = AdventureGameDesignerViewModel.Instance.GetNextGenericName("CONVERSATION",
                        conversations.Select(obj => obj.BaseName));

                    var conversation = new Conversation { BaseName = objectName, ControlId = newItem.ID };
                    conversations.Add(conversation);

                    StencilHelpers.BindStencilToObject(grid, conversation, "StartText", "BaseName");

                    return conversation;

                case ObjectType.ConversationText:
                    var conversationTexts = ConversationDesigner.ConversationDesigner.Instance.ConversationTexts;

                    var conversationText = new ConversationText {ControlId = newItem.ID };
                    conversationTexts.Add(conversationText);

                    StencilHelpers.BindStencilToObject(grid, conversationText, "Text", "Text");

                    return conversationText;

                case ObjectType.ConversationResponse:
                    var conversationResponses = ConversationDesigner.ConversationDesigner.Instance.ConversationResponses;

                    var conversationResponse = new ConversationResponse { ControlId = newItem.ID };
                    conversationResponses.Add(conversationResponse);

                    StencilHelpers.BindStencilToObject(grid, conversationResponse, "Response", "Response");
                    StencilHelpers.BindStencilToObject(grid, conversationResponse, "SortOrder", "SortOrder");

                    return conversationResponse;
            }

            return null;
        }

        public static AdventureObjectBase ProcessNewAdventureObject(DesignerItem newItem)
        {
            var grid = (Grid)newItem.Content;
            var stencilSetup = GetSetupForStencil(newItem.StencilObjectType);
            
            var objectName = AdventureGameDesignerViewModel.Instance.GetNextGenericName(stencilSetup.ObjectTypeName, stencilSetup.adventureObjects.Select(obj => obj.BaseName));
            
            switch (newItem.StencilObjectType)
            {
                case ObjectType.Room:
                    var room = new Room { BaseName = objectName, ControlId = newItem.ID };
                    AdventureGameDesignerViewModel.Instance.Rooms.Add(room);

                    SetupRoom(room, grid, stencilSetup);

                    return room;

                case ObjectType.Container:
                case ObjectType.Npc:
                case ObjectType.PlaceableObject:
                    PlaceableObject placeableObject = null;

                    switch (newItem.StencilObjectType)
                    {
                        case ObjectType.Npc:
                            placeableObject = new Npc
                            {
                                BaseName = objectName,
                                ControlId = newItem.ID,
                            };
                            break;
                        case ObjectType.Container:
                            placeableObject = new Container
                            {
                                BaseName = objectName,
                                ControlId = newItem.ID,
                            };
                            break;
                        case ObjectType.PlaceableObject:
                            placeableObject = new PlaceableObject
                            {
                                BaseName = objectName,
                                ControlId = newItem.ID,
                            };
                            break;
                    }

                    if (placeableObject != null)
                    {
                        AdventureGameDesignerViewModel.Instance.PlaceableObjects.Add(placeableObject);

                        SetupPlaceableObject(grid, placeableObject, stencilSetup);

                        return placeableObject;
                    }

                    break;

            }

            return null;
        }

        private static void SetupPlaceableObject(Grid grid, PlaceableObject placeableObject,
            (string ObjectTypeName, string LabelName, IEnumerable<AdventureObjectBase> adventureObjects) stencilSetup)
        {
            StencilHelpers.BindStencilToObject(grid, placeableObject, stencilSetup.LabelName, "BaseName");

            var imageName = "ObjectImage";

            if (placeableObject.GetType() == typeof(Container))
            {
                imageName = "ContainerImage";
            }
            else if (placeableObject.GetType() == typeof(Npc))
            {
                imageName = "NpcImage";
            }

            StencilHelpers.BindStencilImageToAdventureObject(grid, placeableObject, imageName);

            AdventureDesigner.Instance.ValidatePlaceableItem(placeableObject);
        }

        private static void SetupRoom(Room room, Grid grid,
            (string ObjectTypeName, string LabelName, IEnumerable<AdventureObjectBase> adventureObjects) stencilSetup)
        {
            StencilHelpers.BindStencilToObject(grid, room, stencilSetup.LabelName, "BaseName");
            StencilHelpers.BindStencilImageToAdventureObject(grid, room, "RoomImage");
            AdventureDesigner.Instance.ValidateRoomItem(room);
        }

        public static bool CanConnect(DesignerItem source, DesignerItem sink)
        {
            // Can't connect a conversation start to itself
            if (source.StencilObjectType == ObjectType.ConversationStart &&
                sink.StencilObjectType == ObjectType.ConversationStart)
            {
                return false;
            }

            // Can't connect a conversation start directly to a response
            if (source.StencilObjectType == ObjectType.ConversationStart &&
                sink.StencilObjectType == ObjectType.ConversationResponse)
            {
                return false;
            }

            // Can't connect a text to conversation start
            if (source.StencilObjectType == ObjectType.ConversationText &&
                sink.StencilObjectType == ObjectType.ConversationStart)
            {
                return false;
            }

            // Can't connect a response to another response
            if (source.StencilObjectType == ObjectType.ConversationResponse &&
                sink.StencilObjectType == ObjectType.ConversationResponse)
            {
                return false;
            }

            // Can't connect text to text
            if (source.StencilObjectType == ObjectType.ConversationText &&
                sink.StencilObjectType == ObjectType.ConversationText)
            {
                return false;
            }

            // Response can't go back to the start
            if (source.StencilObjectType == ObjectType.ConversationResponse &&
                sink.StencilObjectType == ObjectType.ConversationStart)
            {
                return false;
            }

            // A conversation start can link to only one text
            if (source.StencilObjectType == ObjectType.ConversationStart && source.CountConnectorsFromThis() > 0)
            {
                return false;
            }

            // A response can only link to another conversation text
            return source.StencilObjectType != ObjectType.ConversationResponse || source.CountConnectorsFromThis() <= 0;
        }
    }
}
