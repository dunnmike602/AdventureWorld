using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using AdventureLandCore.Extensions;
using AdventureLandCore.Services.CoreApi;
using AdventureLandCore.Services.CoreApi.Interfaces;
using RichTextBox = Xceed.Wpf.Toolkit.RichTextBox;

namespace AdventureLandExplorer.AdventureLandWpf.Consoles
{
    public class RichTextConsole : CommandConsoleBase, IConsole
    {
        private readonly RichTextBox _console;
        private readonly TextBox _input;
        private readonly TextBox _prompt;
        private readonly TextBox _title;
        private readonly ManualResetEvent _waitEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _inputWaitEvent = new ManualResetEvent(false);
        private int? _lastKeyPressed;
        private Paragraph _paragraph;
        private bool _cancelOnKeyPress;

        private List<string> _commandHistory = new List<string>();

        private int? _currentHistoryIndex;
     
        public RichTextConsole(TextBox title, RichTextBox richTextBox, TextBox input, TextBox prompt)
        {
            _title = title;
            _console = richTextBox;
            _input = input;
            _prompt = prompt;
            ClearScreen();
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (!_currentHistoryIndex.HasValue && _commandHistory.Count > 0)
                {
                    _currentHistoryIndex = _commandHistory.Count - 1;
                }
                else if (_currentHistoryIndex.HasValue && _commandHistory.Count > 0 && _currentHistoryIndex.Value > 0)
                {
                    _currentHistoryIndex--;
                }

                if (_currentHistoryIndex.HasValue)
                {
                    _input.Text = _commandHistory[_currentHistoryIndex.Value];
                }
            }

            if (e.Key == Key.Down)
            {
                if (_currentHistoryIndex.HasValue && _commandHistory.Count > 0 &&
                    _currentHistoryIndex.Value < _commandHistory.Count - 1)
                {
                    _currentHistoryIndex++;
                }

                if (_currentHistoryIndex.HasValue)
                {
                    _input.Text = _commandHistory[_currentHistoryIndex.Value];
                }
            }

            if (_cancelOnKeyPress)
            {
                _waitEvent.Set();
                _lastKeyPressed = (int?) e.SystemKey;
            }

            if (e.Key == Key.Enter)
            {
                _inputWaitEvent.Set();
            }
        }

        public int ScreenWidth { get; set; }

        public int CursorLeft { get; set; }

        public int CursorTop { get; set; }

        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleColor ForegroundColor { get; set; }

        public string ConsoleString { get; set; }

        public string Title
        {
            get
            {
                return _console.Dispatcher.CheckAccess() ? _title.Text : _console.Dispatcher.Invoke(() => _title.Text);
            }
            set
            {
                if (_console.Dispatcher.CheckAccess())
                {
                    _title.Text = value;
                }
                else
                {
                    Dispatch(() => { Title = value; });
                }
            }
        }

        public void CenteredWrite(string value, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null)
        {
            void CenteredWriteHelper()
            {
                _paragraph = CreateNewParagraph(TextAlignment.Center);

                Write(value, foregroundColor, backgroundColor);
                
                _paragraph = CreateNewParagraph();

            }

            LogText(value.AddLineBreaks());

            if (_console.Dispatcher.CheckAccess())
            {
                CenteredWriteHelper();
            }
            else
            {
                Dispatch(CenteredWriteHelper);
            }
        }

        public void TurnLoggingOff()
        {
            if (!string.IsNullOrWhiteSpace(LogPathName))
            {
                FormattedWrite("Logging is now switched off.");
            }
        }

        public int? Wait(int milliSeconds, bool cancelOnKeyPress = false)
        {
            _cancelOnKeyPress = cancelOnKeyPress;

            try
            {
                _input.PreviewKeyDown += HandleKeyPress;

                _waitEvent.Reset();

                Task.Run(() => { _waitEvent.WaitOne(milliSeconds); }).Wait();
            }
            finally
            {
                _input.PreviewKeyDown -= HandleKeyPress;
            }

            return _lastKeyPressed;
        }

        public void CenteredWrite(object value, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null)
        {
            CenteredWrite(value.ToString(), foregroundColor, backgroundColor);
        }

        public void FormattedWrite(object value, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null)
        {
            FormattedWrite(value.ToString(), foregroundColor, backgroundColor);
        }

        public void WriteBanner(object value, ConsoleColor? foregroundColor = null)
        {
            WriteBanner(value.ToString(), foregroundColor);
        }

        private void Dispatch(Action action)
        {
            _console.Dispatcher.Invoke(DispatcherPriority.Normal, action);
        }

        public string ReadAlphaNumericString(int count, char? placeHolder = null)
        {
            placeHolder = placeHolder ?? ' ';
            
            Dispatch(() =>
            {
                _input.Text = new string(placeHolder.Value, count);
                _input.CaretIndex = 0;
            });
         
            var characters = string.Empty;

            var awaiter = new ManualResetEvent(false);

            void Handler(object s, KeyEventArgs e)
            {
                var charInput = e.Key.GetCharacterFromKey();

                if (!charInput.HasValue || !char.IsLetterOrDigit(charInput.Value) )
                {
                    e.Handled = true;
                    return;
                }

                characters += charInput;

                Dispatch(() =>
                {
                    var chars = _input.Text.ToCharArray();
                    chars[characters.Length - 1] = charInput.Value;
                    _input.Text = new string(chars);
                    _input.CaretIndex = characters.Length;
                });
                
                if (characters.Length == count)
                {
                    _input.PreviewKeyDown -= Handler;

                    Dispatch(() =>
                    {
                        WriteLine(_input.Text);
                        _console.ScrollToEnd();
                        _input.Text = string.Empty;
                    });

                    awaiter.Set();
                }

                e.Handled = true;
            }

            _input.PreviewKeyDown += Handler;

            awaiter.WaitOne();

            return characters;
        }

        public void WriteBanner(string text, ConsoleColor? foregroundColor)
        {
            void WriteBannerHelper()
            {
                _paragraph = CreateNewParagraph(TextAlignment.Center);
                _paragraph.FontSize = 40;
                Write(text, foregroundColor);

                _paragraph = CreateNewParagraph();
            }

            if (_console.Dispatcher.CheckAccess())
            {
                WriteBannerHelper();
            }
            else
            {
                Dispatch(WriteBannerHelper);
            }
        }

        public void FormattedWrite(string value, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            var oldforeColor = InitColors(foregroundColor, backgroundColor, out var oldbackColor);

            try
            {
                ConsoleString = value;

                WriteLine(value, foregroundColor, backgroundColor);
            }
            finally
            {
                ResetColors(oldforeColor, oldbackColor);
            }
        }

        private void ResetColors(ConsoleColor oldforeColor, ConsoleColor oldbackColor)
        {
            ForegroundColor = oldforeColor;
            BackgroundColor = oldbackColor;
        }

        private ConsoleColor InitColors(ConsoleColor? foregroundColor, ConsoleColor? backgroundColor,
            out ConsoleColor oldbackColor)
        {
            var oldforeColor = ForegroundColor;
            oldbackColor = BackgroundColor;

            if (foregroundColor.HasValue)
            {
                ForegroundColor = foregroundColor.Value;
            }

            if (backgroundColor.HasValue)
            {
                BackgroundColor = backgroundColor.Value;
            }

            return oldforeColor;
        }

        public void WriteLine(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            void WriteLineHelper()
            {
                var oldforeColor = InitColors(foregroundColor, backgroundColor, out var oldbackColor);

                try
                {
                    _paragraph = CreateNewParagraph();

                    Write(value, foregroundColor, backgroundColor);
                }
                finally
                {
                    ResetColors(oldforeColor, oldbackColor);
                }
            }

            if (_console.Dispatcher.CheckAccess())
            {
                WriteLineHelper();
            }
            else
            {
                Dispatch(WriteLineHelper);
            }
        }

        private void SetRunColor(Run run, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            if (foregroundColor.HasValue)
            {
                var color = MapToDrawingColor(foregroundColor.Value);
                run.Foreground = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }

            if (backgroundColor.HasValue)
            {
                var color = MapToDrawingColor(backgroundColor.Value);
                run.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
        }

        public void Write(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            void WriteHelper()
            {
                var run = new Run(value);
               
                SetRunColor(run, foregroundColor, backgroundColor);

                AddToParagraph(run);
            }

            LogText(value);

            var oldforeColor = InitColors(foregroundColor, backgroundColor, out var oldbackColor);

            try
            {
                ConsoleString = value;

                if (_console.Dispatcher.CheckAccess())
                {
                    WriteHelper();
                }
                else
                {
                    Dispatch(WriteHelper);
                }
            }
            finally
            {
                ResetColors(oldforeColor, oldbackColor);
            }
        }

        private void AddToParagraph(Inline inline)
        {
            void AddToParagraphHelper()
            {
                _paragraph.Inlines.Add(inline);
            }

            if (_console.Dispatcher.CheckAccess())
            {
                AddToParagraphHelper();
            }
            else
            {
                Dispatch(AddToParagraphHelper);
            }
        }

        public string ReadLine(string prompt = "")
        {
            string ReadLineHelper()
            {
                LogText(prompt.PrefixLineBreaks());

                _prompt.Text = prompt;
                var text = _input.Text;
                _input.Text = string.Empty;
                _commandHistory.Add(text);

                PruneCommandHistory();

                LogText(text.AddLineBreaks());
                return text;
            }

            try
            {
                _inputWaitEvent.Reset();

                Dispatch(() => _console.ScrollToEnd());

                _input.PreviewKeyDown += HandleKeyPress;

                Task.Run(() => { _inputWaitEvent.WaitOne(); }).Wait();
            }
            finally
            {
                _input.PreviewKeyDown -= HandleKeyPress;
            }

            return _console.Dispatcher.CheckAccess() ? ReadLineHelper() : _console.Dispatcher.Invoke(ReadLineHelper);
        }

        private void PruneCommandHistory()
        {
            const int maxItems = 100;

            if (_commandHistory.Count > maxItems)
            {
                _commandHistory = _commandHistory.Skip(_commandHistory.Count - maxItems).Take(maxItems).ToList();
            }
        }

        public void Beep()
        {
            SystemSounds.Beep.Play();
        }

        public void ClearScreen()
        {
            void ClearScreenHelper()
            {
                _console.Document.Blocks.Clear();
                _paragraph = CreateNewParagraph();
            }

            if (_console.Dispatcher.CheckAccess())
            {
                ClearScreenHelper();
            }
            else
            {
                Dispatch(ClearScreenHelper);
            }
        }

        private Paragraph CreateNewParagraph(TextAlignment textAlignment = TextAlignment.Justify, double lineHeight = 20)
        {
            var paragraph = new Paragraph
            {
                TextAlignment = textAlignment,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                LineHeight = lineHeight,
                LineStackingStrategy = double.IsNaN(lineHeight)
                    ? LineStackingStrategy.MaxHeight
                    : LineStackingStrategy.BlockLineHeight,
            };
            
            _console.Document.Blocks.Add(paragraph);

            return paragraph;
        }

        public void DrawLine(ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            void LineHelper()
            {
                var line = new Line
                {
                    Stretch = Stretch.Fill,
                    X2 = 1,
                    VerticalAlignment = VerticalAlignment.Center
                };


                if (foregroundColor.HasValue)
                {
                    var color = MapToDrawingColor(foregroundColor.Value);
                    line.Stroke = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                }

                var lineContainer = new InlineUIContainer(line);
      
                _paragraph = CreateNewParagraph(TextAlignment.Left, 10);
               
                if (backgroundColor.HasValue)
                {
                    var backColor = MapToDrawingColor(backgroundColor.Value);
                    _paragraph.Background = new SolidColorBrush(Color.FromArgb(backColor.A, backColor.R, backColor.G, backColor.B));
                }

                _paragraph.Inlines.Add(lineContainer);

                _paragraph = CreateNewParagraph();
           
            }

            if (_console.Dispatcher.CheckAccess())
            {
                LineHelper();
            }
            else
            {
                Dispatch(LineHelper);
            }
        }

        public void HideCursor()
        {

        }

        public void TurnLoggingOn(string pathName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LogPathName))
                {
                    var fileName = TurnLoggingOnHelper(pathName);

                    FormattedWrite($"Now logging to file {fileName}.");
                }
            }
            catch (Exception ex)
            {
                FormattedWrite("Could not start console logging. Exception".AddLineBreaks(), ConsoleColor.Red);
                FormattedWrite(ex, ConsoleColor.Red);
            }
        }

        public void ShowCursor()
        {

        }
    }
}