using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AdventureLandCore.Domain;
using AdventureLandCore.Services;
using SharedControls;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace DiagramDesigner.Controls
{
    public partial class CompassWindow
    {
        private readonly PropertyItem _item;

        public CompassWindow()
        {
            InitializeComponent();
        }

        public CompassWindow(PropertyItem item) : this()
        {
            _item = item;

            PopulateScreen(_item.Value as Dictionary<int, Direction>);
        }

        private void PopulateScreen(Dictionary<int, Direction> directions)
        {
            if (directions == null)
            {
                directions = LanguageHelper.GetDirections();
            }

            foreach (var direction in directions)
            {
                var directionAbbreviationControl = (TextBox)FindName($"Key{direction.Key}");

                if (directionAbbreviationControl != null)
                {
                    directionAbbreviationControl.Text = direction.Value.Abbreviation;
                }

                var directionTextControl = (TextBox)FindName($"Value{direction.Key}");

                if (directionTextControl != null)
                {
                    directionTextControl.Text = direction.Value.Text;
                }
            }
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            var newDirections = PopulateDirections();

            var validationMessage = ValidateDirections(newDirections);

            if (string.IsNullOrWhiteSpace(validationMessage))
            {
                _item.Value = newDirections;
                Close();
            }
            else
            {
                TaskDialogService.ShowValidationError(this, validationMessage);
            }
        }

        private string ValidateDirections(Dictionary<int, Direction> newDirections)
        {
            if (newDirections.Count == 0)
            {
                return "Every game requires at least one compass direction.";
            }

            if (newDirections.GroupBy(i => i.Value.Text.ToUpper()).Any(g => g.Count() > 1) ||
                newDirections.GroupBy(i => i.Value.Abbreviation.ToUpper()).Any(g => g.Count() > 1))
            {
                return "You can't have duplicate directions or direction abbreviations.";
            }

            return string.Empty;
        }

        private Dictionary<int, Direction> PopulateDirections()
        {
            var directions = new Dictionary<int, Direction>();

            for (var i = 0; i <= 10; i++)
            {
                var directionAbbreviation = ((TextBox)FindName($"Key{i}"))?.Text.ToUpper();
                var directionText = ((TextBox)FindName($"Value{i}"))?.Text.ToUpper();

                if (!string.IsNullOrWhiteSpace(directionAbbreviation) || !string.IsNullOrWhiteSpace(directionText))
                {
                    directions.Add(i, new Direction
                    {
                        Abbreviation = string.IsNullOrWhiteSpace(directionAbbreviation) ? directionText: directionAbbreviation,
                        Text = string.IsNullOrWhiteSpace(directionText) ? directionAbbreviation : directionText
                    });
                }
            }

            return directions;
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i <= 10; i++)
            {
                var directionAbbreviationControl = (TextBox)FindName($"Key{i}");

                if (directionAbbreviationControl != null)
                {
                    directionAbbreviationControl.Text = string.Empty;
                }

                var directionTextControl = (TextBox)FindName($"Value{i}");

                if (directionTextControl != null)
                {
                    directionTextControl.Text = string.Empty;
                }
            }
        }

        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            PopulateScreen(null);
        }
    }
}
