using System.Collections.ObjectModel;

namespace DiagramDesigner.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddIfNotNull<T>(this ObservableCollection<T> collection, 
            T item) where T : class
        {
            if (item != null)
            {
                collection.Add(item);
            }
        }
    }
}
