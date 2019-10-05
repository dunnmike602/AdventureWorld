using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Symbols.Helpers;
using Syncfusion.Windows.Tools.Controls;

namespace DiagramDesigner.Extensions
{
    public static class StencilHelpers
    {
        public static void BindParentToObject(Grid childGrid, Grid parentGrid,  PlaceableObject childObject, AdventureObjectBase parentAdventureObject)
        {
            // Determine name of the required label based on whether it is a container or not
            var parentName = "ContainerParentName";

            if (childObject.GetType() == typeof(PlaceableObject))
            {
                parentName = "ObjectParentName";
            }
            else if (childObject.GetType() == typeof(Npc))
            {
                parentName = "NpcParentName";
            }

            BindStencilToObject(childGrid, parentAdventureObject, parentName, "BaseName");

            BindParentAndChildBackgrounds(childGrid, parentGrid, childObject, parentAdventureObject);
        }

        private static void BindParentAndChildBackgrounds(Grid childGrid, Grid parentGrid, PlaceableObject childObject,
            AdventureObjectBase parentAdventureObject)
        {
            // Bind the childs background to the parent, parent could be room, container or object
            string parentBackgroundName;

            switch (parentAdventureObject.ObjectType)
            {
                case ObjectType.Room:
                    parentBackgroundName = "RoomRectangle";
                    break;
                case ObjectType.PlaceableObject:
                    parentBackgroundName = "ObjectRectangle";
                    break;
                case ObjectType.Npc:
                    parentBackgroundName = "NpcRectangle";
                    break;
                default:
                    parentBackgroundName = "ContainerRectangle";
                    break;
            }

            var parentBorderControl = (Border) parentGrid.FindName(parentBackgroundName);

            var childBackgroundName = "ContainerParentNameBackGround";

            if (childObject.GetType() == typeof(PlaceableObject))
            {
                childBackgroundName = "ObjectParentNameBackGround";
            }
            else if (childObject.GetType() == typeof(Npc))
            {
                childBackgroundName = "NpcParentNameBackGround";
            }

            var childBorderControl = (Border) childGrid.FindName(childBackgroundName);

            var backgroundBinding = new Binding("Background") {Source = parentBorderControl};
            childBorderControl.SetBinding(Border.BackgroundProperty, backgroundBinding);
        }

        public static void BindStencilToObject(Grid grid, AdventureObjectBase adventureObject, string labelName, string propertyName)
        {
            var textBlock = (TextBlock) grid.FindName(labelName);

            var textBinding = new Binding(propertyName) {Source = adventureObject};
            textBlock.SetBinding(TextBlock.TextProperty, textBinding);
            textBlock.SetBinding(FrameworkElement.ToolTipProperty, textBinding);
        }

        public static void BindStencilImageToAdventureObject(Grid grid, AdventureObjectBase adventureObject, string labelName)
        {
            var image = (Image)grid.FindName(labelName);

            var imageBinding = new Binding("Image") { Source = adventureObject};
            image.SetBinding(Image.SourceProperty, imageBinding);

            var visibilityBinding = new Binding("ShowImages") { Source = AdventureGameDesignerViewModel.Instance,
                Converter = new BoolToVisibilityConverter()};

            image.SetBinding(UIElement.VisibilityProperty, visibilityBinding);
        }

        public static void BindConnectorToObject(FrameworkElement connector, AdventureObjectBase adventureObject,
            string labelName)
        {
            var textBinding = new Binding(labelName) {Source = adventureObject};

            connector.SetBinding(FrameworkElement.ToolTipProperty, textBinding);
        }

        /// <summary>
        /// Remove all the parent bindings from the container object it is no longer linked.
        /// </summary>
        public static void RemoveParentBindingFromObject(Grid grid, PlaceableObject childObject)
        {
            var labelName = "ObjectParentName";

            if (childObject.GetType() == typeof(Container))
            {
                labelName = "ContainerParentName";
            }
            else if (childObject.GetType() == typeof(Npc))
            {
                labelName = "NpcParentName";
            }

            // Determine name of the required label based on whether it is a container or not
           var textBlock = (TextBlock)grid.FindName(labelName);

            BindingOperations.ClearBinding(textBlock, TextBox.TextProperty);
            BindingOperations.ClearBinding(textBlock, FrameworkElement.ToolTipProperty);

            var childBackgroundName = "ObjectParentNameBackGround";

            if (childObject.GetType() == typeof(Container))
            {
                childBackgroundName = "ContainerParentNameBackGround";
            }
            else if (childObject.GetType() == typeof(Npc))
            {
                childBackgroundName = "NpcParentNameBackGround";
            }

            var childBorderControl = (Border)grid.FindName(childBackgroundName);

            BindingOperations.ClearBinding(childBorderControl, Border.BackgroundProperty);

            textBlock.Text = null;
        }
    }
}
