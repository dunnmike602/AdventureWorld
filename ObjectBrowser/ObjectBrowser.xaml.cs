using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ReflectionHelper.Core;
using ReflectionHelper.Core.Extensions;
using MemberDescriptor = ReflectionHelper.Core.MemberDescriptor;

namespace MLDComputing.ObjectBrowser
{
    public partial class ObjectBrowser : INotifyPropertyChanged
    {
        public event SnippitGeneratedEventHandler SnippitGenerated;

        private static ApplicationDomainDescriptor _allAssembliesCache;
        private ObservableCollection<MemberDescriptor> _memberList;
        private ObservableCollection<NamespaceDescriptor> _searchedData;

        public ApplicationDomainDescriptor AllAssemblies
        {
            get => _allAssembliesCache;
            set
            {
                _allAssembliesCache = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NamespaceDescriptor> SearchedData
        {
            get => _searchedData;
            set
            {
                _searchedData = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MemberDescriptor> MemberList
        {
            get => _memberList;
            set
            {
                _memberList = value;
                OnPropertyChanged();
            }
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<string> SearchItems
        {
            get => (ObservableCollection<string>) GetValue(SearchItemsProperty);
            set => SetValue(SearchItemsProperty, value);
        }

        public static readonly DependencyProperty SearchItemsProperty = DependencyProperty.Register(
            "SearchItems", typeof(ObservableCollection<string>), typeof(ObjectBrowser),
            new FrameworkPropertyMetadata()
            {
                BindsTwoWayByDefault = true
            });

        public string XmlDocumentationFilesLocation
        {
            get => (string)GetValue(XmlDocumentationFilesLocationProperty);
            set => SetValue(XmlDocumentationFilesLocationProperty, value);
        }

        public static readonly DependencyProperty XmlDocumentationFilesLocationProperty = DependencyProperty.Register(
            "XmlDocumentationFilesLocation", typeof(string), typeof(ObjectBrowser),
            new FrameworkPropertyMetadata()
            {
                BindsTwoWayByDefault = true
            });

        public Dictionary<string, List<string>> AssemblyAndNamespaceFilter
        {
            get => (Dictionary<string, List<string>>)GetValue(AssemblyAndNamespaceFilterProperty);
            set => SetValue(AssemblyAndNamespaceFilterProperty, value);
        }

        public static readonly DependencyProperty AssemblyAndNamespaceFilterProperty = DependencyProperty.Register(
            "AssemblyAndNamespaceFilter", typeof(Dictionary<string, List<string>>), typeof(ObjectBrowser),
            new FrameworkPropertyMetadata()
            {
                DefaultValue = null,
                BindsTwoWayByDefault = true
            });

        public ObjectBrowser()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;

            Loaded += ObjectBrowserOnLoaded;

            SearchExplorer.Visibility = Visibility.Collapsed;
            ObjectExplorer.Visibility = Visibility.Visible;
        }

        private void InitialiseData(Progress<int> progressMonitor, Dictionary<string, List<string>> filter)
        {
            if (_allAssembliesCache == null)
            {
                ApplicationDomainDescriptor.ProgressMonitor = progressMonitor;

                _allAssembliesCache = new ApplicationDomainDescriptor(AppDomain.CurrentDomain, filter);
            }
        }

        private async void ObjectBrowserOnLoaded(object sender, RoutedEventArgs e)
        {
            ApplicationDomainDescriptor.XmlDocumentationFilesLocation = XmlDocumentationFilesLocation;

            Progress.Visibility = Visibility.Visible;
            LayoutRoot.Visibility = Visibility.Collapsed;

            var progress = new Progress<int>(value => { LoadProgress.Value = value; });

            var filter = AssemblyAndNamespaceFilter;

            await Task.Run(() => { InitialiseData(progress, filter); });

            Progress.Visibility = Visibility.Collapsed;
            LayoutRoot.Visibility = Visibility.Visible;

            OnPropertyChanged(nameof(AllAssemblies));
        }

        private void ObjectExplorerOnSelected(object sender, RoutedEventArgs e)
        {
            if (ObjectExplorer.SelectedItem is AssemblyDescriptor assemblyDescriptor)
            {
                ProcessAssembly(assemblyDescriptor);
                return;
            }

            if (ObjectExplorer.SelectedItem is NamespaceDescriptor namespaceDescriptor)
            {
                ProcessNamespace(namespaceDescriptor);
                return;
            }

            if (ObjectExplorer.SelectedItem is ReflectionHelper.Core.TypeDescriptor typeDescriptor)
            {
                ProcessType(typeDescriptor);
            }
        }
       
        private void SearchExplorerOnSelected(object sender, RoutedEventArgs e)
        {
            if (SearchExplorer.SelectedItem is NamespaceDescriptor namespaceDescriptor)
            {
                ProcessNamespace(namespaceDescriptor);
                return;
            }

            if (SearchExplorer.SelectedItem is ReflectionHelper.Core.TypeDescriptor typeDescriptor)
            {
                ProcessType(typeDescriptor);
                return;
            }

            if (SearchExplorer.SelectedItem is MemberDescriptor memberDescriptor)
            {
                MemberList = null;
                ProcessMember(memberDescriptor);
            }
        }

        private void ProcessType(ReflectionHelper.Core.TypeDescriptor typeDescriptor)
        {
            MemberList = new ObservableCollection<MemberDescriptor>(typeDescriptor.MemberDescriptors);

            SummaryPanel.Inlines.Clear();
            SummaryPanel.Inlines.Add(typeDescriptor.TypeDefinition);
            SummaryPanel.Inlines.Add(new LineBreak());
            SummaryPanel.Inlines.Add("    Member of ");

            var linkText = new Run(typeDescriptor.ContainingNamespace.DisplayName) { FontWeight = FontWeights.Bold };

            var hyperLink = new Hyperlink(linkText) { NavigateUri = new Uri("http://dummy") };

            hyperLink.RequestNavigate += (s, a) =>
            {
                typeDescriptor.IsSelected = false;
                typeDescriptor.ContainingNamespace.IsSelected = true;
            };

            SummaryPanel.Inlines.Add(hyperLink);

            SummaryPanel.Inlines.Add(new LineBreak());
            SummaryPanel.Inlines.Add(new LineBreak());
            
            if (!typeDescriptor.HasDescription())
            {
                typeDescriptor.CodeComments = typeDescriptor.CurrentType.Assembly.GetNameOnly().GetSummaryCommentForType(
                    typeDescriptor.CurrentType, ApplicationDomainDescriptor.XmlDocumentationFilesLocation);
            }

            if (typeDescriptor.HasDescription())
            {
                SummaryPanel.Inlines.Add(new Bold(new Run("Summary:")));
                SummaryPanel.Inlines.Add(new LineBreak());

                SummaryPanel.Inlines.Add(typeDescriptor.CodeComments);
            }
        }

        private void ProcessNamespace(NamespaceDescriptor namespaceDescriptor)
        {
            MemberList = null;

            SummaryPanel.Inlines.Clear();
            SummaryPanel.Inlines.Add("namespace ");
            SummaryPanel.Inlines.Add(new Bold(new Run(namespaceDescriptor.DisplayName)));
            SummaryPanel.Inlines.Add(new LineBreak());
            SummaryPanel.Inlines.Add("    Member of ");

            var linkText = new Run(namespaceDescriptor.ContainingAssembly.CurrentAssembly.GetNameOnly()) {FontWeight = FontWeights.Bold};

            var hyperLink = new Hyperlink(linkText) {NavigateUri = new Uri("http://dummy")};

            hyperLink.RequestNavigate += (s, a) =>
            {
                namespaceDescriptor.IsSelected = false;
                namespaceDescriptor.ContainingAssembly.IsSelected = true;
            };

            SummaryPanel.Inlines.Add(hyperLink);
        }

        private void ProcessAssembly(AssemblyDescriptor assemblyDescriptor)
        {
            MemberList = null;
            SummaryPanel.Inlines.Clear();
            SummaryPanel.Inlines.Add("Assembly ");
            SummaryPanel.Inlines.Add(new Bold(new Run(assemblyDescriptor.DisplayName)));
            SummaryPanel.Inlines.Add(new LineBreak());
            SummaryPanel.Inlines.Add("    " + assemblyDescriptor.Location);
        }

        private void DetailsPanelOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DetailsPanel.SelectedItem is MemberDescriptor memberDescriptor)
            {
                ProcessMember(memberDescriptor);
            }
        }
    
        private void ProcessMember(MemberDescriptor memberDescriptor)
        {
            SummaryPanel.Inlines.Clear();
            SummaryPanel.Inlines.Add(memberDescriptor.ImageDescription);
            SummaryPanel.Inlines.Add(new LineBreak());
            SummaryPanel.Inlines.Add("    Member of ");

            var linkText = new Run(memberDescriptor.ContainingType.DisplayName) { FontWeight = FontWeights.Bold };

            var hyperLink = new Hyperlink(linkText) { NavigateUri = new Uri("http://dummy") };

            hyperLink.RequestNavigate += (s, a) =>
            {
                MemberList = null;
                memberDescriptor.IsSelected = false;
                memberDescriptor.ContainingType.IsSelected = false;
                memberDescriptor.ContainingType.IsSelected = true;
            };

            SummaryPanel.Inlines.Add(hyperLink);

            SummaryPanel.Inlines.Add(new LineBreak());
            SummaryPanel.Inlines.Add(new LineBreak());

            if (!memberDescriptor.HasDescription())
            {
                var currentType = memberDescriptor.ContainingType.CurrentType;

                var documentationComments = currentType.Assembly.GetNameOnly().GetMethodDocumentation(
                    memberDescriptor.CurrentMember, ApplicationDomainDescriptor.XmlDocumentationFilesLocation);

                memberDescriptor.CodeComments = documentationComments.Summary;
                memberDescriptor.ParameterComments = documentationComments.ParameterComments;
                memberDescriptor.ReturnValue = documentationComments.ReturnValue;
            }

            if (memberDescriptor.HasDescription())
            {
                SummaryPanel.Inlines.Add(new Bold(new Run("Summary:")));
                SummaryPanel.Inlines.Add(new LineBreak());

                SummaryPanel.Inlines.Add(memberDescriptor.CodeComments);
            }

            if (memberDescriptor.ParameterComments.Any())
            {
                SummaryPanel.Inlines.Add(new LineBreak());
                SummaryPanel.Inlines.Add(new LineBreak());

                SummaryPanel.Inlines.Add(new Bold(new Run("Parameters:")));
                SummaryPanel.Inlines.Add(new LineBreak());

                foreach (var parameter in memberDescriptor.ParameterComments)
                {
                    SummaryPanel.Inlines.Add(new Italic(new Run($"{parameter.Key}: ")));
                    SummaryPanel.Inlines.Add(new Run(parameter.Value));
                    SummaryPanel.Inlines.Add(new LineBreak());
                }
            }

            if (memberDescriptor.HasReturnValue())
            {
                if (!memberDescriptor.ParameterComments.Any())
                {
                    SummaryPanel.Inlines.Add(new LineBreak());
                }

                SummaryPanel.Inlines.Add(new LineBreak());

                SummaryPanel.Inlines.Add(new Bold(new Run("Returns:")));
                SummaryPanel.Inlines.Add(new LineBreak());

                SummaryPanel.Inlines.Add(memberDescriptor.ReturnValue);
            }
        }

        private void EventSetterOnHandler(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
                item.BringIntoView();
            }
        }

        private void CmdSearchOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchObjectBrowser.Text))
            {
                return;
            }
            
            MemberList = null;

            if (!SearchItems.Any(item => item.IsEqualTo(SearchObjectBrowser.Text)))
            {
                SearchItems.Insert(0, SearchObjectBrowser.Text);
            }

            SearchedData = new ObservableCollection<NamespaceDescriptor>(_allAssembliesCache.Search(SearchObjectBrowser.Text));
            
            SearchExplorer.Visibility = Visibility.Visible;
            ObjectExplorer.Visibility = Visibility.Collapsed;
        }

        private void CmdCancelOnClick(object sender, RoutedEventArgs e)
        {
            MemberList = null;
            SearchedData = null;
            SearchObjectBrowser.Text = null;
            SearchExplorer.Visibility = Visibility.Collapsed;
            ObjectExplorer.Visibility = Visibility.Visible;
        }

        private void InvokeSnippitSelected(string snippit, string containingTypeName)
        {
            var eventHandler = SnippitGenerated;
            eventHandler?.Invoke(this, new SnippitGeneratedEventHandlerArgs() { Snippit = snippit, ContainingTypeName = containingTypeName});
        }

        private void MemberViewMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            if (DetailsPanel.SelectedItem is MemberDescriptor memberDescriptor)
            {
                InvokeSnippitSelected(memberDescriptor.DisplayName, memberDescriptor.ContainingType.CurrentType.Name);
            }
        }

        private void SearchMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            if (SearchExplorer.SelectedItem is ReflectionHelper.Core.TypeDescriptor typeDescriptor)
            {
                InvokeSnippitSelected(typeDescriptor.DisplayName, null);
                return;
            }

            if (SearchExplorer.SelectedItem is MemberDescriptor memberDescriptor)
            {
                InvokeSnippitSelected(memberDescriptor.DisplayName, memberDescriptor.ContainingType.CurrentType.Name);
            }
        }

        private void ExplorerMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            if (ObjectExplorer.SelectedItem is ReflectionHelper.Core.TypeDescriptor typeDescriptor)
            {
                InvokeSnippitSelected(typeDescriptor.DisplayName, null);
                return;
            }
        }
    }
}
