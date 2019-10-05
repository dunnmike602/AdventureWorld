using System;
using System.Threading.Tasks;
using DiagramDesigner.Scripting.Interfaces;

namespace DiagramDesigner.Scripting
{
    public abstract class ScriptHandlerBase : IScriptHandler
    {
        private static readonly IScriptHandler Executor = new PythonHandler();

        public abstract void Init(ICSharpCode.AvalonEdit.TextEditor codeEditor);
        
        public abstract Task<Tuple<string, object>> Compile(string code);

        protected virtual void Dispose(bool disposing)
        {
        }

        public static IScriptHandler GetHandler()
        {
            return Executor;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}