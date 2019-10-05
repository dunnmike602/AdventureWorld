using System;
using System.IO;
using System.Reflection;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Extensions
{
    [IgnoreInObjectBrowser]
    public static class AssemblyExtensions
    {
        public static string GetCommandTemplateByName(this Assembly source, string name)
        {
            return source.GetResourceAsString($"DiagramDesigner.Controls.Resources.Python{name}CommandTemplate.txt");
        }

        public static string GetCommandTemplate(this Assembly source)
        {
            return source.GetResourceAsString("DiagramDesigner.Controls.Resources.PythonCommandTemplate.txt");
        }

        public static string GetCommonCodeTemplate(this Assembly source)
        {
            return source.GetResourceAsString("DiagramDesigner.Controls.Resources.PythonCommonCodeTemplate.txt");
        }

        public static string GetPreprocessCodeTemplate(this Assembly source)
        {
            return source.GetResourceAsString("DiagramDesigner.Controls.Resources.PythonPreprocessTemplate.txt");
        }

        public static string GetConversationCodeTemplate(this Assembly source)
        {
            return source.GetResourceAsString("DiagramDesigner.Controls.Resources.PythonConversationTemplate.txt");
        }
        
        public static string GetControlCodeTemplate(this Assembly source)
        {
            return source.GetResourceAsString("DiagramDesigner.Controls.Resources.PythonGetControlCodeTemplate.txt");
        }

        public static string GetPythonIntellisenseFile(this Assembly source)
        {
            return source.GetResourceAsString("DiagramDesigner.Controls.Resources.ICSharpCode.PythonBinding.Resources.Python.xshd");
        }
        
        public static string GetVersionString(this Assembly source)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            return version.Build + "." + version.Major + "." + version.Minor + "." + version.Revision;
        }

        public static string GetResourceAsString(this Assembly source, string resourceName)
        {
            using (var stream = source.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}