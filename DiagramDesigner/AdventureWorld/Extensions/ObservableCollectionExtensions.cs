using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DiagramDesigner.AdventureWorld.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static ObservableCollection<string> ToObservableCollection(this StringCollection source)
        {
            if (source == null)
            {
                return new ObservableCollection<string>();
            }

            var target = new ObservableCollection<string>();

            foreach (var item in source)
            {
               target.Add(item);
            }

            return target;
        }

        public static StringCollection ToStringCollection(this ObservableCollection<string> source)
        {
            if (source == null)
            {
                return new StringCollection();
            }

            var target = new StringCollection();
            target.AddRange(source.ToArray());

            return target;
        }
    }
}