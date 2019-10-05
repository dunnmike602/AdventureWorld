using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Extensions;
using DiagramDesigner.Helpers;
using DiagramDesigner.Interfaces;
using DiagramDesigner.Symbols.Helpers;

namespace DiagramDesigner.Symbols
{
    public partial class DesignerCanvas
    {
        public static RoutedCommand Group = new RoutedCommand();
        public static RoutedCommand Ungroup = new RoutedCommand();
        public static RoutedCommand BringForward = new RoutedCommand();
        public static RoutedCommand BringToFront = new RoutedCommand();
        public static RoutedCommand SendBackward = new RoutedCommand();
        public static RoutedCommand SendToBack = new RoutedCommand();
        public static RoutedCommand AlignTop = new RoutedCommand();
        public static RoutedCommand AlignVerticalCenters = new RoutedCommand();
        public static RoutedCommand AlignBottom = new RoutedCommand();
        public static RoutedCommand AlignLeft = new RoutedCommand();
        public static RoutedCommand AlignHorizontalCenters = new RoutedCommand();
        public static RoutedCommand AlignRight = new RoutedCommand();
        public static RoutedCommand DistributeHorizontal = new RoutedCommand();
        public static RoutedCommand DistributeVertical = new RoutedCommand();
        public static RoutedCommand SelectAll = new RoutedCommand();
        public static RoutedCommand SameWidth = new RoutedCommand();
        public static RoutedCommand SameHeight = new RoutedCommand();
        public static RoutedCommand SameSize = new RoutedCommand();
        public static RoutedCommand ShowInExplorer = new RoutedCommand();
        public static RoutedCommand ShowInConversationExplorer = new RoutedCommand();
        
        public DesignerCanvas()
        {
            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            CommandBindings.Add(new CommandBinding(ShowInExplorer, ShowInExplorerExecuted, ShowInExplorerEnabled));
            CommandBindings.Add(new CommandBinding(ShowInConversationExplorer, ShowInConversationExplorerExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, CutExecuted, CutEnabled));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyExecuted, CopyEnabled));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteExecuted, PasteEnabled));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, DeleteExecuted, DeleteEnabled));
            CommandBindings.Add(new CommandBinding(Group, GroupExecuted, GroupEnabled));
            CommandBindings.Add(new CommandBinding(Ungroup, UngroupExecuted, Ungroup_Enabled));
            CommandBindings.Add(new CommandBinding(BringForward, BringForward_Executed, Order_Enabled));
            CommandBindings.Add(new CommandBinding(BringToFront, BringToFrontExecuted, Order_Enabled));
            CommandBindings.Add(new CommandBinding(SendBackward, SendBackward_Executed, Order_Enabled));
            CommandBindings.Add(new CommandBinding(SendToBack, SendToBackExecuted, Order_Enabled));
            CommandBindings.Add(new CommandBinding(AlignTop, AlignTopExecuted, AlignEnabled));
            CommandBindings.Add(new CommandBinding(AlignVerticalCenters, AlignVerticalCenters_Executed, AlignEnabled));
            CommandBindings.Add(new CommandBinding(AlignBottom, AlignBottom_Executed, AlignEnabled));
            CommandBindings.Add(new CommandBinding(AlignLeft, AlignLeft_Executed, AlignEnabled));
            CommandBindings.Add(new CommandBinding(AlignHorizontalCenters, AlignHorizontalCentersExecuted, AlignEnabled));
            CommandBindings.Add(new CommandBinding(AlignRight, AlignRight_Executed, AlignEnabled));
            CommandBindings.Add(new CommandBinding(DistributeHorizontal, DistributeHorizontal_Executed,DistributeEnabled));
            CommandBindings.Add(new CommandBinding(DistributeVertical, DistributeVerticalExecuted, DistributeEnabled));
            CommandBindings.Add(new CommandBinding(SelectAll, SelectAllExecuted, SelectAllEnabled));
            CommandBindings.Add(new CommandBinding(SameWidth, MakeSameWidthExecuted, AlignEnabled));
            CommandBindings.Add(new CommandBinding(SameHeight, MakeSameHeightExecuted, AlignEnabled));
            CommandBindings.Add(new CommandBinding(SameSize, MakeSameSizeExecuted, AlignEnabled));
            
            AllowDrop = true;
            Clipboard.Clear();
        }


        private void CopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
        }

        private void SelectAllEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Children.OfType<DesignerItem>().Any();
        }

        private void CopyEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Any();
        }

        public static XElement SerializeDesignerItems(IEnumerable<DesignerItem> designerItems)
        {
            // Remove the images they cause problems in serialisation and are not needed
            var enumerable = designerItems as DesignerItem[] ?? designerItems.ToArray();

            foreach (var designerItem in enumerable)
            {
                var adventureObject = AdventureGameDesignerViewModel.Instance.FindObjectByGuid(designerItem.ID);
                adventureObject?.SuppressImagePath();
            }

            var serializedItems = new XElement("DesignerItems",
                from item in enumerable
                let contentXaml = XamlWriter.Save(item.Content)
                select new XElement("DesignerItem",
                    new XElement("Left", GetLeft(item)),
                    new XElement("Top", GetTop(item)),
                    new XElement("Width", item.Width),
                    new XElement("Height", item.Height),
                    new XElement("ID", item.ID),
                    new XElement("zIndex", GetZIndex(item)),
                    new XElement("IsGroup", item.IsGroup),
                    new XElement("ParentId", item.ParentId),
                    new XElement("Content", contentXaml),
                    new XElement("StencilObjectType", item.StencilObjectType)
                    )
                );

            foreach (var designerItem in enumerable)
            {
                var adventureObject = AdventureGameDesignerViewModel.Instance.FindObjectByGuid(designerItem.ID);
                adventureObject?.RestoreImagePath();
            }

            return serializedItems;
        }

        public static DesignerItem DeserializeDesignerItem(XElement itemXml, Guid id,
           double offsetX, double offsetY)
        {
            var item = new DesignerItem(id)
            {
                Width = double.Parse(itemXml.Element("Width").Value, CultureInfo.InvariantCulture),
                Height = double.Parse(itemXml.Element("Height").Value, CultureInfo.InvariantCulture),
                ParentId = new Guid(itemXml.Element("ParentId").Value),
                IsGroup = bool.Parse(itemXml.Element("IsGroup").Value),
                StencilObjectType = (ObjectType)Enum.Parse(typeof(ObjectType),itemXml.Element("StencilObjectType").Value)
            };

            SetLeft(item, double.Parse(itemXml.Element("Left").Value, CultureInfo.InvariantCulture) + offsetX);
            SetTop(item, double.Parse(itemXml.Element("Top").Value, CultureInfo.InvariantCulture) + offsetY);
            SetZIndex(item, int.Parse(itemXml.Element("zIndex").Value));
            var content = XamlReader.Load(XmlReader.Create(new StringReader(itemXml.Element("Content").Value)));
            item.Content = content;
            return item;
        }

        private void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var root = LoadSerializedDataFromClipBoard();

            if (root == null)
            {
                return;
            }

            // create DesignerItems
            var mappingOldToNewIDs = new Dictionary<Guid, Guid>();
            var newItems = new List<ISelectable>();
            var itemsXml = root.Elements("DesignerItems").Elements("DesignerItem");

            var offsetX = double.Parse(root.Attribute("OffsetX").Value, CultureInfo.InvariantCulture);
            var offsetY = double.Parse(root.Attribute("OffsetY").Value, CultureInfo.InvariantCulture);

            foreach (var itemXml in itemsXml)
            {
                var oldId = new Guid(itemXml.Element("ID").Value);
                var newId = Guid.NewGuid();
                mappingOldToNewIDs.Add(oldId, newId);
                var item = DeserializeDesignerItem(itemXml, newId, offsetX, offsetY);
                Children.Add(item);
                SetConnectorDecoratorTemplate(item);
                newItems.Add(item);

                var itemObjectType = AdventureObjectHelper.ProcessNewAdventureObject(item);
            }

            // update group hierarchy
            SelectionService.ClearSelection();

            foreach (var selectable in newItems)
            {
                var el = (DesignerItem) selectable;
                if (el.ParentId != Guid.Empty)
                {
                    el.ParentId = mappingOldToNewIDs[el.ParentId];
                }
            }
            
            foreach (DesignerItem item in newItems)
            {
                if (item.ParentId == Guid.Empty)
                {
                    SelectionService.AddToSelection(item);
                }
            }

            // create Connections
            var connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (var connectionXML in connectionsXML)
            {
                var oldSourceID = new Guid(connectionXML.Element("SourceID").Value);
                var oldSinkID = new Guid(connectionXML.Element("SinkID").Value);

                if (mappingOldToNewIDs.ContainsKey(oldSourceID) && mappingOldToNewIDs.ContainsKey(oldSinkID))
                {
                    var newSourceID = mappingOldToNewIDs[oldSourceID];
                    var newSinkID = mappingOldToNewIDs[oldSinkID];

                    var sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                    var sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                    var sourceConnector = GetConnector(this, newSourceID, sourceConnectorName);
                    var sinkConnector = GetConnector(this, newSinkID, sinkConnectorName);

                    var connection = new Connection(sourceConnector, sinkConnector);
                    SetZIndex(connection, int.Parse(connectionXML.Element("zIndex").Value));
                    Children.Add(connection);

                    SelectionService.AddToSelection(connection);

                    ConnectionLinkerHelper.ProcessExit(sourceConnector, sinkConnector, connection);
                }
            }

            BringToFront.Execute(null, this);

            // update paste offset
            root.Attribute("OffsetX").Value = (offsetX + 10).ToString();
            root.Attribute("OffsetY").Value = (offsetY + 10).ToString();
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
        }

        private void PasteEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);
        }

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var designerCanvas = sender as  DesignerCanvas;

            if (designerCanvas?.GetCanvasType() == CanvasType.AdventureDesigner)
            {
                DeleteAdventureObjectCurrentSelection();

                AdventureDesigner.Instance.CleanUpParentsAfterDelete();
                AdventureDesigner.Instance.CleanUpChildrenAfterDelete();
            }
            else if (designerCanvas?.GetCanvasType() == CanvasType.Conversation)
            {
                DeleteConversationCurrentSelection();
            }
        }

        private void DeleteEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Any();
        }

        private void CutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
            DeleteAdventureObjectCurrentSelection();
        }

        private void CutEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Any();
        }

        private void ShowInExplorerEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var canExecute = false;

            var item = e.OriginalSource as DesignerItem;

            var placeableObject =
                AdventureGameDesignerViewModel.Instance.PlaceableObjects.FirstOrDefault(obj =>
                    obj.ControlId == item?.ID);

            if (placeableObject?.GetParentRoom() != null)
            {
                canExecute = true;
            }

            var room =
                AdventureGameDesignerViewModel.Instance.Rooms.FirstOrDefault(obj =>
                    obj.ControlId == item?.ID);
            
            if (room != null)
            {
                canExecute = true;
            }

            e.CanExecute = canExecute;
        }

        private void ShowInExplorerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is DesignerItem itemClicked)
            {
                AdventureDesigner.Instance.ShowEntityExplorer(itemClicked);
            }
        }

        private void ShowInConversationExplorerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is DesignerItem itemClicked)
            {
                ConversationDesigner.ConversationDesigner.Instance.ShowConversationDesigner(itemClicked);
            }
        }

        private void GroupExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var items = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                        where item.ParentId == Guid.Empty
                        select item).ToList();

            var rect = items.GetBoundingRectangle();

            var groupItem = new DesignerItem
            {
                IsGroup = true,
                Width = rect.Width,
                Height = rect.Height
            };

            SetLeft(groupItem, rect.Left);
            SetTop(groupItem, rect.Top);
            var groupCanvas = new Canvas();
            groupItem.Content = groupCanvas;
            SetZIndex(groupItem, Children.Count);
            Children.Add(groupItem);

            foreach (var item in items)
            {
                item.ParentId = groupItem.ID;
            }

            SelectionService.SelectItem(groupItem);
        }

        private void GroupEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var count = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                         where item.ParentId == Guid.Empty
                         select item).Count();

            e.CanExecute = count > 1;
        }

        private void UngroupExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var groups = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          where item.IsGroup && item.ParentId == Guid.Empty
                          select item).ToArray();

            foreach (var groupRoot in groups)
            {
                var children = from child in SelectionService.CurrentSelection.OfType<DesignerItem>()
                               where child.ParentId == groupRoot.ID
                               select child;

                foreach (var child in children)
                    child.ParentId = Guid.Empty;

                SelectionService.RemoveFromSelection(groupRoot);
                Children.Remove(groupRoot);
                UpdateZIndex();
            }
        }

        private void Ungroup_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentId != Guid.Empty
                              select item;


            e.CanExecute = groupedItem.Any();
        }

        private void BringForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var ordered = (from item in SelectionService.CurrentSelection
                                       orderby GetZIndex(item as UIElement) descending
                                       select item as UIElement).ToList();

            var count = Children.Count;

            for (var i = 0; i < ordered.Count; i++)
            {
                var currentIndex = GetZIndex(ordered[i]);
                var newIndex = Math.Min(count - 1 - i, currentIndex + 1);
                if (currentIndex != newIndex)
                {
                    SetZIndex(ordered[i], newIndex);
                    var it = Children.OfType<UIElement>().Where(item => GetZIndex(item) == newIndex);

                    foreach (var elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
        }

        private void Order_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Any();
        }

        private void BringToFrontExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            var childrenSorted = (from UIElement item in Children
                                              orderby GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();

            var i = 0;
            var j = 0;
            foreach (var item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    var idx = GetZIndex(item);
                    SetZIndex(item, childrenSorted.Count - selectionSorted.Count + j++);
                }
                else
                {
                    SetZIndex(item, i++);
                }
            }
        }

        private void SendBackward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var ordered = (from item in SelectionService.CurrentSelection
                                       orderby GetZIndex(item as UIElement) ascending
                                       select item as UIElement).ToList();

            var count = Children.Count;

            for (var i = 0; i < ordered.Count; i++)
            {
                var currentIndex = GetZIndex(ordered[i]);
                var newIndex = Math.Max(i, currentIndex - 1);
                if (currentIndex != newIndex)
                {
                    SetZIndex(ordered[i], newIndex);
                    var it = Children.OfType<UIElement>().Where(item => GetZIndex(item) == newIndex);

                    foreach (var elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
        }

        private void SendToBackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            var childrenSorted = (from UIElement item in Children
                                              orderby GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();
            var i = 0;
            var j = 0;
            foreach (var item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    var idx = GetZIndex(item);
                    SetZIndex(item, j++);

                }
                else
                {
                    SetZIndex(item, selectionSorted.Count + i++);
                }
            }
        }

        private void AlignTopExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                where item.ParentId == Guid.Empty
                select item).ToList();

            if (selectedItems.Count > 1)
            {
                var top = GetTop(selectedItems.First());

                foreach (var item in selectedItems)
                {
                    var delta = top - GetTop(item);
                    foreach (var groupable in SelectionService.GetGroupMembers(item))
                    {
                        var di = (DesignerItem) groupable;
                        SetTop(di, GetTop(di) + delta);
                    }
                }
            }
        }

        private void AlignEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentId == Guid.Empty
                              select item;


            e.CanExecute = groupedItem.Count() > 1;
        }

        private void AlignVerticalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentId == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                var bottom = GetTop(selectedItems.First()) + selectedItems.First().Height / 2;

                foreach (var item in selectedItems)
                {
                    var delta = bottom - (GetTop(item) + item.Height / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        SetTop(di, GetTop(di) + delta);
                    }
                }
            }
        }

        private void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentId == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                var bottom = GetTop(selectedItems.First()) + selectedItems.First().Height;

                foreach (var item in selectedItems)
                {
                    var delta = bottom - (GetTop(item) + item.Height);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        SetTop(di, GetTop(di) + delta);
                    }
                }
            }
        }

        private void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentId == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                var left = GetLeft(selectedItems.First());

                foreach (var item in selectedItems)
                {
                    var delta = left - GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        SetLeft(di, GetLeft(di) + delta);
                    }
                }
            }
        }

        private void MakeSameSizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResizeHelper(true, true);
        }

        private void MakeSameWidthExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResizeHelper(true, false);
        }

        private void MakeSameHeightExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResizeHelper(false, true);
        }

        private void ResizeHelper(bool sameWidths, bool sameHeights)
        {
            var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                where item.ParentId == Guid.Empty
                select item).ToList();

            if (selectedItems.Count > 1)
            {
                var baseWidth = selectedItems.First().ActualWidth;
                var baseHeight = selectedItems.First().ActualHeight;

                foreach (var item in selectedItems)
                {
                    if (sameWidths)
                    {
                        item.Width = baseWidth;
                    }

                    if (sameHeights)
                    {
                        item.Height = baseHeight;
                    }
                }
            }
        }

        private void AlignHorizontalCentersExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentId == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                var center = GetLeft(selectedItems.First()) + selectedItems.First().Width / 2;

                foreach (var item in selectedItems)
                {
                    var delta = center - (GetLeft(item) + item.Width / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        SetLeft(di, GetLeft(di) + delta);
                    }
                }
            }
        }

        private void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentId == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                var right = GetLeft(selectedItems.First()) + selectedItems.First().Width;

                foreach (var item in selectedItems)
                {
                    var delta = right - (GetLeft(item) + item.Width);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        SetLeft(di, GetLeft(di) + delta);
                    }
                }
            }
        }

        private void DistributeHorizontal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentId == Guid.Empty
                                let itemLeft = GetLeft(item)
                                orderby itemLeft
                                select item;

            if (selectedItems.Count() > 1)
            {
                var left = double.MaxValue;
                var right = double.MinValue;
                double sumWidth = 0;
                foreach (var item in selectedItems)
                {
                    left = Math.Min(left, GetLeft(item));
                    right = Math.Max(right, GetLeft(item) + item.Width);
                    sumWidth += item.Width;
                }

                var distance = Math.Max(0, (right - left - sumWidth) / (selectedItems.Count() - 1));
                var offset = GetLeft(selectedItems.First());

                foreach (var item in selectedItems)
                {
                    var delta = offset - GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        SetLeft(di, GetLeft(di) + delta);
                    }
                    offset = offset + item.Width + distance + (item.Width/2);

                    SnapToGrid(item, new List<UIElement>());
                }
            }
        }

        private void DistributeEnabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentId == Guid.Empty
                              select item;


            e.CanExecute = groupedItem.Count() > 1;
        }

        private void DistributeVerticalExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentId == Guid.Empty
                                let itemTop = GetTop(item)
                                orderby itemTop
                                select item;

            if (selectedItems.Count() > 1)
            {
                var top = double.MaxValue;
                var bottom = double.MinValue;
                double sumHeight = 0;
                foreach (var item in selectedItems)
                {
                    top = Math.Min(top, GetTop(item));
                    bottom = Math.Max(bottom, GetTop(item) + item.Height);
                    sumHeight += item.Height;
                }

                var distance = Math.Max(0, (bottom - top - sumHeight) / (selectedItems.Count() - 1));
                var offset = GetTop(selectedItems.First());

                foreach (var item in selectedItems)
                {
                    var delta = offset - GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        SetTop(di, GetTop(di) + delta);
                    }
                    offset = offset + item.Height + distance + (item.Height / 2);

                    SnapToGrid(item, new List<UIElement>());
                }
            }
        }

        private void SelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.SelectAll();
        }


        public static XElement SerializeConnections(IEnumerable<Connection> connections)
        {
            var serializedConnections = new XElement("Connections",
                           from connection in connections
                           select new XElement("Connection",
                                      new XElement("SourceID", connection.Source.ParentDesignerItem.ID),
                                      new XElement("SinkID", connection.Sink.ParentDesignerItem.ID),
                                      new XElement("SourceConnectorName", connection.Source.Name),
                                      new XElement("SinkConnectorName", connection.Sink.Name),
                                      new XElement("SourceArrowSymbol", connection.SourceArrowSymbol),
                                      new XElement("SinkArrowSymbol", connection.SinkArrowSymbol),
                                      new XElement("zIndex", GetZIndex(connection)),
                                      new XElement("ID", connection.ID)
                                     )
                                  );

            return serializedConnections;
        }


        private XElement LoadSerializedDataFromClipBoard()
        {
            if (Clipboard.ContainsData(DataFormats.Xaml))
            {
                var clipboardData = Clipboard.GetData(DataFormats.Xaml) as string;

                if (string.IsNullOrEmpty(clipboardData))
                {
                    return null;
                }

                return XElement.Load(new StringReader(clipboardData));
            }

            return null;
        }
        
        private void CopyCurrentSelection()
        {
            var selectedDesignerItems =
                SelectionService.CurrentSelection.OfType<DesignerItem>();

            var selectedConnections =
                SelectionService.CurrentSelection.OfType<Connection>().ToList();

            foreach (var connection in Children.OfType<Connection>())
            {
                if (!selectedConnections.Contains(connection))
                {
                    var sourceItem = (from item in selectedDesignerItems
                                               where item.ID == connection.Source.ParentDesignerItem.ID
                                               select item).FirstOrDefault();

                    var sinkItem = (from item in selectedDesignerItems
                                             where item.ID == connection.Sink.ParentDesignerItem.ID
                                             select item).FirstOrDefault();

                    if (sourceItem != null &&
                        sinkItem != null &&
                        BelongToSameGroup(sourceItem, sinkItem))
                    {
                        selectedConnections.Add(connection);
                    }
                }
            }

            var designerItemsXML = SerializeDesignerItems(selectedDesignerItems);
            var connectionsXML = SerializeConnections(selectedConnections);

            var root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            root.Add(new XAttribute("OffsetX", 10));
            root.Add(new XAttribute("OffsetY", 10));

            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
        }

        private void DeleteConversationCurrentSelection()
        {
            foreach (var connection in SelectionService.CurrentSelection.OfType<Connection>())
            {
                Children.Remove(connection);
                ConversationDesigner.ConversationDesigner.Instance.RemoveChildById(
                    connection.Source.ParentDesignerItem.ID, connection.Sink.ParentDesignerItem.ID);

                connection.CleanUpConnection();
            }

            foreach (var item in SelectionService.CurrentSelection.OfType<DesignerItem>())
            {
                var cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;

                var connectors = new List<Connector>();
                DependancyObjectHelper.GetConnectors(cd, connectors);

                foreach (var connector in connectors)
                {
                    foreach (var connection in connector.Connections)
                    {
                        Children.Remove(connection);
                        ConversationDesigner.ConversationDesigner.Instance.RemoveChildById(
                            connection.Source.ParentDesignerItem.ID, connection.Sink.ParentDesignerItem.ID);
                    }
                }

                Children.Remove(item);

                ConversationDesigner.ConversationDesigner.Instance.RemoveItemById(item.ID);
            }

            SelectionService.ClearSelection();
            UpdateZIndex();
        }

        private void DeleteAdventureObjectCurrentSelection()
        {
            AdventureDesigner.Instance.DisableExplorer = true;

            try
            {
                foreach (var connection in SelectionService.CurrentSelection.OfType<Connection>())
                {
                    Children.Remove(connection);
                    AdventureGameDesignerViewModel.Instance.DeleteExitByGuid(connection.ID);
                    AdventureDesigner.Instance.ClearPropertiesOfObject(connection.ID);
                    connection.CleanUpConnection();
                }

                foreach (var item in SelectionService.CurrentSelection.OfType<DesignerItem>())
                {
                    var cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;

                    var connectors = new List<Connector>();
                    DependancyObjectHelper.GetConnectors(cd, connectors);

                    foreach (var connector in connectors)
                    {
                        foreach (var con in connector.Connections)
                        {
                            Children.Remove(con);
                            AdventureGameDesignerViewModel.Instance.DeleteExitByGuid(con.ID);
                            AdventureDesigner.Instance.ClearPropertiesOfObject(con.ID);
                        }
                    }

                    Children.Remove(item);

                    DeleteObject(item.ID);
                }

                SelectionService.ClearSelection();
                UpdateZIndex();
            }
            finally
            {
                AdventureDesigner.Instance.DisableExplorer = false;
            }
        }

        private void DeleteObject(Guid id)
        {
            if (AdventureObjectHelper.GetIsRoomDesigner(Name))
            {
                AdventureGameDesignerViewModel.Instance.DeleteRoomByGuid(id);
            }
            else
            {
                AdventureGameDesignerViewModel.Instance.DeleteObjectByGuid(id);
            }

            AdventureDesigner.Instance.ClearPropertiesOfObject(id);
        }

        private void UpdateZIndex()
        {
            var ordered = (from UIElement item in Children
                                       orderby GetZIndex(item as UIElement)
                                       select item as UIElement).ToList();

            for (var i = 0; i < ordered.Count; i++)
            {
                SetZIndex(ordered[i], i);
            }
        }

        public static Connector GetConnector(DesignerCanvas designerCanvas, Guid itemId, 
            string connectorName)
        {
            var designerItem = (from item in designerCanvas.Children.OfType<DesignerItem>()
                                         where item.ID == itemId
                                         select item).FirstOrDefault();

            var connectorDecorator = designerItem.Template.FindName("PART_ConnectorDecorator", designerItem) as Control;
            connectorDecorator.ApplyTemplate();

            return connectorDecorator.Template.FindName(connectorName, connectorDecorator) as Connector;
        }

        private bool BelongToSameGroup(IGroupable item1, IGroupable item2)
        {
            var root1 = SelectionService.GetGroupRoot(item1);
            var root2 = SelectionService.GetGroupRoot(item2);

            return (root1.ID == root2.ID);
        }

        public void SetDataContext(object context)
        {
            DataContext = context;
        }
    }
}
