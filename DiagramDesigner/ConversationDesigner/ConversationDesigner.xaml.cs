using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Services.Helpers;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.AdventureWorld.Extensions;
using DiagramDesigner.ConversationDesigner.Helpers;
using DiagramDesigner.Helpers;
using DiagramDesigner.Interfaces;
using DiagramDesigner.Symbols;
using DiagramDesigner.Symbols.Helpers;
using ReactiveUI;
using SharedControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using AdventureObjectBase = DiagramDesigner.AdventureWorld.Domain.AdventureObjectBase;
using Conversation = DiagramDesigner.AdventureWorld.Domain.Conversation;
using ConversationObjectBase = DiagramDesigner.AdventureWorld.Domain.ConversationObjectBase;
using ConversationResponse = DiagramDesigner.AdventureWorld.Domain.ConversationResponse;
using ConversationText = DiagramDesigner.AdventureWorld.Domain.ConversationText;
using Npc = DiagramDesigner.AdventureWorld.Domain.Npc;

namespace DiagramDesigner.ConversationDesigner
{
    public partial class ConversationDesigner : INotifyPropertyChanged, IStatusMessageProvider
    {
        [Browsable(false)]
        public ObservableCollection<ScriptContainer> ScriptContainers { get; set; }

        private const string QuitCommand = "Quit";
        private const int MarginSize = 20;

        public static ConversationDesigner Instance => Application.Current.Windows.OfType<ConversationDesigner>().FirstOrDefault();
        public ReactiveCommand<Unit, Unit> RunConversationCommand { get; set; }
        public ReactiveCommand<string, Unit> RunSelectResponseCommand { get; set; }
        public RoutedCommand CheckCommand { get; set; } = new RoutedCommand();
        public RoutedCommand SaveCurrentProjectCommand { get; set; } = new RoutedCommand();
        public ReactiveCommand<Unit, Unit> SaveConversationCommand { get; set; }

        private double _scale = 1f;

        private bool _commandIsExecuting;
        private Conversation _selectedConversation;
        private string _conversationText;
        private ReactiveList<string> _currentResponses;
        private ReactiveList<ConversationResponse> _sortedResponses;
        private Npc _currentNpc;
        private ConversationTree _conversationTree;
        private ReactiveList<ConversationResponse> _conversationResponses;
        private ReactiveList<ConversationText> _conversationTexts;

        public List<ScriptContainerHeader> ScriptHeaders => new List<ScriptContainerHeader>
        {
            new ScriptContainerHeader
            {
                Header = "Conversation Start Scripts",
                Image = "/Resources/Images/ConStartScript.png",
                Type = ScriptTypes.ConversationStart
            },
            new ScriptContainerHeader
            {
                Header = "Npc Speaks Scripts",
                Image = "/Resources/Images/NpcSpeaksScript.png",
                Type = ScriptTypes.ConversationText
            },
            new ScriptContainerHeader
            {
                Header = "Player Responds Scripts",
                Image = "/Resources/Images/PlayerResScript.png",
                Type = ScriptTypes.ConversationResponse
            }
        };



        public Npc CurrentNpc
        {
            get => _currentNpc;
            set
            {
                _currentNpc = value;
                Title = $"Conversations for {CurrentNpc.BaseName}";
                OnPropertyChanged();
            }
        }

        public ReactiveList<ConversationResponse> SortedResponses
        {
            get => _sortedResponses;
            set
            {
                _sortedResponses = value;
                OnPropertyChanged();
            }
        }

        public ReactiveList<string> CurrentResponses
        {
            get => _currentResponses;
            set
            {
                _currentResponses = value;
                OnPropertyChanged();
            }
        }

        public double Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnPropertyChanged();
            }
        }

        public string ConversationText
        {
            get => _conversationText;
            set
            {
                _conversationText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ConversationTree ConversationTree
        {
            get => _conversationTree;
            set
            {
                _conversationTree = value;
                OnPropertyChanged();
            }
        }

        public ReactiveList<ConversationResponse> ConversationResponses
        {
            get => _conversationResponses;
            set
            {
                _conversationResponses = value;
                OnPropertyChanged();
            }
        }

        public ReactiveList<ConversationText> ConversationTexts
        {
            get => _conversationTexts;
            set
            {
                _conversationTexts = value;
                OnPropertyChanged();
            }
        }

        public Conversation SelectedConversation
        {
            get => _selectedConversation;
            set
            {
                _selectedConversation = value;
                OnPropertyChanged();
            }
        }

        public bool CommandIsExecuting
        {
            get => _commandIsExecuting;
            set
            {
                _commandIsExecuting = value;
                OnPropertyChanged();
            }
        }

        public ConversationDesigner()
        {
            InitializeComponent();

            DataContext = this;

            ConversationTree = new ConversationTree
            {
                Conversations = new ReactiveList<Conversation> { ChangeTrackingEnabled = true },
                ValidationItems = new ObservableCollection<ValidationItemAttribute>()
            };

            ConversationResponses = new ReactiveList<ConversationResponse> { ChangeTrackingEnabled = true };
            ConversationTexts = new ReactiveList<ConversationText> { ChangeTrackingEnabled = true };

            SetupCommands();
        }

        private void SetupSubScriptions()
        {
            ConversationTree.Conversations.ItemChanged.Subscribe(_ => BuildScriptObjects());
            ConversationTexts.ItemChanged.Subscribe(_ => BuildScriptObjects());
            ConversationResponses.ItemChanged.Subscribe(_ => BuildScriptObjects());

            ConversationTree.Conversations.ItemsAdded.Subscribe(_ => BuildScriptObjects());
            ConversationTexts.ItemsAdded.Subscribe(_ => BuildScriptObjects());
            ConversationResponses.ItemsAdded.Subscribe(_ => BuildScriptObjects());

            ConversationTree.Conversations.ItemsRemoved.Subscribe(_ => BuildScriptObjects());
            ConversationTexts.ItemsRemoved.Subscribe(_ => BuildScriptObjects());
            ConversationResponses.ItemsRemoved.Subscribe(_ => BuildScriptObjects());
        }

        public void LoadData(Guid playerGuid)
        {
            try
            {
                ScriptExplorer.Reset(ScriptHeaders);

                if (File.Exists(GetConversationsPath(playerGuid)))
                {
                    OpenNewFile();
                }

                SetupSubScriptions();
            }
            catch (Exception ex)
            {
                TaskDialogService.ShowError(this, ex.Message, ex.ToString());
            }
        }

        private void OpenNewFile()
        {
            ResetStatus();

            var root = LoadDocument(CurrentNpc.ControlId);

            LoadDesigner(root, ConDesigner);

            InitialiseObjects(root);

            ReapplyBindings();

            ScriptExplorer.Reset(ScriptHeaders);

            BuildScriptObjects();
        }

        private static XElement LoadDocument(Guid playerGuid)
        {
            try
            {
                return XElement.Load(GetConversationsPath(playerGuid));
            }
            catch (Exception ex)
            {
                throw new Exception("This file is missing, corrupt or does not appear to be the correct format for an Adventure Conversation file.", ex);
            }
        }

        private async Task SetUpValidationItems()
        {
            try
            {
                SetStatus("VALIDATING MODEL.....", Colors.Black, true);
                CheckModelButton.IsEnabled = true;
                await Task.Run(() => { SetUpValidationItemsHelper(); });

                if (IsModelValid())
                {
                    SetStatus("GAME MODEL IS VALID", Colors.Green, false);
                }
                else
                {
                    SetStatus("MODEL IS INVALID", Colors.Red, false);
                }
            }
            catch (Exception)
            {
                SetStatus(string.Empty, Colors.Transparent, false);
                throw;
            }
            finally
            {
                CheckModelButton.IsEnabled = true;
            }
        }

        private bool IsModelValid()
        {
            return ConversationTree.ValidationItems.Count(m => m.Severity == Severities.Error) == 0;
        }

        private void SetUpValidationItemsHelper()
        {
            ConversationTree.ValidationItems.Clear();

            ValidateConversationForDuplicateNames();

            ValidateConversationTextsForParentsAndText();

            ValidateConversationResponsesForParents();

            foreach (var conversationText in ConversationTexts)
            {
                ValidateForDescriptions(conversationText);
            }

            foreach (var conversationResponses in ConversationResponses)
            {
                ValidateForDescriptions(conversationResponses);
            }
        }

        public void ValidateForDescriptions(ConversationText source)
        {
            var items = ValidationItemHelper.CreateOptionsValidationItems(source, ShowConversation,
                ValidationCategories.ConversationText, source.BaseName, ValidationTypes.All, source.ControlId);

            ValidationItemHelper.Add(ConversationTree.ValidationItems, items);
        }

        public void ValidateForDescriptions(ConversationResponse source)
        {
            var items = ValidationItemHelper.CreateOptionsValidationItems(source, ShowConversation,
                ValidationCategories.ConversationResponse, source.BaseName, ValidationTypes.All, source.ControlId);

            ValidationItemHelper.Add(ConversationTree.ValidationItems, items);
        }

        private void ValidateConversationResponsesForParents()
        {
            var allParentItems = ConversationTexts.Where(con => con.ConversationResponses.Count > 0)
                .SelectMany(con => con.ConversationResponses.Select(conres => conres.ControlId)).ToList();

            foreach (var conversationResponse in ConversationResponses)
            {
                if (allParentItems.All(id => id != conversationResponse.ControlId))
                {
                    ConversationTree.ValidationItems.Add(BuildNoParentValidationItem(conversationResponse));
                }
            }
        }

        private void ValidateConversationTextsForParentsAndText()
        {
            var allParentItems = ConversationTree.Conversations.Where(con => con.ConversationText != null).Select(con => con.ConversationText.ControlId)
                .Union(ConversationResponses.Where(con => con.ConversationText != null).Select(con => con.ConversationText.ControlId)).ToList();

            foreach (var conversationText in ConversationTexts)
            {
                if (allParentItems.All(id => id != conversationText.ControlId))
                {
                    ConversationTree.ValidationItems.Add(BuildNoParentValidationItem(conversationText));
                }
            }
        }

        private ValidationItemAttribute BuildNoParentValidationItem(ConversationObjectBase conversationText)
        {
            return new ValidationItemAttribute
            {
                Action = ShowConversation,
                Severity = Severities.Error,
                ControlId = conversationText.ControlId,
                ValidationCategory = ValidationCategories.Conversation,
                Description = "This Conversation Object does not have a parent.",
                ValidationType = ValidationTypes.NoParent,
                Name = conversationText.BaseName
            };
        }

        private void ValidateConversationForDuplicateNames()
        {
            var duplicates = ConversationTree.Conversations.GroupBy(m => m.BaseName).Where(m => m.Count() > 1).ToList();

            if (duplicates.Any())
            {
                foreach (var duplicate in duplicates)
                {
                    ConversationTree.ValidationItems.Add(new ValidationItemAttribute
                    {
                        Action = ShowConversation,
                        Severity = Severities.Error,
                        ControlId = duplicate.First().ControlId,
                        ValidationCategory = ValidationCategories.Conversation,
                        Description = $"More than one Conversation has the name {duplicate.First().BaseName}",
                        ValidationType = ValidationTypes.Duplicate,
                        Name = duplicate.First().BaseName
                    });
                }
            }
        }

        private bool ShowConversation(Guid? controlId)
        {
            if (controlId != null)
            {
                var itemToSelect = ConDesigner.GetDesignerItemByUid(controlId.Value);

                if (itemToSelect != null)
                {
                    ConDesigner.SelectionService.ClearSelection();
                    ConDesigner.SelectionService.SelectItem(itemToSelect);
                    itemToSelect.BringIntoView();

                    ShowPropertiesForObject(controlId.Value);

                    return true;
                }
            }

            return false;
        }


        private void ReapplyBindings()
        {
            foreach (var designerItem in ConDesigner.Children.OfType<DesignerItem>())
            {
                AdventureObjectHelper.RebindConversationObject(designerItem);
            }
        }

        private void InitialiseObjects(XElement root)
        {
            var conversationsXml = root.Element("DataObjects")?.Element("Conversations")?.Value;
            ConversationTree.Conversations =
                new ReactiveList<Conversation>(DeserializeFromStringHelper<Conversation[]>(conversationsXml))
                {
                    ChangeTrackingEnabled = true
                };


            ConversationResponses =
                new ReactiveList<ConversationResponse>(GetAllResponses())
                {
                    ChangeTrackingEnabled = true
                };

            ConversationTexts =
                new ReactiveList<ConversationText>(GetAllTexts())
                {
                    ChangeTrackingEnabled = true
                };
        }

        private IList<ConversationText> GetAllTexts()
        {
            var texts = new List<ConversationText>();

            foreach (var conversation in ConversationTree.Conversations)
            {
                ProcessConversationText(conversation.ConversationText, texts);
                
            }

            return texts;
        }

        private void ProcessConversationText(ConversationText conversationText, List<ConversationText> texts)
        {
            if (conversationText == null || texts.Any(text => text.ControlId == conversationText.ControlId))
            {
                return;
            }
            
            texts.Add(conversationText);

            foreach (var response in conversationText.ConversationResponses)
            {
                if (response.ConversationText != null &&
                    texts.All(text => text.ControlId != response.ConversationText.ControlId))
                {
                    texts.Add(response.ConversationText);
                }

                ProcessConversationText(response.ConversationText, texts);
            }
        }

        private IEnumerable<ConversationResponse> GetAllResponses()
        {
            var responses = new List<ConversationResponse>();

            foreach (var conversation in ConversationTree.Conversations)
            {
                ProcessConversationResponses(conversation.ConversationText, responses);
            }

            return responses;
        }

        private void ProcessConversationResponses(ConversationText conversationText, List<ConversationResponse> responses)
        {
            if (conversationText?.ConversationResponses == null)
            {
                return;
            }

            foreach (var response in conversationText.ConversationResponses)
            {
                if (response == null || responses.Any(res => res.ControlId == response.ControlId))
                {
                    continue;
                }

                responses.Add(response);

                if (response.ConversationText != null)
                {
                    ProcessConversationResponses(response.ConversationText, responses);
                }
            }
        }

        private static void LoadDesigner(XContainer root, DesignerCanvas designerCanvas)
        {
            designerCanvas.InvalidateVisual();

            var itemsXml = root.Element(designerCanvas.Name)?.Elements("DesignerItems").Elements("DesignerItem");

            foreach (var item in from itemXml in itemsXml
                                 let id = new Guid(itemXml.Element("ID")?.Value)
                                 select DesignerCanvas.DeserializeDesignerItem(itemXml, id, 0, 0))
            {
                designerCanvas.Children.Add(item);
                DesignerCanvas.SetConnectorDecoratorTemplate(item);
            }

            var connectionsXml = root.Element(designerCanvas.Name).Elements("Connections").Elements("Connection");

            foreach (var connectionXml in connectionsXml)
            {
                var sourceId = new Guid(connectionXml.Element("SourceID").Value);
                var sinkId = new Guid(connectionXml.Element("SinkID").Value);

                var sourceConnectorName = connectionXml.Element("SourceConnectorName").Value;
                var sinkConnectorName = connectionXml.Element("SinkConnectorName").Value;

                var sourceConnector = DesignerCanvas.GetConnector(designerCanvas, sourceId, sourceConnectorName);
                var sinkConnector = DesignerCanvas.GetConnector(designerCanvas, sinkId, sinkConnectorName);

                var connection = new Connection(sourceConnector, sinkConnector)
                { ID = new Guid(connectionXml.Element("ID").Value) };

                Panel.SetZIndex(connection, int.Parse(connectionXml.Element("zIndex").Value));

                designerCanvas.Children.Add(connection);

                connection.ApplyTemplate();
            }
        }

        private void SetupCommands()
        {
            var canRun = this.WhenAnyValue(x => x.CommandIsExecuting).CombineLatest(this.WhenAnyValue(x => x.SelectedConversation),
                (isExecuting, selectedConversation) => !isExecuting && selectedConversation != null);

            RunConversationCommand = ReactiveCommand.Create(RunConversationCommandExecuted, canRun);

            SaveConversationCommand = ReactiveCommand.Create(RunSaveCommandExecuted);

            RunSelectResponseCommand = ReactiveCommand.Create<string>(RunSelectResponseCommandExecuted);

            CommandBindings.Add(new CommandBinding(CheckCommand, CheckExecuted));
            CommandBindings.Add(new CommandBinding(SaveCurrentProjectCommand, SaveCurrentProjectCommandExecuted));
        }

        private void SaveCurrentProjectCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RunSaveCommandExecuted();
        }

        private async void RunSaveCommandExecuted()
        {
            try
            {
                await SaveFile(GetDesignAsXElement());
            }
            catch (Exception exception)
            {
                TaskDialogService.ShowApplicationError(this, exception);
            }
        }

        private async void CheckExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResetStatus();

            await SetUpValidationItems();

            ShowErrors();
        }


        public void ShowErrors()
        {
            SetConversationObjectExplorerTabIndexesToIndex(ConversationObjectExplorerTabIndexes.Errors);
        }

        private void SetConversationObjectExplorerTabIndexesToIndex(ConversationObjectExplorerTabIndexes tabId)
        {
            ConversationObjectExplorer.SelectedIndex = (int)tabId;
        }

        private void RunSelectResponseCommandExecuted(string conversationResponse)
        {
            if (conversationResponse.IsEqualTo(QuitCommand))
            {
                SetComplete();
            }
            else
            {
                var npcConversationText = SortedResponses[Convert.ToInt32(conversationResponse) - 1].ConversationText;

                if (npcConversationText == null)
                {
                    SetComplete();
                }
                else
                {
                    SetConversationText(npcConversationText.Text.AddLineBreaks().PrefixLineBreaks());
                    SetResponseText(npcConversationText.ConversationResponses);
                }
            }
        }

        public IObservable<bool> RunConversationCommandCanExecute { get; set; }

        private void RunConversationCommandExecuted()
        {
            CommandIsExecuting = true;

            ConversationText = string.Empty;

            if (SelectedConversation.ConversationText != null)
            {
                SetConversationText(SelectedConversation.ConversationText.Text.AddLineBreaks(2));

                if (SelectedConversation.ConversationText.ConversationResponses.Any())
                {
                    SetResponseText(SelectedConversation.ConversationText.ConversationResponses);
                }
                else
                {
                    SetComplete();
                }
            }
            else
            {
                SetComplete();
            }
        }

        private void SetComplete()
        {
            SetConversationText("Conversation is complete...".PrefixLineBreaks());
            CommandIsExecuting = false;
            CurrentResponses = null;
            SortedResponses = null;
        }

        private void SetResponseText(List<ConversationResponse> conversationTextConversationResponses)
        {
            if (!conversationTextConversationResponses.Any())
            {
                SetComplete();
                return;
            }

            var sortedResponses = conversationTextConversationResponses.OrderBy(con => con.SortOrder).ToList();

            for (var index = 0; index < sortedResponses.Count; index++)
            {
                var conversationResponse = sortedResponses[index];
                SetConversationText($"{index + 1}. {conversationResponse.Response.AddLineBreaks()}");
            }

            SortedResponses = new ReactiveList<ConversationResponse>(sortedResponses);
            CurrentResponses = new ReactiveList<string>(Enumerable.Range(1, sortedResponses.Count).Select(
                i => i.ToString()).Union(new List<string> { QuitCommand }));
        }

        private void SetConversationText(string text)
        {
            ConversationText += text;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetDesignerToDefaultScale()
        {
            try
            {
                Scale = 1.0f;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Scale < 0.05f)
                {
                    return;
                }

                Scale *= 0.95f;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Scale > 3f)
                {
                    return;
                }

                Scale *= 1.05f;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ZoomToFit(object sender, RoutedEventArgs e)
        {
            try
            {
                SetDesignerToDefaultScale();

                var scale = ResizeToFit(ConDesigner, ScrollConversations);

                Scale = scale ?? 0d;

                ScrollConversations.InvalidateScrollInfo();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private double? ResizeToFit(DesignerCanvas canvas, ScrollViewer viewer)
        {
            var scale = 1.0;

            canvas.UpdateLayout();

            if ((canvas.ActualWidth > (viewer.ViewportWidth - MarginSize)) ||
                (canvas.ActualHeight > (viewer.ViewportHeight - MarginSize)))
            {
                var widthScale = (viewer.ViewportWidth - MarginSize) / canvas.ActualWidth;
                var heightScale = (viewer.ViewportHeight - MarginSize) / canvas.ActualHeight;

                scale = Math.Min(widthScale, heightScale);
            }

            return scale;
        }


        private void CmdZoomDefaultClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Scale = 1.0f;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ArrangeAllObjectsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ArrangeObjects(ConDesigner.Children.OfType<DesignerItem>().ToList(), ScrollConversations, ConDesigner);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private Size GetSnapGridSize()
        {
            var pointBrush = ConDesigner.Background as VisualBrush;

            // ReSharper disable once PossibleNullReferenceException
            return new Size(pointBrush.Viewbox.Width, pointBrush.Viewbox.Height);
        }

        private void ArrangeObjects(List<DesignerItem> designerItems, ScrollViewer viewer, DesignerCanvas canvas)
        {
            Scale = 1.0f;

            canvas.UpdateLayout();

            var snapSize = GetSnapGridSize();

            var dimensionWidth = GlobalUiConstants.ConversationTemplateItemWidth;
            var dimensionHeight = GlobalUiConstants.ConversationTemplateItemHeight;
            var xSpacing = snapSize.Width;
            var ySpacing = snapSize.Height;

            var numberOfItemsInRow = (int)((viewer.ViewportWidth - MarginSize) / (dimensionWidth + xSpacing));

            var x = xSpacing;
            var y = ySpacing;

            for (var i = 0; i < designerItems.Count; i++)
            {
                designerItems[i].Width = dimensionWidth;
                designerItems[i].Height = dimensionHeight;

                Canvas.SetLeft(designerItems[i], x);
                Canvas.SetTop(designerItems[i], y);

                x += xSpacing;
                x += dimensionWidth;

                if ((i + 1) % numberOfItemsInRow == 0 && (i + 1) >= numberOfItemsInRow)
                {
                    x = xSpacing;
                    y += (ySpacing);
                    y += (int)dimensionHeight;
                }
            }

            canvas.UpdateLayout();
        }

        public void ShowPropertiesForObject(Guid controlId)
        {
            try
            {
                var adventureObject = GetConversationObject(controlId);

                ShowPropertiesForObject(adventureObject);
            }
            catch (Exception exception)
            {
                TaskDialogService.ShowApplicationError(this, exception);
            }
        }

        public ConversationObjectBase GetConversationObject(Guid controlId)
        {
            return ConversationTree.Conversations.Cast<ConversationObjectBase>().Union(ConversationResponses).Union(ConversationTexts)
                .FirstOrDefault(con => con.ControlId == controlId);
        }

        public AdventureObjectBase GetSelectedObject()
        {
            return ObjectProperties.SelectedObject as AdventureObjectBase;
        }

        public void ShowPropertiesForObject(AdventureObjectBase adventureObject)
        {
            try
            {
                ObjectProperties.SelectedObject = adventureObject;
                SetConversationObjectExplorerTabIndexesToIndex(ConversationObjectExplorerTabIndexes.ObjectExplorer);
            }
            catch (Exception exception)
            {
                TaskDialogService.ShowApplicationError(this, exception);
            }
        }

        private static XElement SerializeDesigner(DesignerCanvas designerCanvas)
        {
            var designerItems = designerCanvas.Children.OfType<DesignerItem>();
            var connections = designerCanvas.Children.OfType<Connection>();

            var element = new XElement(designerCanvas.Name);

            var designerItemsXml = DesignerCanvas.SerializeDesignerItems(designerItems);
            element.Add(designerItemsXml);

            var connectionsXml = DesignerCanvas.SerializeConnections(connections);
            element.Add(connectionsXml);

            return element;
        }

        private XElement GetDesignAsXElement()
        {
            var serializedItems = SerializeDesigner(ConDesigner);
            var root = new XElement("Root");

            root.Add(serializedItems);

            var data = new XElement("DataObjects");

            var conversations = new XElement("Conversations");
            conversations.Add(SerializeToString(ConversationTree.Conversations.ToArray()));
            data.Add(conversations);

            root.Add(data);

            return root;
        }

        private static string SerializeToString<T>(T sourceObject)
        {
            return SerializationExtensions.SerializeToString(sourceObject, new[] { typeof(Npc), typeof(Conversation),
                typeof(ConversationResponse), typeof(ConversationText) });
        }

        private static T DeserializeFromStringHelper<T>(string source) where T : class
        {
            return SerializationExtensions.CreateInstance<T>(source, new[] { typeof(Npc), typeof(Conversation),
                typeof(ConversationResponse), typeof(ConversationText) });
        }

        private static string GetConversationsDirectory()
        {
            return Path.Combine(AdventureGameDesignerViewModel.Instance.ProjectDirectory, $"{AdventureDesigner.Instance.GetProjectSupportDirName()}",
                "Conversations");
        }

        private static string GetConversationsPath(Guid playerGuid)
        {
            return Path.Combine(GetConversationsDirectory(), $"{playerGuid}.xml");
        }

        private async Task SaveFile(XElement xElement)
        {
            try
            {
                SetStatus("Saving Conversations.....", Colors.Green, true);

                var conversationDirectory = GetConversationsDirectory();

                FileHelper.EnsureDirectories(conversationDirectory);

                xElement.Save(GetConversationsPath(CurrentNpc.ControlId));

                CurrentNpc.ConversationTree = ConversationTree;

                await SetUpValidationItems();

                AdventureDesigner.Instance.RefreshObjectIfSelected(CurrentNpc);

                AdventureDesigner.Instance.SaveCurrentProject(false);
            }
            finally
            {
                SetStatus("Save Complete", Colors.Green, false);
            }
        }

        internal void ResetStatus()
        {
            SetStatus(string.Empty, Colors.Transparent, false);
        }

        public void SetStatus(string text, Color color, bool spinnerState)
        {
            if (spinnerState)
            {
                ((Storyboard)FindResource("WaitStoryboard")).Begin();
                WaitSpinner.Visibility = Visibility.Visible;
            }
            else
            {
                ((Storyboard)FindResource("WaitStoryboard")).Stop();
                WaitSpinner.Visibility = Visibility.Collapsed;
            }

            Status.Text = text;
            Status.Foreground = new SolidColorBrush(color);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception exception)
            {
                TaskDialogService.ShowApplicationError(this, exception);
            }
        }

        private void MainScreenTabsOnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == (int)ConversationExplorerTabIndexes.ScriptDesigner)
            {
                ScriptExplorer.SetupExplorer();
            }
        }

        public void ShowScriptDesigner(Script script)
        {
            ScriptExplorer.InitialiseFromScript(script);

            if (MainScreenTabs.SelectedIndex != (int)ConversationExplorerTabIndexes.ScriptDesigner)
            {
                SetMainScreenTab(ConversationExplorerTabIndexes.ScriptDesigner);
            }
            else
            {
                ScriptExplorer.SetupExplorer();
            }
        }

        public void RemoveChildById(Guid sourceId, Guid childId)
        {
            try
            {
                var sourceObject = GetConversationObject(sourceId);
                var childObject = GetConversationObject(childId);
                sourceObject.RemoveChild(childObject);
            }
            catch (Exception exception)
            {
                TaskDialogService.ShowApplicationError(this, exception);
            }
        }

        public void RemoveItemById(Guid sourceId)
        {
            var conversation = ConversationTree.Conversations.FirstOrDefault(con => con.ControlId == sourceId);

            if (conversation != null)
            {
                ConversationTree.Conversations.Remove(conversation);
                return;
            }

            var conversationResponse = ConversationResponses.FirstOrDefault(con => con.ControlId == sourceId);

            if (conversationResponse != null)
            {
                ConversationResponses.Remove(conversationResponse);
                return;
            }

            var conversationText = ConversationTexts.FirstOrDefault(con => con.ControlId == sourceId);

            if (conversationText != null)
            {
                ConversationTexts.Remove(conversationText);
            }
        }

        public void ShowConversationDesigner(DesignerItem itemClicked)
        {
            var conversation = ConversationTree.Conversations.FirstOrDefault(con => con.ControlId == itemClicked.ID);

            CommandIsExecuting = false;

            CurrentResponses = null;

            SelectedConversation = conversation;

            SetMainScreenTab(ConversationExplorerTabIndexes.ConversationExplorer);
        }

        public void SetMainScreenTab(ConversationExplorerTabIndexes tabId)
        {
            MainScreenTabs.SelectedIndex = (int)tabId;
        }

        private async void ValidationGridClick(object sender, MouseButtonEventArgs e)
        {
            var validationItem = (ValidationItemAttribute)ErrorList.SelectedItem;
            var itemFound = validationItem.Action(validationItem.ControlId);

            if (!itemFound)
            {
                await SetUpValidationItems();
            }
            else
            {
                ErrorList.SelectedIndex = -1;
            }
        }

        private void BuildScriptObjects()
        {
            EnsureScriptData();

            foreach (var conversation in ConversationTree.Conversations)
            {
                ScriptContainers.Add(new ScriptContainer { Name = $"{conversation.BaseName} pre-process", Script = conversation.ConversationPreprocessScript, Type = ScriptTypes.ConversationStart });
            }

            foreach (var conversationText in ConversationTexts)
            {
                ScriptContainers.Add(new ScriptContainer { Name = $"{conversationText.GetTextSummary()} pre-process", Script = conversationText.ConversationPreprocessScript, Type = ScriptTypes.ConversationText });
            }

            foreach (var conversationResponses in ConversationResponses)
            {
                ScriptContainers.Add(new ScriptContainer { Name = $"{conversationResponses.GetTextSummary()} pre-process", Script = conversationResponses.ConversationPreprocessScript, Type = ScriptTypes.ConversationResponse });
            }

            MessageBus.Current.SendMessage(ScriptContainers, MessageBusContractConstants.ConversationScriptContract);
        }

        private void EnsureScriptData()
        {
            if (ScriptContainers == null)
            {
                ScriptContainers = new ObservableCollection<ScriptContainer>();
            }

            ScriptContainers.Clear();
        }
        
        private async void ConversationDesignerOnClosing(object sender, CancelEventArgs e)
        {
            if (Options.Instance.AutoSave)
            {
                await SaveFile(GetDesignAsXElement());
                return;
            }

            var result = TaskDialogService.AskQuestion(this, "Are you sure you want to quit without Saving?",
                "Don't show me this message again (switches on Auto Save)",
                new[] { "&Yes", "&No" });

            if (result.VerificationChecked == true)
            {
                Options.Instance.AutoSave = true;
            }

            if (result.CustomButtonResult == 1)
            {
                e.Cancel = true;
            }
        }
    }
}
