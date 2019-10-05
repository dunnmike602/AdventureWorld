using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DiagramDesigner.AdventureWorld.Extensions;
using MLDComputing.ObjectBrowser;
using ReflectionHelper.Annotations;

namespace DiagramDesigner
{
    public partial class ApiExplorer : INotifyPropertyChanged
    {
        public event SnippitGeneratedEventHandler SnippitGenerated;

        protected CompositeDisposable Disposables { get; set; } = new CompositeDisposable();

        private ObservableCollection<string> _currentSearchItems;

        public ObservableCollection<string> CurrentSearchItems
        {
            get => _currentSearchItems;
            set
            {
                _currentSearchItems = value;
                OnPropertyChanged();
            }
        }
        
        public ApiExplorer()
        {
            InitializeComponent();

            DataContext = this;

            XmlDocumentationFilesLocation = System.IO.Path.Combine(Environment.CurrentDirectory, "XmlDocumentation");
         
            AssemblyAndNamespaceFilter = new Dictionary<string, List<string>>
            {
                {
                    "AdventureLandCore",
                    new List<string> { "AdventureLandCore.Services.CoreApi.Interfaces", "AdventureLandCore.Domain"}
                },
                {"mscorlib", null},
                {"System", null},
                {"System.Xml", null}
            };

            LoadSettings();

            SetupSubscriptions();

            ObjectBrowser.SnippitGenerated += ObjectBrowserSnippitGenerated;
        }

        private void ObjectBrowserSnippitGenerated(object sender, SnippitGeneratedEventHandlerArgs args)
        {
            var eventHandler = SnippitGenerated;
            eventHandler?.Invoke(this, args);
        }

        private void SetupSubscriptions()
        {
            var baseNameDisposable =  Observable
                .FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    handler => (sender, args) => handler(args),
                    handler => CurrentSearchItems.CollectionChanged += handler,
                    handler => CurrentSearchItems.CollectionChanged -= handler).Subscribe(_ => SaveSearchSettings());
              
            Disposables.Add(baseNameDisposable);
        }

        private void SaveSearchSettings()
        {
            // Save max 50 items
            var itemsToSave = new ObservableCollection<string>(CurrentSearchItems.Take(50));
            Properties.Settings.Default.RecentSearches = itemsToSave.ToStringCollection();
            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            CurrentSearchItems = Properties.Settings.Default.RecentSearches.ToObservableCollection();
        }

        public string XmlDocumentationFilesLocation { get; set; }

        public Dictionary<string, List<string>> AssemblyAndNamespaceFilter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ApiExplorerOnClosed(object sender, EventArgs e)
        {
           Disposables.Dispose();
        }
    }
}
