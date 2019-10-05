using System;
using System.Threading.Tasks;

namespace DiagramDesigner.Scripting.Interfaces
{
    public interface IScriptHandler : IDisposable
    {
        void Init(ICSharpCode.AvalonEdit.TextEditor codeEditor);

        Task<Tuple<string, object>> Compile(string code);
    }
}