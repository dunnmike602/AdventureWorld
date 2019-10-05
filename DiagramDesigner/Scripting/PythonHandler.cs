using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Scripting;
using AdventureLandCore.Services.CoreApi;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using CommandConsole = AdventureLandCore.Services.CoreApi.CommandConsole;

namespace DiagramDesigner.Scripting
{
    public class PythonHandler : ScriptHandlerBase
    {
        private bool _disposed;

        private ScriptEngine _engine;
        private ScriptScope _scope;
        
        public override void Init(ICSharpCode.AvalonEdit.TextEditor codeEditor)
        {
            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();

            PythonHelper.GetStandardOptions(_engine.Runtime);

            if (codeEditor != null)
            {
                var syntaxHighlightData = Assembly.GetExecutingAssembly().GetPythonIntellisenseFile();

                codeEditor.SyntaxHighlighting = HighlightingLoader.Load(
                    new XmlTextReader(new StringReader(syntaxHighlightData)),
                    HighlightingManager.Instance);
            }
            
            _scope.SetVariable(GlobalConstants.ScriptApiName, new AdventureApi());
            _scope.SetVariable(GlobalConstants.ConsoleApiName, new CommandConsole());
            _scope.SetVariable(GlobalConstants.LanguageApiName, new LanguageApi(null, null));
        }

        public override async Task<Tuple<string, object>> Compile(string code)
        {
            Tuple<string, object> results;
            ScriptSource script = null;
         
            try
            {
                var returnValue = await Task.Run(() =>
                {
                    script = _engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
                    script.Compile();

                    return string.Empty;
                });

                results = returnValue.Length == 0 ? new Tuple<string, object>(string.Empty, script) :
                    new Tuple<string, object>(string.Join(Environment.NewLine, returnValue), null);
            }
            catch (Exception ex)
            {
                var eo = _engine.GetService<ExceptionOperations>();

                results = new Tuple<string, object>(eo.FormatException(ex), null);
            }

            return results;
        }
    
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _engine?.Runtime.Shutdown();
            }
            
            _disposed = true;

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}