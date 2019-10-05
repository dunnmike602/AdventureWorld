using System.Windows;
using System.Windows.Controls;

namespace DiagramDesigner.Controls.Behaviours
{
    public static class TreeViewItemBehavior
    {
        public static bool GetIsBroughtIntoViewWhenSelected(TreeViewItem treeViewItem)
        {
            return (bool)treeViewItem.GetValue(IsBroughtIntoViewWhenSelectedProperty);
        }

        public static void SetIsBroughtIntoViewWhenSelected(TreeViewItem treeViewItem, bool value)
        {
            treeViewItem.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
        }

        public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached(
                "IsBroughtIntoViewWhenSelected",
                typeof(bool),
                typeof(TreeViewItemBehavior),
                new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));

        static void OnIsBroughtIntoViewWhenSelectedChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (!(depObj is TreeViewItem item))
                return;

            if (e.NewValue is bool == false)
            {
                return;
            }

            if ((bool) e.NewValue)
            {
                item.Selected += OnTreeViewItemSelected;
            }
            else
            {
                item.Selected -= OnTreeViewItemSelected;
            }
        }

        static void OnTreeViewItemSelected(object sender, RoutedEventArgs e)
        {
            // Only react to the Selected event raised by the TreeViewItem
            // whose IsSelected property was modified. Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!ReferenceEquals(sender, e.OriginalSource))
                return;

            var item = e.OriginalSource as TreeViewItem;
            item?.BringIntoView();
        }
    }
}
