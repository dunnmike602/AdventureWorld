using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AdventureLandCore.Properties;

namespace AdventureLandCore.Domain
{
    public class Script : INotifyPropertyChanged
    {
        public const string NoErrorsText = "No Errors";

        private string _scriptImage = "/Resources/Images/python.png";
        private string _scriptStateImage;
        private object _compiledSource;
        private string _compileErrors;
        private string _scriptStateText;
        private string _source = string.Empty;

        public Script()
        {
            SetScriptStateImage();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        public string Source
        {
            get => _source;
            set
            {
                _source = value;
                SetScriptStateImage();
                OnPropertyChanged();
            }
        }

        [IgnoreDataMember]
        public string ScriptImage
        {
            get => _scriptImage;
            set
            {
                _scriptImage = value;
                OnPropertyChanged();
            }
        }

        [IgnoreDataMember]
        public string ScriptStateImage
        {
            get => _scriptStateImage;
            set
            {
                _scriptStateImage = value;
                OnPropertyChanged();
            }
        }

        [IgnoreDataMember]
        public string ScriptStateText
        {
            get => _scriptStateText;
            set
            {
                _scriptStateText = value;
                OnPropertyChanged();
            }
        }

        [IgnoreDataMember]
        public object CompiledSource
        {
            get => _compiledSource;
            set
            {
                _compiledSource = value;
                SetScriptStateImage();
                OnPropertyChanged();
            }
        }

        [IgnoreDataMember]
        public string CompileErrors
        {
            get => _compileErrors;
            set
            {
                _compileErrors = value;
                SetScriptStateImage();
                OnPropertyChanged();
            }
        }

        public string GetCompileErrors()
        {
            return string.IsNullOrWhiteSpace(CompileErrors) ? NoErrorsText : CompileErrors;
        }

        public bool HasCompileErrors => !string.IsNullOrWhiteSpace(CompileErrors);
        
        private void SetScriptStateImage()
        {
            if (!HasScriptSource())
            {
                ScriptStateImage = null;
                ScriptStateText = null;
                return;
            }

            if (HasCompileErrors)
            {
                ScriptStateImage = "/Resources/Images/red.png";
                ScriptStateText = "Script is invalid";
                return;
            }

            if (!HasCompileErrors && CompiledSource != null)
            {
                ScriptStateImage = "/Resources/Images/green.png";
                ScriptStateText = "Script is valid";
                return;
            }

            ScriptStateImage = "/Resources/Images/amber.png";
            ScriptStateText = "Script  has not been compiled";
        }

        public bool HasScriptSource()
        {
            return !
                string.IsNullOrWhiteSpace(Source);
        }

        public bool HasCompiledScript()
        {
            return CompiledSource != null;
        }

        public void ClearScript()
        {
            CompileErrors = null;
            CompiledSource = null;
            Source = null;
        }
    }
}