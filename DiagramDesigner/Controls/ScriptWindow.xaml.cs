using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AdventureLandCore.Domain;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.AdventureWorld.GameExecution;
using DiagramDesigner.Scripting;
using DiagramDesigner.Scripting.Interfaces;
using AdventureLandCore.Extensions;
using DiagramDesigner.Extensions;
using DiagramDesigner.Interfaces;
using ICSharpCode.AvalonEdit.Search;
using Microsoft.Scripting.Utils;
using Microsoft.Win32;
using ReactiveUI;
using SharedControls;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;
using PropertyChangedEventHandler = System.ComponentModel.PropertyChangedEventHandler;

namespace DiagramDesigner.Controls
{
    public partial class ScriptWindow : INotifyPropertyChanged
    {
        public RoutedCommand SaveCommand
        {
            get { return (RoutedCommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register("SaveCommand", typeof(RoutedCommand), typeof(ScriptWindow));

        private ScriptContainer _scriptContainer;

        private IScriptHandler _scriptHandler;
        private bool _disableSelection;
        private ApiExplorer _explorer;
        private SearchPanel _searchPanel;

        public ObservableCollection<ScriptContainerHeader> ScriptContainerHeaders { get; } = new ObservableCollection<ScriptContainerHeader>();

        private List<ScriptContainerHeader> _scriptHeaders;

        public Script Script => ScriptContainer?.Script;

        public string UpdateContract { get; set; }

        public ScriptContainer ScriptContainer
        {
            get => _scriptContainer;
            set
            {
                _scriptContainer = value;

                if (_scriptContainer != null)
                {
                    CodeTextEditor.Text = Script.Source;

                    ScriptName.Text = ScriptContainerHeaders.SelectMany(hdrs => hdrs.ScriptContainers)
                        .FirstOrDefault(sc => sc.Script.Id == Script.Id)
                        ?.Name;
                }
            }
        }
        
        public ScriptWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            DataContext = this;

            InitTextEditor();

            _scriptHandler = ScriptHandlerBase.GetHandler();
            _scriptHandler.Init(CodeTextEditor);
        }

        private void InitTextEditor()
        {
            _searchPanel = SearchPanel.Install(CodeTextEditor);
        }
      
        private async void CmdRunClick(object sender, RoutedEventArgs e)
        {
            await RunHelper();
        }

        private async Task RunHelper()
        {
            if (await ContinueWithRun())
            {
                var code = CodeTextEditor.Text;

                if (!string.IsNullOrWhiteSpace(code))
                {
                    ExecutorFactory.ExecuteGame(AdventureGameDesignerViewModel.Instance.Map(), ClientType.ConsoleApplication,
                        new Script { Source = code});
                }
            }
        }

        private async Task<bool> ContinueWithRun()
        {
            if (!Options.Instance.ShowModelValidationDialog)
            {
                return true;
            }

            var isGameValid = await AdventureDesigner.Instance.CheckCurrentGameIsValid();

            if (!isGameValid)
            {
                var parentWindow = Window.GetWindow(this);

                var result = TaskDialogService.AskQuestion(parentWindow,
                    "The current game model has validation errors, this script may not run as you expect it. Do you want to continue?",
                    "Don't show me this message again",
                    new[] {"&Yes", "&No"});

                if (result.VerificationChecked == true)
                {
                    Options.Instance.ShowModelValidationDialog = false;
                }

                if (result.CustomButtonResult == 1)
                {
                    return false;
                }
            }

            return true;
        }

        private void EnableButtonsHelper(bool state)
        {
            CmdRun.IsEnabled = state;
            CmdValidate.IsEnabled = state;
        }

        private async void CmdCompileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var code = CodeTextEditor.Text;

                EnableButtonsHelper(false);

                Errors.Text = string.Empty;

                GetParentWindow().SetStatus("COMPILING.....", Colors.Black, true);

                await CompileCodeHelper(code);

                ScriptTab.SelectedIndex = 1;
            }
            catch (Exception)
            {
                GetParentWindow().SetStatus(string.Empty, Colors.Transparent, false);
                throw;
            }
            finally
            {
                EnableButtonsHelper(true);
            }
        }

        private async Task CompileCodeHelper(string code)
        {
            var result = await _scriptHandler.Compile(code);

            var compileErrors = result.Item1;

            if (string.IsNullOrWhiteSpace(compileErrors) && ScriptContainer.Type == ScriptTypes.Command)
            {
                Script.CompileErrors = null;
                compileErrors = ScriptContainer.Script.ValidateCommandScript();
            }

            if (string.IsNullOrWhiteSpace(compileErrors))
            {
                GetParentWindow().SetStatus("SUCCESSFUL COMPILATION", Colors.Green, false);
                Script.CompileErrors = null;
                Script.CompiledSource = result.Item2;
                
                Errors.Text += Script.NoErrorsText;
            }
            else
            {
                Script.CompileErrors = compileErrors;
                Script.CompiledSource = null;
                Errors.Text += compileErrors.AddLineBreaks();
                GetParentWindow().SetStatus("ERRORS OCCURRED DURING COMPILATION", Colors.Red, false);
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CodeTextEditorOnTextChanged(object sender, EventArgs e)
        {
            if (Script.Source != CodeTextEditor.Text)
            {
                Script.Source = CodeTextEditor.Text;
                Script.CompiledSource = null;
            }
        }

        public void Reset(List<ScriptContainerHeader> scriptHeaders)
        {
            _scriptHeaders = scriptHeaders;

            Errors.Text = string.Empty;
            CodeTextEditor.Text = string.Empty;
            ScriptContainer = null;
            ResetScriptCollections(scriptHeaders);
            ScriptContainer = null;
            SetupExplorer();
        }

        private void ResetScriptCollections(List<ScriptContainerHeader> scriptHeaders)
        {
            if (ScriptContainerHeaders.Count > 0)
            {
                return;
            }

            ScriptContainerHeaders.Clear();

            ScriptContainerHeaders.AddRange(scriptHeaders);
        }

        public void SetupExplorer()
        {
            if (Script == null && ScriptContainerHeaders[0].ScriptContainers.Count > 0)
            {
                var scriptContainer = ScriptContainerHeaders[0].ScriptContainers[0];

                ScriptContainer = scriptContainer;
                ScriptName.Text = scriptContainer.Name;
            }

            GiveCorrectItemFocus();
        }

        private void GiveCorrectItemFocus()
        {
            var selectedScriptContainer = ScriptContainerHeaders.SelectMany(hdrs => hdrs.ScriptContainers)
                .FirstOrDefault(sc => sc.Script.Id == Script.Id);

            if (selectedScriptContainer != null && !selectedScriptContainer.IsSelected)
            {
                _disableSelection = true;
                selectedScriptContainer.IsSelected = true;
            }

            ScriptExplorer.Expand(true);
        }

        private void LoadScripts(ObservableCollection<ScriptContainer> scripts)
        {
            ResetScriptCollections(_scriptHeaders);
          
            foreach (var script in scripts)
            {
                var scriptHeader = ScriptContainerHeaders.FirstOrDefault(sh => sh.Type == script.Type);

                if (scriptHeader == null)
                {
                    continue;
                }

                // Add new scripts
                if (scriptHeader.ScriptContainers.FirstOrDefault(sc => sc.Script.Id == script.Script.Id) == null)
                {
                    scriptHeader.ScriptContainers.Add(script);
                    continue;
                }

                // Update existing scripts
                var existingScript = scriptHeader.ScriptContainers.FirstOrDefault(sc => sc.Script.Id == script.Script.Id);
                if (existingScript != null)
                {
                    existingScript.Name = script.Name;
                }
            }

            var itemsToDelete = ScriptContainerHeaders.SelectMany(hdrs => hdrs.ScriptContainers)
                .Where(item => scripts.All(item2 => item2.Script.Id != item.Script.Id)).ToList();

            // Remove deleted items
            foreach (var scriptContainer in itemsToDelete)
            {
                var container =  ScriptContainerHeaders.FirstOrDefault(sh => sh.Type == scriptContainer.Type);

                container?.ScriptContainers.Remove(scriptContainer);
            }

            // Resort commands
            var commandHeader = ScriptContainerHeaders.FirstOrDefault(sch => sch.Type == ScriptTypes.Command);

            if (commandHeader != null)
            {
                commandHeader.ScriptContainers =  new ObservableCollection<ScriptContainer>(commandHeader.ScriptContainers.OrderBy(sc => sc.Name));
            }
        }

        private async void CmdCompileAllClick(object sender, RoutedEventArgs e)
        {
            var hasErrors = false;

            try
            {
                foreach (var scriptContainer in ScriptContainerHeaders.SelectMany(hdrs => hdrs.ScriptContainers))
                {
                    if (!scriptContainer.Script.HasScriptSource())
                    {
                        continue;
                    }

                    EnableButtonsHelper(false);

                    GetParentWindow().SetStatus($"COMPILING {scriptContainer.Name} .....", Colors.Black, true);

                    scriptContainer.Script.CompileErrors = null;

                    var result = await _scriptHandler.Compile(scriptContainer.Script.Source);

                    var compileErrors = result.Item1;

                    if (scriptContainer.Type == ScriptTypes.Command && string.IsNullOrWhiteSpace(compileErrors))
                    {
                        compileErrors = scriptContainer.Script.ValidateCommandScript();
                    }

                    if (string.IsNullOrWhiteSpace(compileErrors))
                    {
                        scriptContainer.Script.CompileErrors = null;
                        scriptContainer.Script.CompiledSource = result.Item2;
                    }
                    else
                    {
                        hasErrors = true;
                        scriptContainer.Script.CompileErrors = compileErrors;
                        scriptContainer.Script.CompiledSource = null;
                    }
                }
            }
            finally
            {
                if (hasErrors)
                {
                    GetParentWindow().SetStatus("ONE OR MORE COMPILATION FAILURES", Colors.Red, false);
                }
                else
                {
                    GetParentWindow().SetStatus("ALL SCRIPTS COMPILED SUCCESSFULLY", Colors.Green, false);
                }

                EnableButtonsHelper(true);
            }
        }

        private void CmdExpandClick(object sender, RoutedEventArgs e)
        {
            ScriptExplorer.Expand(true);
        }

        private void CmdCollapseClick(object sender, RoutedEventArgs e)
        {
            ScriptExplorer.Expand(false);
        }

        private void ScriptExplorerOnSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_disableSelection)
                {
                    return;
                }

                if (ScriptExplorer.SelectedItem is ScriptContainer scriptContainer)
                {
                    ScriptContainer = scriptContainer;
                    ScriptName.Text = scriptContainer.Name;
                    Errors.Text = Script.GetCompileErrors();
                }
            }
            finally
            {
                _disableSelection = false;
            }
        }

        private void CmdObjectBrowserClick(object sender, RoutedEventArgs e)
        {
            if (_explorer != null)
            {
                if (_explorer.WindowState == WindowState.Minimized)
                {
                    _explorer.WindowState = WindowState.Normal;
                }

                _explorer.Focus();
                return;
            }

            _explorer = new ApiExplorer()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                //Topmost = true,
                Width = SystemParameters.PrimaryScreenWidth * .6,
                Height = SystemParameters.PrimaryScreenHeight * .45,
                ShowInTaskbar = true,
            };

            _explorer.SnippitGenerated += ExplorerSnippitGenerated;
            _explorer.Closed += (s, evt) => _explorer = null;

            _explorer.Show();
        }

        private void ExplorerSnippitGenerated(object sender, MLDComputing.ObjectBrowser.SnippitGeneratedEventHandlerArgs args)
        {
            CodeTextEditor.Document.Insert(CodeTextEditor.TextArea.Caret.Offset, args.Snippit);
        }

        public void InitialiseFromScript(Script script)
        {
            ScriptContainer = ScriptContainerHeaders.SelectMany(hdr => hdr.ScriptContainers)
                .FirstOrDefault(sc => sc.Script.Id == script.Id);
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            var contents = GetFileContents();

            if (contents != null)
            {
                CodeTextEditor.Document.Insert(CodeTextEditor.TextArea.Caret.Offset, contents);
            }
        }

        private string GetFileContents()
        {
            var openFile = new OpenFileDialog
            {
                Filter = "Script files (*.cs;*.py;*.txt)|*.cs;*.py;.txt|All files (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System),
            };

            if (openFile.ShowDialog() == true)
            {
                using (var file = File.OpenText(openFile.FileName))
                {
                    return file.ReadToEnd();
                }
            }
            else
            {
                return null;
            }
        }

        private void SaveFileClick(object sender, RoutedEventArgs e)
        {
            SaveCommand.Execute(null, this);
        }

        private void QuickSearchClick(object sender, RoutedEventArgs e)
        {
            _searchPanel.Open();
            _searchPanel.Reactivate(); 
        }

        ~ScriptWindow()
        {
            _scriptHandler.Dispose();
        }

        private IStatusMessageProvider GetParentWindow()
        {
            return (IStatusMessageProvider) Window.GetWindow(this);
        }

        private void ScriptWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            MessageBus.Current.Listen<ObservableCollection<ScriptContainer>>(UpdateContract).Subscribe(LoadScripts);
        }
    }
}

