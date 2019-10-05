using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using AdventureLandCore.Extensions;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Symbols.Helpers;
using DiagramDesigner.ToolboxImpl;

namespace DiagramDesigner.Symbols
{
    public partial class DesignerCanvas : Canvas
    {
        public bool CanHaveConnectors { get; set; }

        private Size GetSnapGridSize()
        {  
            var pointBrush = Background as System.Windows.Media.VisualBrush;

            return new Size(pointBrush.Viewbox.Width, pointBrush.Viewbox.Height);
        }

        private void SnapToGrid(UIElement masterItem, List<UIElement> itemsToMove)
        {
            if (!Options.Instance.SnapToGrid)
            {
                return;
            }

            var gridSize = GetSnapGridSize();

            var gridSizeX = Convert.ToInt32(gridSize.Width);
            var gridSizeY = Convert.ToInt32(gridSize.Height);

            var xSnap = GetLeft(masterItem) % gridSizeX;
            var ySnap = GetTop(masterItem) % gridSizeY;

            // If it's less than half the grid size, snap left/up
            // (by subtracting the remainder),
            // otherwise move it the remaining distance of the grid size right/down
            // (by adding the remaining distance to the next grid point).
            if (xSnap <= gridSizeX / 2.0)
                xSnap *= -1;
            else
                xSnap = gridSizeX - xSnap;
            if (ySnap <= gridSizeY / 2.0)
                ySnap *= -1;
            else
                ySnap = gridSizeY - ySnap;

            if (itemsToMove.Contains(masterItem) == false)
                itemsToMove.Add(masterItem);

            if (double.IsNaN(xSnap) || double.IsNaN(ySnap))
            {
                return;
            }

            foreach (var itemToMove in itemsToMove)
            {
                var xSnapPosition = xSnap + GetLeft(itemToMove);
                var ySnapPosition = ySnap + GetTop(itemToMove);

                SetLeft(itemToMove, xSnapPosition);
                SetTop(itemToMove, ySnapPosition);
            }
        }

        private Point? _rubberbandSelectionStartPoint;

        private SelectionService _selectionService;
        internal SelectionService SelectionService => _selectionService ?? (_selectionService = new SelectionService(this));

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (Equals(e.Source, this))
            {
                // in case that this click is the start of a 
                // drag operation we cache the start point
                _rubberbandSelectionStartPoint = e.GetPosition(this);

                // if you click directly on the canvas all 
                // selected items are 'de-selected'
                SelectionService.ClearSelection();
                Focus();
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // if mouse button is not pressed we have no drag operation, ...
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _rubberbandSelectionStartPoint = null;
            }

            // ... but if mouse button is pressed and start
            // point value is set we do have one
            if (_rubberbandSelectionStartPoint.HasValue)
            {
                // create rubberband adorner
                var adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    var adorner = new RubberbandAdorner(this, _rubberbandSelectionStartPoint);
                    adornerLayer.Add(adorner);
                }
            }
            e.Handled = true;
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            if (e.Source is UIElement element)
            {
                var itemsToMove = new List<UIElement>();

                foreach (var selectable in SelectionService.CurrentSelection.Where(item => item is UIElement))
                {
                    itemsToMove.Add(selectable as UIElement);
                }

                SnapToGrid(element, itemsToMove);
            }
        }
        
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (!(e.Data.GetData(typeof(DragObject)) is DragObject dragObject) ||
                string.IsNullOrEmpty(dragObject.Xaml) || !dragObject.CanDropInDesigner(Name))
            {
                return;
            }

            var content = XamlReader.Load(XmlReader.Create(new StringReader(dragObject.Xaml)));

            if (content != null)
            {
                var newItem = new DesignerItem {Content = content, StencilObjectType = dragObject.StencilObjectType};

                var position = e.GetPosition(this);

                var desiredSize = dragObject.DesiredSize.Value;
                newItem.Width = desiredSize.Width;
                newItem.Height = desiredSize.Height;

                dragObject.DropAction?.Invoke(newItem);

                SetZIndex(newItem, Children.Count);
                Children.Add(newItem);                    
               
                //update selection
                SelectionService.SelectItem(newItem);
                newItem.Focus();

                newItem.SetPropertyGridToCurrentSelectedItem(this);

                SnapToGrid(newItem, new List<UIElement>());

                if (dragObject.ParentDesignerItem == null)
                {
                    SetLeft(newItem, Math.Max(0, position.X - newItem.Width / 2));
                    SetTop(newItem, Math.Max(0, position.Y - newItem.Height / 2));
                }
                else
                {
                    // Handle joing to parent if required
                    SetLeft(newItem, GetLeft(dragObject.ParentDesignerItem) + (dragObject.ParentDesignerItem.Width * 2.5));
                    SetTop(newItem, GetTop(dragObject.ParentDesignerItem));

                    var sourceConnector = dragObject.ParentDesignerItem.GetConnector("Right");
                    newItem.UpdateLayout();
                    var sinkConnector = newItem.GetConnector("Left");

                    var newConnection = new Connection(sourceConnector, sinkConnector);
                    Children.Add(newConnection);


                    if (GetCanvasType() == CanvasType.AdventureDesigner)
                    {
                        ConnectionLinkerHelper.ProcessExit(sourceConnector, sinkConnector, newConnection);
                    }
                    else if (GetCanvasType() == CanvasType.Conversation)
                    {
                        ConnectionLinkerHelper.ProcessConversationLink(sourceConnector, sinkConnector);
                    }
                }
            }

            e.Handled = true;
        }
        
        protected override Size MeasureOverride(Size constraint)
        {
            var size = new Size();

            foreach (UIElement element in InternalChildren)
            {
                var left = GetLeft(element);
                var top = GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                var desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        internal DesignerItem GetDesignerItemByUid(Guid controlId)
        {
            return (from object child in InternalChildren
                select child as DesignerItem
                into designerItem
                where designerItem != null && designerItem.ID == controlId
                select designerItem).
                FirstOrDefault();
        }

        internal Connection GetConnectorByUid(Guid controlId)
        {
            return (from object child in InternalChildren
                select child as Connection
                into connection
                where connection != null && connection.ID == controlId
                select connection).FirstOrDefault();
        }

        public CanvasType GetCanvasType()
        {
            if (Name.IsEqualToAny("RoomDesigner", "ObjectDesigner"))
            {
                return CanvasType.AdventureDesigner;
            }
            else if (Name.IsEqualToAny("ConDesigner"))
            {
                return CanvasType.Conversation;
            }

            return CanvasType.Unknown;
        }

        public static void SetConnectorDecoratorTemplate(DesignerItem item)
        {
            if (item.ApplyTemplate() && item.Content is UIElement)
            {
                var template = DesignerItem.GetConnectorDecoratorTemplate((UIElement) item.Content);

                if (item.Template.FindName("PART_ConnectorDecorator", item) is Control decorator && template != null)
                {
                    decorator.Template = template;
                }
            }
        }
    }
}
