using Microsoft.Scripting.Hosting;

namespace AdventureLandCore.Scripting
{
    public static class PythonHelper
    {
        public static void GetStandardOptions(ScriptRuntime scriptRuntime)
        {
            foreach (var assembly in BaseScriptHelper.GetStandardReference())
            {
                scriptRuntime.LoadAssembly(assembly);
            }
        }
    }
}