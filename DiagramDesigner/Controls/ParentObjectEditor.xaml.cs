using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using DiagramDesigner.AdventureWorld.Domain;
using ReactiveUI;
using ReflectionHelper.Annotations;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace DiagramDesigner.Controls
{
    public partial class ParentObjectEditor : ITypeEditor, INotifyPropertyChanged
    {
        private ReactiveList<AdventureObjectBase> _parents = new ReactiveList<AdventureObjectBase>() ;
        private PropertyItem _item;

        public AdventureObjectBase SelectedParentObject
        {
            get => _item?.Value as AdventureObjectBase;
            set
            {
                _item.Value = string.IsNullOrWhiteSpace(value?.BaseName) ? null : value;
                
                OnPropertyChanged();
            }
        }

        public ReactiveList<AdventureObjectBase> Parents
        {
            get => _parents;
            set
            {
                _parents = value;
                OnPropertyChanged();
            }
        }
        
        public ParentObjectEditor()
        {
            InitializeComponent();

            DataContext = this;
        }

        private bool IsAlreadyChild(IEnumerable<PlaceableObject> placeableObjects, PlaceableObject startObject)
        {
            var alreadyChild = false;

            if (startObject == null)
            {
                return false;
            }

            foreach (var nextObject in placeableObjects)
            {
                if (nextObject.ControlId == startObject.ControlId)
                {
                    alreadyChild = true;
                    break;
                }
                
                if (nextObject is AdventureWorld.Domain.Container container && container.PlaceableObjects.Count > 0)
                {
                    return IsAlreadyChild(container.PlaceableObjects, startObject);
                }
            }

            return alreadyChild;
        }

        private void PopulateParents()
        {
            Parents.Clear();

            foreach (var room in AdventureGameDesignerViewModel.Instance.RoomsList)
            {
                Parents.Add(room);
            }

            var selectedObject = (_item.ParentElement as PropertyGrid)?.SelectedObject as PlaceableObject;
            var containerSelectObject = selectedObject as AdventureWorld.Domain.Container;

            // Add the candidate parent object checking for any that would create recursive cycles
            foreach (var container in AdventureGameDesignerViewModel.Instance.PlaceableObjectsList.OfType<AdventureWorld.Domain.Container>())
            {
                if(selectedObject?.ControlId == container.ControlId)
                {
                    continue;
                }

                if(!(selectedObject is AdventureWorld.Domain.Container) || !IsAlreadyChild(containerSelectObject.PlaceableObjects, container))
                {
                    Parents.Add(container);
                }
            }

            // Must always have the selected item
            if (SelectedParentObject != null && Parents.All(parent => parent.ControlId != SelectedParentObject.ControlId))
            {
                Parents.Add(SelectedParentObject);
            }

            if (Parents.Count > 0)
            {
                Parents.Insert(0, new AdventureObjectBase());
            }
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            _item = propertyItem;

            PopulateParents();

            OnPropertyChanged(nameof(SelectedParentObject));
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
