using System;
using System.Collections.Generic;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Scripting.Interfaces;
using AdventureLandCore.Services.CoreApi.Interfaces;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
#pragma warning disable 1591

namespace AdventureLandCore.Scripting
{
    public class PythonExecutionEngine : IExecutionEngine
    {
        private readonly IAdventureApi _adventureApi;
        private readonly IConsole _console;
        private readonly ILanguageApi _languageApi;
        private readonly bool _debug;
        private ScriptEngine _engine;
        private ScriptScope _scope;

        public PythonExecutionEngine(IAdventureApi adventureApi, IConsole console, ILanguageApi languageApi,
            bool debug = false)
        {
            _adventureApi = adventureApi;
            _console = console;
            _languageApi = languageApi;
            _debug = debug;
            Initialise();
        }

        private void Initialise()
        {
            var options = new Dictionary<string, object>();

            if (_debug)
            {
                options["Debug"] = true;
            }

            _engine = Python.CreateEngine(options);
            _scope = _engine.CreateScope();

            PythonHelper.GetStandardOptions(_engine.Runtime);

            SetupApi();
        }

        private void SetupApi()
        {
            _scope.SetVariable(GlobalConstants.ScriptApiName, _adventureApi);
            _scope.SetVariable(GlobalConstants.ConsoleApiName, _console);
            _scope.SetVariable(GlobalConstants.LanguageApiName, _languageApi);
        }

        private string GetFormattedError(Exception ex, string name)
        {
            var eo = _engine.GetService<ExceptionOperations>();
            var error = eo.FormatException(ex);

            return $"A Script named {name} has failed to execute: ".AddLineBreaks() + error;
        }

        public void Compile(Script script, string name, string globalCode)
        {
            try
            {
                if (script.HasScriptSource())
                {
                    // Always add some libraries
                    var source = "from System import * ".AddLineBreaks() +
                                 "from AdventureLandCore import *  ".AddLineBreaks() +
                                 "from System.Xml import * ".AddLineBreaks();

                    source += globalCode.AddLineBreaks();

                    source += script.Source;

                    var compiledCode = _engine.CreateScriptSourceFromString(source, SourceCodeKind.Statements).Compile();

                    script.CompiledSource = compiledCode;
                    script.CompileErrors = string.Empty;
                }
            }
            catch (Exception ex)
            {
                script.CompileErrors = GetFormattedError(ex, name);
                script.CompiledSource = null;
            }
        }

        public void ExecuteMethod(Script script, string methodName)
        {
            if (script.CompiledSource == null)
            {
                throw new Exception("The supplied script for the method processor has no source.");
            }
            
            try
            {
                var pythonCode = ((CompiledCode)script.CompiledSource);
                pythonCode.Execute(_scope);
                dynamic scope = _scope;

                _engine.Operations.InvokeMember(scope, methodName);
            }
            catch (Exception ex)
            {
                _console.WriteLine(GetFormattedError(ex, "Method Processor"), ConsoleColor.Red);
            }
        }


        public bool ExecuteConversationScript(Script script, Npc npc, ConversationObjectBase conversationObjectBase, ConversationStage conversationStage)
        {
            if (script.CompiledSource == null)
            {
                return true;
            }

            try
            {
                var pythonCode = ((CompiledCode)script.CompiledSource);
                pythonCode.Execute(_scope);
                dynamic scope = _scope;

                return scope.Execute(npc, conversationObjectBase, conversationStage);
            }
            catch (Exception ex)
            {
                _console.WriteLine(GetFormattedError(ex, $"{npc.Name} Preprocess"), ConsoleColor.Red);
                return false;
            }
        }

        public bool ExecuteObjectScript(Script script, AdventureObjectBase adventureObject, ParsedAdventureCommand adventureCommand)
        {
            if (script.CompiledSource == null)
            {
                return true;
            }

            try
            {
                var pythonCode = ((CompiledCode)script.CompiledSource);
                pythonCode.Execute(_scope);
                dynamic scope = _scope;

                return scope.Execute(adventureObject, adventureCommand);
            }
            catch (Exception ex)
            {
                _console.WriteLine(GetFormattedError(ex, $"{adventureObject.Name} Preprocess"), ConsoleColor.Red);
                return false;
            }
        }

        public bool ExecuteLoopScript(Script script, string name)
        {
            if (script.CompiledSource == null)
            {
                return true;
            }

            try
            {
                // Always assume we can continue, the script will have to set this explicitly
                _adventureApi.CanContinue = true;

                ((CompiledCode)script.CompiledSource).Execute(_scope);
            }
            catch (Exception ex)
            {
                _console.WriteLine(GetFormattedError(ex, name), ConsoleColor.Red);
            }

            return _adventureApi.CanContinue;
        }

        public ParsedAdventureCommand ExecuteInputProcessor(Script script, string playerInput)
        {
            if (script.CompiledSource == null)
            {
                throw new Exception("The supplied script for the input processor has no source.");
            }

            try
            {
                var pythonCode = ((CompiledCode)script.CompiledSource);
                pythonCode.Execute(_scope);
                dynamic scope = _scope;
                
                return scope.ProcessInput(playerInput);
            }
            catch (Exception ex)
            {
                _console.WriteLine(GetFormattedError(ex, "Input Processor"), ConsoleColor.Red);
                return null;
            }
        }

        public void MoveAutoFollowNpcs(Script script)
        {
            if (script.CompiledSource == null)
            {
                throw new Exception("The supplied script for the input processor has no source.");
            }

            try
            {
                var pythonCode = ((CompiledCode)script.CompiledSource);
                pythonCode.Execute(_scope);
                dynamic scope = _scope;

                scope.MoveAutoFollowNpcs();
            }
            catch (Exception ex)
            {
                _console.WriteLine(GetFormattedError(ex, "Input Processor"), ConsoleColor.Red);
            }
        }

        public bool ExecuteCommandFunction(Script script, ParsedAdventureCommand adventureCommand)
        {
            if (script.CompiledSource == null)
            {
                throw new Exception($"The {adventureCommand.ParsedCommand.Word} has no source.");
            }

            try
            {
                var pythonCode = ((CompiledCode) script.CompiledSource);
                pythonCode.Execute(_scope);
                dynamic scope = _scope;

                return scope.Execute(adventureCommand);
            }
            catch (Exception ex)
            {
                _console.WriteLine(GetFormattedError(ex, $"{adventureCommand.ParsedCommand.Word}"), ConsoleColor.Red);
                return false;
            }
        }
    }
}