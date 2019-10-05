using System;
using AdventureLandCore.Domain;

namespace AdventureLandCore.Extensions
{
    public static class ScriptExtensions
    {
        public static string ValidateCommandScript(this Script sourceScript)
        {
            if (!sourceScript.HasCompileErrors && sourceScript.Source.IndexOf("def Execute", StringComparison.CurrentCultureIgnoreCase) == -1)
            {
                sourceScript.CompileErrors = @"A command script must have a ""def Execute"" functions defined in it.";
            }

            return sourceScript.CompileErrors;
        }
    }
}