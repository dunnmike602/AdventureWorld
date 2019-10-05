using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using DiagramDesigner.AdventureWorld.Domain;

namespace DiagramDesigner.AdventureWorld.CustomCollections
{
    public class ObservableItemCollection<T> : ObservableCollection<T>
        where T : IdentifableObjectBase
    {
        public ObservableItemCollection(List<T> list) : base(list) { }

        public ObservableItemCollection() : base() { }

        private void DoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var changedObject = (T) sender;

            for (var index = 0; index < Items.Count; index++)
            {
                var item = Items[index];

                if (item.ControlId == changedObject.ControlId)
                {
                    this[index] = changedObject;
                }
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= DoPropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += DoPropertyChanged;
                }
            }

            base.OnCollectionChanged(e);
        }
    }
}
