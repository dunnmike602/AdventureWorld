using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DiagramDesigner.Interfaces;

namespace DiagramDesigner.Symbols
{
    internal class SelectionService
    {
        private readonly DesignerCanvas _designerCanvas;

        private List<ISelectable> _currentSelection;

        internal List<ISelectable> CurrentSelection => _currentSelection ?? (_currentSelection = new List<ISelectable>());

        public SelectionService(DesignerCanvas canvas)
        {
            _designerCanvas = canvas;
        }

        internal void SelectItem(ISelectable item)
        {
            ClearSelection();
            AddToSelection(item);
            SetToolBarToCurrentDesigner();
        }

        private void SetToolBarToCurrentDesigner()
        {
            var buttonNames = new[]
                {
                    "CmdAlignHorizontalCenters", "CmdCopy", "CmdPaste", "CmdCut", "CmdDelete", "CmdAlignObjectsRight", "CmdSelectAll",
                    "CmdDistributeHorizontal", "CmdAlignObjectsCenteredVertical", "CmdAlignObjectsTop",
                    "CmdAlignObjectsBottom", "CmdDistributeVertical", "CmdGroup","CmdUnGroup", "CmdAlignObjectsCenteredHorizontal",
                    "CmdForward", "CmdBackward","CmdToFront", "CmdToBack","CmdMakeSameSize", "CmdMakeSameHeight","CmdMakeSameWidths"
                };

            foreach (var buttonName in buttonNames)
            {
                var cmdButton = (Button)Application.Current.MainWindow.FindName(buttonName);
                cmdButton.CommandTarget = _designerCanvas;
            }
        }

        internal void AddToSelection(ISelectable item)
        {
            if (item is IGroupable)
            {
                var groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = true;
                    CurrentSelection.Add(groupItem);
                }
            }
            else
            {
                item.IsSelected = true;
                CurrentSelection.Add(item);
            }
        }

        internal void RemoveFromSelection(ISelectable item)
        {
            if (item is IGroupable)
            {
                var groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = false;
                    CurrentSelection.Remove(groupItem);
                }
            }
            else
            {
                item.IsSelected = false;
                CurrentSelection.Remove(item);
            }
        }

        internal void ClearSelection()
        {
            CurrentSelection.ForEach(item => item.IsSelected = false);
            CurrentSelection.Clear();
        }

        internal void SelectAll()
        {
            ClearSelection();
            CurrentSelection.AddRange(_designerCanvas.Children.OfType<ISelectable>());
            CurrentSelection.ForEach(item => item.IsSelected = true);
        }

        internal List<IGroupable> GetGroupMembers(IGroupable item)
        {
            var list = _designerCanvas.Children.OfType<IGroupable>();
            var rootItem = GetRoot(list, item);
            return GetGroupMembers(list, rootItem);
        }

        internal IGroupable GetGroupRoot(IGroupable item)
        {
            var list = _designerCanvas.Children.OfType<IGroupable>();
            return GetRoot(list, item);
        }

        private static IGroupable GetRoot(IEnumerable<IGroupable> list, IGroupable node)
        {
            if (node == null || node.ParentId == Guid.Empty)
            {
                return node;
            }

            return (from item in list where item.ID == node.ParentId select GetRoot(list, item)).FirstOrDefault();
        }

        private static List<IGroupable> GetGroupMembers(IEnumerable<IGroupable> list, IGroupable parent)
        {
            var groupMembers = new List<IGroupable> {parent};

            var children = list.Where(node => node.ParentId == parent.ID);

            foreach (var child in children)
            {
                groupMembers.AddRange(GetGroupMembers(list, child));
            }

            return groupMembers;
        }
    }
}
