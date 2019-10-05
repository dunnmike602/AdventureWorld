using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DiagramDesigner.Extensions
{
    public static class TreeViewExtensions
    {
        public static void Expand(this ItemsControl treeView, bool state)
        {
            ExpandSubContainers(treeView, state);
        }

       private static void ExpandSubContainers(ItemsControl parentContainer, bool state)
       {
            foreach (var item in parentContainer.Items)
            {
                if (!(parentContainer.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem currentContainer))
                {
                    currentContainer = item as TreeViewItem;
                }

                if (currentContainer?.Items.Count > 0)
                {
                    // Expand the current item. 
                    currentContainer.IsExpanded = state;

                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        // If the sub containers of current item is not ready, we need to wait until 
                        // they are generated. 
                        currentContainer.ItemContainerGenerator.StatusChanged += delegate
                        {
                            ExpandSubContainers(currentContainer, state);
                        };
                    }
                    else
                    {
                        // If the sub containers of current item is ready, we can directly go to the next 
                        // iteration to expand them. 
                        ExpandSubContainers(currentContainer, state);
                    }
                }
            }
        }
    }
}