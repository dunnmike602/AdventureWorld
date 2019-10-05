using System;
using System.Collections.Generic;
using System.Windows;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Helpers;
using DiagramDesigner.Symbols;
using DiagramDesigner.Symbols.Helpers;

namespace DiagramDesigner.ToolboxImpl
{
    public class DragObject
    {
        private ObjectType _stencilObjectType;

        public string Xaml { get; set; }

        public Size? DesiredSize { get; set; }

        public DesignerItem ParentDesignerItem { get; set; }

        public ObjectType StencilObjectType
        {
            get => _stencilObjectType;
            set
            {
                _stencilObjectType = value;

                DropAction = DropActions[_stencilObjectType];

                DesiredSize = _stencilObjectType == ObjectType.Container || _stencilObjectType == ObjectType.Npc ||
                              _stencilObjectType == ObjectType.PlaceableObject
                              || _stencilObjectType == ObjectType.Room
                    ? new Size(GlobalUiConstants.TemplateItemWidth, GlobalUiConstants.TemplateItemHeight)
                    : new Size(GlobalUiConstants.ConversationTemplateItemWidth,
                        GlobalUiConstants.ConversationTemplateItemHeight);
            }
        }

        public Dictionary<ObjectType, Func<DesignerItem, AdventureObjectBase>> DropActions { get; set; }

        public Func<DesignerItem, AdventureObjectBase> DropAction { get; set; }

        public DragObject()
        {
            DropActions = new Dictionary<ObjectType, Func<DesignerItem, AdventureObjectBase>>();

            Func<DesignerItem, AdventureObjectBase> processNewAdventureObject =
                AdventureObjectHelper.ProcessNewAdventureObject;

            Func<DesignerItem, AdventureObjectBase> processConversationObject =
                AdventureObjectHelper.ProcessConversationObject;

            DropActions.Add(ObjectType.Container, processNewAdventureObject);
            DropActions.Add(ObjectType.Npc, processNewAdventureObject);
            DropActions.Add(ObjectType.PlaceableObject, processNewAdventureObject);
            DropActions.Add(ObjectType.Room, processNewAdventureObject);
            DropActions.Add(ObjectType.ConversationStart, processConversationObject);
            DropActions.Add(ObjectType.ConversationText, processConversationObject);
            DropActions.Add(ObjectType.ConversationResponse, processConversationObject);
        }
        
        public bool CanDropInDesigner(string designerName)
        {
            return (StencilObjectType != ObjectType.PlaceableObject && StencilObjectType != ObjectType.Container &&
                    StencilObjectType != ObjectType.Npc) ||
                   !AdventureObjectHelper.GetIsRoomDesigner(designerName);
        }

        public ObjectClass GetObjectClass()
        {
            return StencilObjectType == ObjectType.PlaceableObject || StencilObjectType == ObjectType.Container
                                                                   || StencilObjectType == ObjectType.Npc ||
                                                                   StencilObjectType == ObjectType.Room
                ? ObjectClass.Game
                : ObjectClass.Conversation;
        }
    }
}