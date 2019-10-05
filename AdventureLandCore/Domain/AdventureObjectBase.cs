using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Domain
{
    [IgnoreInObjectBrowser]
    [DataContract(IsReference = true)]
    public abstract class AdventureObjectBase : INotifyPropertyChanged
    {
        private string _name;
        private string[] _descriptions;
        private bool _isValid = true;
        private Script _preProcessScript;
        private bool _showRandomDescription;
        private int _currentDescriptionIndex;

        protected static Random RandomGenerator = new Random();
       
        /// <summary>
        /// Unique identifier for the Adventure Game object.
        /// </summary>
        [DataMember]
        [ReadOnly(true)]
        [Description("Unique identifier for the Adventure Game object.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Unique name for the Adventure Game object.
        /// </summary>
        [DataMember]
        [Description("Unique name for the Adventure Game object.")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Adventure object category, for example Room.
        /// </summary>
        [DataMember]
        [Description("Adventure object category, for example Room.")]
        public string Category { get; set; }

        /// <summary>
        /// List of all possible descriptions for the object that can be used depending on state.
        /// </summary>
        [DataMember]
        [Description("List of all possible descriptions for the object that can be used depending on state.")]
        public string[] Descriptions
        {
            get => ReplaceVariables(_descriptions);
            set
            {
                _descriptions = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentDescription));
            }
        }

        /// <summary>
        /// Gets/sets the current description for the object, always defaults to first item in the Descriptions list. 
        /// If ShowRandomDescription is True a random description is picked everytime. Otherwise the CurrentDescriptionIndex is used
        /// to pick this description.  
        /// </summary>
        [Description("Gets/sets the current description for the object, always defaults to first item in the Descriptions list If ShowRandomDescription is " +
                     "True a random description is picked everytime. Otherwise the CurrentDescriptionIndex is used")]
        [IgnoreDataMember]
        public string CurrentDescription => ShowRandomDescription && Descriptions.Length > 0
            ? Descriptions[RandomGenerator.Next(0, Descriptions.Length)]
            : Descriptions[CurrentDescriptionIndex];

        /// <summary>
        /// Gets/sets the current description index. If this is out of range of the Descriptions array an errors is thrown when the description is accessed.
        /// It defaults to 0 which will pick the first description in the list.
        /// </summary>
        [DataMember]
        [Description("Gets/sets the current description index. If this is out of range of the Descriptions array an errors is thrown when the description is accessed. " +
                     "It defaults to 0 which will pick the first description in the list.")]
        public int CurrentDescriptionIndex
        {
            get => _currentDescriptionIndex;
            set
            {
                _currentDescriptionIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentDescription));
            }
        }

        /// <summary>
        /// Script to run before the object is processed by a command.
        /// </summary>
        [DataMember]
        [Description("Script to run before the object is processed by a command.")]
        public Script PreProcessScript
        {
            get => _preProcessScript;
            set
            {
                _preProcessScript = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Flag that can be used by scripts to determine if the object is valid. This allows objects to be created on the fly to represent entities that are typed in as text
        /// but do not exist in the game.
        /// </summary>
        [DataMember]
        [Description("Script to run before the object is processed by a command.")]
        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// If set to true, randomizes the display of the Description property, a new random Description is picked on every display.
        /// </summary>
        [DataMember]
        [Description("If set to true, randomizes the display of the Description property, a new random Description is picked on every display.")]
        public bool ShowRandomDescription
        {
            get => _showRandomDescription;
            set
            {
                _showRandomDescription = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// This is a convenience property, it is used in partial word matches to track the original word that was used. For example if an Object Lamp was matched
        /// with three letters LAM, then this property will contain this.
        /// </summary>
        public string WordThatMatchedThis { get; set; }

        /// <summary>
        /// Default public constructor.
        /// </summary>
        protected AdventureObjectBase()
        {
            Descriptions = new string[0];
        }

        /// <summary>
        /// Base implementation of all Adventure Objects for ToString
        /// </summary>
        /// <returns>The name of the Adventure game object.</returns>
        public override string ToString()
        {
            return Name;
        }

        [IgnoreInObjectBrowser]
#pragma warning disable 1591
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 1591

        [IgnoreInObjectBrowser]
#pragma warning disable 1591
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
#pragma warning restore 1591
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract bool IsContainer { get; }

        public abstract bool IsNpc { get; }

        public abstract bool IsPlaceableObject { get; }
        
        public abstract bool IsRoom { get; }

        public abstract bool IsExit { get; }

        private string[] ReplaceVariables(string[] descriptions)
        {
            if (GameData.GlobalVariables == null)
            {
                return descriptions;
            }

            const string replacementSelector = "##(.*?)##";

            var processedDescriptions = new List<string>();

            foreach (var de in descriptions)
            {
                var regex = new Regex(replacementSelector, RegexOptions.Multiline);

                processedDescriptions.Add(regex.Replace(de, match =>
                {
                    var variableName = match.Value.Replace("#", string.Empty);

                    return GameData.GlobalVariables.ContainsKey(variableName)
                        ? GameData.GlobalVariables[variableName].ToString()
                        : match.Value;
                }));
            }

            return processedDescriptions.ToArray();
        }
    }
}