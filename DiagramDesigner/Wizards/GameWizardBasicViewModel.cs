using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using AdventureLandCore.Domain;
using DiagramDesigner.Controls;
using DiagramDesigner.Wizards.Attributes;
using ReactiveUI;

namespace DiagramDesigner.Wizards
{
    public class GameWizardBasicViewModel : ValidatableViewModelBase
    {
        private readonly string[] _titles = new[] { "Lost In the Jungle", "The Temple of Fear", "Space Rescue", "The Caverns of Hoplessness", "Murder in the Abbey" };

        private readonly string[] _introductions = new[] { "Coughing and spluttering you stagger out of the wreck of your two seater plane. Stranded and alone, how will you ever find your way out of the steaming, impenetrable jungle."
            , "Dr Idaho Smith, archeologist and adventurer!! You are standing outside the fabled lost temple of Coricancha. It has been your life's ambition to find it. Now how are you going to get in",
            "Space Station Zero Alpha, standing in the control room the destruct sequence has started, you have 60 minutes to get to the space shuttle and escape.",
            "How you got in to these caves is not important, how you are going to get out is. It is pitch black.", "Inspector Jack Dextor of Scotland Yard. This could be the strangest case of your career! The Abbott has been found dead in a locked room, did he kill himself, or was there more sinister forces at work. Your investigations are only just beginning." };

        private readonly string[] _lostMessages = new[]
        {
            "Oh dear the jungle has claimed another victim!!", "Dr Smith, your powers are weak, the treasure is mine.", "BOOM!!!", "Oh dear you are stuck here forever and ever....",
            "Bad Luck Inspector this case will be your greatest failure."
        };

        private readonly string[] _wonMessages = new[]
        {
            "Thats it you can see civilisation, you are saved.", "The door opens with a click, the treasure is yours.",  "Just in time, the shuttle separates from the station. Safe at last.",
            "You have escaped the caverns, many before have not.", "The perpertrator is unmasked. Well done Inspector."
        };

        private readonly string[] _gameDescriptions = new[]
        {
            "Trapped in the African jungle after a plane crash. Can you use all your ingenuity to find an escape route.",
            "A famous archeologist has discovered a hidden temple. Can he find the treasure inside.",
            "Trapped in a space station can you escape before it explodes",
            "Trapped in a labrinth of caves and tunnels you must use your wits to escape.",
            "A murder mystery is afoot in the Abbey, can you solve it or will you be totally confused."
        };

        private string _title;
        private string _introduction;
        private string _playerLostMessage;
        private string _wonGameMessage;
        private string _anotherGameText;
        private string _commandPromptText;
        private string _anotherGameYesResponse;
        private string _gameName;
        private string _gameDescription;
        private int _maximumScore = 1;
        private int _maximumInventorySize = 1;
        private bool _basicInfoIsValid;
        private bool _enablePlayerLost;
        private bool _enableTitles;
        private bool _enableScore;

        public ReactiveCommand PopulateAllFields { get; set; }
        public ReactiveCommand PopulateAllFieldsWithLorem { get; set; }
        public ReactiveCommand PopulateTitle { get; set; }
        public ReactiveCommand PopulateTitleWithLorem { get; set; }
        public ReactiveCommand PopulateIntroduction { get; set; }
        public ReactiveCommand PopulateIntroductionWithLorem { get; set; }
        public ReactiveCommand OpenTextEditorForIntroduction { get; set; }
        public ReactiveCommand PopulatePlayerLost { get; set; }
        public ReactiveCommand PopulatePlayerLostWithLorem { get; set; }
        public ReactiveCommand OpenTextEditorForPlayerLost { get; set; }
        public ReactiveCommand PopulateWonMessage { get; set; }
        public ReactiveCommand PopulateWonMessageWithLorem { get; set; }
        public ReactiveCommand OpenTextEditorForWonMessage { get; set; }
        public ReactiveCommand PopulateAnotherGame { get; set; }
        public ReactiveCommand PopulateAnotherGameWithLorem { get; set; }
        public ReactiveCommand OpenTextEditorForAnotherGame { get; set; }
        public ReactiveCommand PopulateAnotherGameYesWithLorem { get; set; }
        public ReactiveCommand PopulateAnotherGameYes { get; set; }
        public ReactiveCommand PopulateCommandPromptWithLorem { get; set; }
        public ReactiveCommand PopulateCommandPrompt { get; set; }
        public ReactiveCommand PopulateGameNameWithLorem { get; set; }
        public ReactiveCommand PopulateGameName { get; set; }
        public ReactiveCommand PopulateGameDescription { get; set; }
        public ReactiveCommand PopulateGameDescriptionWithLorem { get; set; }
        public ReactiveCommand OpenTextEditorForGameDescription { get; set; }

        public GameWizardBasicViewModel()
        {
            PopulateAllFields = ReactiveCommand.Create(PopulateAllFieldsHandler);
            PopulateAllFieldsWithLorem = ReactiveCommand.Create(PopulateAllFieldsWithLoremHandler);
            PopulateTitle = ReactiveCommand.Create(PopulateTitleHandler);
            PopulateTitleWithLorem = ReactiveCommand.Create(PopulateTitleWithLoremHandler);
            PopulateIntroduction = ReactiveCommand.Create(PopulateIntroductionHandler);
            PopulateIntroductionWithLorem = ReactiveCommand.Create(PopulateIntroductionWithLoremHandler);
            OpenTextEditorForIntroduction = ReactiveCommand.Create(OpenTextEditorForIntroductionHandler);
            PopulatePlayerLost = ReactiveCommand.Create(PopulatePlayerLostHandler);
            PopulatePlayerLostWithLorem = ReactiveCommand.Create(PopulatePlayerLostWithLoremHandler);
            OpenTextEditorForPlayerLost = ReactiveCommand.Create(OpenTextEditorForPlayerLostHandler);
            PopulateWonMessage = ReactiveCommand.Create(PopulateWonMessageHandler);
            PopulateWonMessageWithLorem = ReactiveCommand.Create(PopulateWonMessageWithLoremHandler);
            OpenTextEditorForWonMessage = ReactiveCommand.Create(OpenTextEditorForWonMessageHandler);
            PopulateAnotherGame = ReactiveCommand.Create(PopulateAnotherGameHandler);
            PopulateAnotherGameWithLorem = ReactiveCommand.Create(PopulateAnotherGameWithLoremHandler);
            OpenTextEditorForAnotherGame = ReactiveCommand.Create(OpenTextEditorForAnotherGameHandler);
            PopulateAnotherGameYesWithLorem = ReactiveCommand.Create(PopulateAnotherGameYesWithLoremHandler);
            PopulateAnotherGameYes = ReactiveCommand.Create(PopulateAnotherGameYesHandler);
            PopulateCommandPromptWithLorem = ReactiveCommand.Create(PopulateCommandPromptWithLoremHandler);
            PopulateCommandPrompt = ReactiveCommand.Create(PopulateCommandPromptHandler);
            PopulateGameNameWithLorem = ReactiveCommand.Create(PopulateGameNameWithLoremHandler);
            PopulateGameName = ReactiveCommand.Create(PopulateGameNameHandler);
            PopulateGameDescription = ReactiveCommand.Create(PopulateGameDescriptionHandler);
            PopulateGameDescriptionWithLorem = ReactiveCommand.Create(PopulateGameDescriptionWithLoremHandler);
            OpenTextEditorForGameDescription = ReactiveCommand.Create(OpenTextEditorForGameDescriptionHandler);

            Disposables.Add(Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => PropertyChanged += h,
                    h => PropertyChanged -= h
                )
                .Subscribe(_ => CheckScreenValid()));
        }

        private void CheckScreenValid()
        {
            BasicInfoIsValid = !HasErrors;
        }

        public bool BasicInfoIsValid
        {
            get => _basicInfoIsValid;
            set => this.RaiseAndSetIfChanged(ref _basicInfoIsValid, value);
        }
        
        [RequiredIf(nameof(EnableTitles), ErrorMessage = "You must specify a Title for your game.")]
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        [RequiredIf(nameof(EnableTitles), ErrorMessage = "You must specify an Introduction for your game.")]
        public string Introduction
        {
            get => _introduction;
            set => this.RaiseAndSetIfChanged(ref _introduction, value);
        }

        [RequiredIf(nameof(EnablePlayerLost),ErrorMessage = "You must specify a Player Lost Message for your game.")]
        public string PlayerLostMessage
        {
            get => _playerLostMessage;
            set => this.RaiseAndSetIfChanged(ref _playerLostMessage, value);
        }

        [RequiredIf(nameof(EnableScore), ErrorMessage = "You must specify a Won Game Message for your game.")]
        public string WonGameMessage
        {
            get => _wonGameMessage;
            set => this.RaiseAndSetIfChanged(ref _wonGameMessage, value);
        }

        [Required(ErrorMessage = "You must specify a Another Game Text for your game.")]
        public string AnotherGameText
        {
            get => _anotherGameText;
            set => this.RaiseAndSetIfChanged(ref _anotherGameText, value);
        }

        [Required(ErrorMessage = "You must specify a Command Prompt Text for your game.")]
        public string CommandPromptText
        {
            get => _commandPromptText;
            set => this.RaiseAndSetIfChanged(ref _commandPromptText, value);
        }

        [Required(ErrorMessage = "You must specify a Another Game Yes Response for your game.")]
        public string AnotherGameYesResponse
        {
            get => _anotherGameYesResponse;
            set => this.RaiseAndSetIfChanged(ref _anotherGameYesResponse, value);
        }
        
        [Required(ErrorMessage = "You must specify a Name for your game.")]
        public string GameName
        {
            get => _gameName;
            set => this.RaiseAndSetIfChanged(ref _gameName, value);
        }

        [Required(ErrorMessage = "You must specify a Description for your game.")]
        public string GameDescription
        {
            get => _gameDescription;
            set => this.RaiseAndSetIfChanged(ref _gameDescription, value);
        }

        public int MaximumScore
        {
            get => _maximumScore;
            set => this.RaiseAndSetIfChanged(ref _maximumScore, value);
        }

        public int MaximumInventorySize
        {
            get => _maximumInventorySize;
            set => this.RaiseAndSetIfChanged(ref _maximumInventorySize, value);
        }

        public bool EnablePlayerLost
        {
            get => _enablePlayerLost;
            set
            {
                _enablePlayerLost = value;
                Validate();
                CheckScreenValid();
            }
        }

        public bool EnableTitles
        {
            get => _enableTitles;
            set
            {
                _enableTitles = value;
                Validate();
                CheckScreenValid();
            }
        }

        public bool EnableScore
        {
            get => _enableScore;
            set
            {
                _enableScore = value;
                Validate();
                CheckScreenValid();
            }
        }

        public void PopulateTitleHandler()
        {
            Title = _titles[NextIndex];

            IncrementIndex();
        }

        public void PopulateTitleWithLoremHandler()
        {
            Title = Faker.Lorem.Sentence();
        }

        public void PopulateIntroductionWithLoremHandler()
        {
            Introduction = Faker.Lorem.Paragraph();
        }

        public void PopulatePlayerLostWithLoremHandler()
        {
            PlayerLostMessage = Faker.Lorem.Sentence();
        }

        private void OpenTextEditorForIntroductionHandler()
        {
            Introduction = OpenEditorForItem(Introduction);
        }

        private void OpenTextEditorForPlayerLostHandler()
        {
            PlayerLostMessage = OpenEditorForItem(PlayerLostMessage);
        }

        private void OpenTextEditorForWonMessageHandler()
        {
            WonGameMessage = OpenEditorForItem(WonGameMessage);
        }

        private void OpenTextEditorForAnotherGameHandler()
        {
            AnotherGameText = OpenEditorForItem(AnotherGameText);
        }

        private void PopulateAnotherGameWithLoremHandler()
        {
            AnotherGameText = Faker.Lorem.Sentence();
        }

        private void PopulateAnotherGameHandler()
        {
            AnotherGameText = "Would you like another game?";
        }

        private void PopulateWonMessageWithLoremHandler()
        {
            WonGameMessage = Faker.Lorem.Sentence();
        }

        private void PopulateWonMessageHandler()
        {
            WonGameMessage = _wonMessages[NextIndex];

            IncrementIndex();
        }

        private void PopulateAnotherGameYesHandler()
        {
            AnotherGameYesResponse = "Y";
        }

        private void PopulateCommandPromptHandler()
        {
            CommandPromptText = GlobalConstants.DefaultPrompt;
        }

        private void PopulateCommandPromptWithLoremHandler()
        {
            CommandPromptText = Faker.Lorem.Words(1).First();
        }

        private void PopulateGameNameHandler()
        {
            GameName = _titles[NextIndex];

            IncrementIndex();
        }

        private void PopulateGameNameWithLoremHandler()
        {
            GameName = Faker.Lorem.Sentence();
        }

        private void PopulateAnotherGameYesWithLoremHandler()
        {
            AnotherGameYesResponse = Faker.Lorem.Words(1).First();
        }

        private string OpenEditorForItem(string source)
        {
            var editor = new RichTextEditor(source)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            editor.ShowDialog();

            return editor.Text;
        }

        public void PopulateIntroductionHandler()
        {
            Introduction = _titles[NextIndex];

            IncrementIndex();
        }

        public void PopulatePlayerLostHandler()
        {
            PlayerLostMessage = _lostMessages[NextIndex];

            IncrementIndex();
        }

        private void OpenTextEditorForGameDescriptionHandler()
        {
            GameDescription = OpenEditorForItem(GameDescription);
        }

        private void PopulateGameDescriptionWithLoremHandler()
        {
            GameDescription = Faker.Lorem.Sentence();
        }

        private void PopulateGameDescriptionHandler()
        {
            GameDescription = _gameDescriptions[NextIndex];

            IncrementIndex();
        }

        public void PopulateAllFieldsWithLoremHandler()
        {
            Title = Faker.Lorem.Sentence();
            Introduction = Faker.Lorem.Paragraph();
            PlayerLostMessage = Faker.Lorem.Sentence();
            WonGameMessage = Faker.Lorem.Sentence();
            AnotherGameYesResponse = Faker.Lorem.Words(1).First();
            AnotherGameText = Faker.Lorem.Sentence();
            GameDescription = Faker.Lorem.Sentence();
            GameName = Faker.Lorem.Sentence();
            CommandPromptText = Faker.Lorem.Words(1).First();
        }

        public void PopulateAllFieldsHandler()
        {
            Title = _titles[NextIndex];
            Introduction = _introductions[NextIndex];
            PlayerLostMessage = _lostMessages[NextIndex];
            WonGameMessage = _wonMessages[NextIndex];
            AnotherGameText = "Would you like another game?";
            GameDescription = _gameDescriptions[NextIndex];
            GameName = _titles[NextIndex];
            CommandPromptText = GlobalConstants.DefaultPrompt;
            AnotherGameYesResponse = "Y";

            IncrementIndex();
        }

    }
}