using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using DiagramDesigner.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract]
    [CategoryOrder("Text", 1)]
    [CategoryOrder("Scripts", 2)]
    [CategoryOrder("Flags", 2)]
    [DisplayName("Command Mapping")]
    public class AdventureCommandMapping : IdentifableObjectBase
    {
        private string _verbName;
        private string _aliasList;
        private string _oneWordSubstitutionList;
        private string _helpText;
        private Script _scriptCommand;
        private bool _isBuiltInCommand = true;
        private bool _isEnabled = true;

        public AdventureCommandMapping()
        {
            _scriptCommand = new Script {Source = Assembly.GetExecutingAssembly().GetCommandTemplate()};
        }

        [DataMember, Category("Text"), PropertyOrder(1), DisplayName("Verb Name"),
         Description(
             "Text that the player must type in to cause this command to be invoked in the game. All commands an aliases mut be unique if you are using the game engines default language processor as it is not context sensitive.")]
        public string VerbName
        {
            get => _verbName;
            set
            {
                _verbName = value;
                OnPropertyChanged();
            }
        }

        [DataMember, Category("Text"), PropertyOrder(2), DisplayName("Alias List"),
         Editor(typeof(TextEditor), typeof(TextEditor)),
         Description("List of alternative text that will trigger the same command.")]
        public string AliasList
        {
            get => _aliasList;
            set
            {
                _aliasList = value;
                OnPropertyChanged();
            }
        }

        [DataMember, Category("Text"), PropertyOrder(3), DisplayName("One Word Substitution List"),
         Editor(typeof(TextEditor), typeof(TextEditor)),
         Description(
             "List of single words that can trigger the whole action. For example GO NORTH can be executed simply by typing NORTH.")]
        public string OneWordSubstitutionList
        {
            get => _oneWordSubstitutionList;
            set
            {
                _oneWordSubstitutionList = value;
                OnPropertyChanged();
            }
        }

        [DataMember, Category("Text"), PropertyOrder(4), DisplayName("Help Text"),
         Editor(typeof(TextEditor), typeof(TextEditor)),
         Description(
             "Text that will be shown for this command when the user asks for help. This is an optional feature.")]
        public string HelpText
        {
            get => _helpText;
            set
            {
                _helpText = value;
                OnPropertyChanged();
            }
        }

        [DataMember, Category("Scripts"), PropertyOrder(7), DisplayName("Script"),
         Description(
             "Script that is used to perform the commands action by the game engine. Must have an Execute method defined on it."),
         Editor(typeof(ScriptEditor), typeof(ScriptEditor))]
        public Script ScriptCommand
        {
            get => _scriptCommand;
            set
            {
                _scriptCommand = value;
                OnPropertyChanged();
            }
        }

        [DataMember, Category("Flags"), PropertyOrder(8), ReadOnly(true), DisplayName("Built In Command"),
         Description("This command is recognised by the engine out of the box, it has not been added by the designer.")]
        public bool IsBuiltInCommand
        {
            get => _isBuiltInCommand;
            set
            {
                _isBuiltInCommand = value;
                OnPropertyChanged();
            }
        }

        [DataMember, Category("Flags"), PropertyOrder(9), DisplayName("Enabled"),
         Description("Set to false to prevent the game engine recognising this command.")]
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public IList<string> GetAllNames()
        {
            var substitutionList = new List<string>();

            if (!string.IsNullOrWhiteSpace(OneWordSubstitutionList))
            {
                substitutionList = OneWordSubstitutionList.TextToList();
            }

            return AliasList.TextToList().Concat(new List<string>{VerbName}).Concat(substitutionList).ToList();
        }

        public void CopyFrom(AdventureCommandMapping sourceMapping)
        {
            IsBuiltInCommand = sourceMapping.IsBuiltInCommand;
            IsEnabled = sourceMapping.IsEnabled;
            ScriptCommand = sourceMapping.ScriptCommand;
            HelpText = sourceMapping.HelpText;
            VerbName = sourceMapping.VerbName;
            AliasList = sourceMapping.AliasList;
            OneWordSubstitutionList = sourceMapping.OneWordSubstitutionList;
        }
    }
}