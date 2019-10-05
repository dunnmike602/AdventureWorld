using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using AdventureLandCore.Services.CoreApi;
using AdventureLandCore.Services.CoreApi.Interfaces;
using RichTextBox = Xceed.Wpf.Toolkit.RichTextBox;

namespace AdventureLandWpf.AdventureLandWpf.Consoles
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
            if (_cancelOnKeyPress)
            {
                _waitEvent.Set();
                _lastKeyPressed = (int?)e.SystemKey;
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
                    _console.Dispatcher.Invoke(() =>
                    {
                        Title = value;
                    });
                }
            }
        }

        public void CenteredWrite(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            void CenteredWriteHelper()
            {
                _paragraph = CreateNewParagraph(TextAlignment.Center);
                Write(value, foregroundColor, backgroundColor);
                _console.Document.Blocks.Add(_paragraph);
                _paragraph = CreateNewParagraph();
                _console.Document.Blocks.Add(_paragraph);
            }

            if (_console.Dispatcher.CheckAccess())
            {
                CenteredWriteHelper();
            }
            else
            {
                _console.Dispatcher.Invoke(CenteredWriteHelper);
            }
        }

        public int? Wait(int milliSeconds, bool cancelOnKeyPress = false)
        {
            _cancelOnKeyPress = cancelOnKeyPress;

            try
            {
                _input.KeyDown += HandleKeyPress;

                _waitEvent.Reset();

                Task.Run(() => { _waitEvent.WaitOne(milliSeconds); }).Wait();
            }
            finally
            {
                _input.KeyDown -= HandleKeyPress;
            }

            return _lastKeyPressed;
        }

        public void CenteredWrite(object value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            CenteredWrite(value.ToString(), foregroundColor, backgroundColor);
        }

        public void FormattedWrite(object value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            FormattedWrite(value.ToString(), foregroundColor, backgroundColor);
        }

        public void WriteBanner(object value, ConsoleColor? foregroundColor = null)
        {
            WriteBanner(value.ToString(), foregroundColor);
        }

        public void WriteBanner(string text, ConsoleColor? foregroundColor)
        {
            void WriteBannerHelper()
            {
                _paragraph = CreateNewParagraph(TextAlignment.Center);
                _paragraph.FontSize = 40;
                Write(text, foregroundColor);
                _console.Document.Blocks.Add(_paragraph);

                _paragraph = CreateNewParagraph();
                _console.Document.Blocks.Add(_paragraph);
            }

            if (_console.Dispatcher.CheckAccess())
            {
                WriteBannerHelper();
            }
            else
            {
                _console.Dispatcher.Invoke(WriteBannerHelper);
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
            var oldforeColor = InitColors(foregroundColor, backgroundColor, out var oldbackColor);

            try
            {
                Write(value, foregroundColor, backgroundColor);

                AddNewLine();
            }
            finally
            {
                ResetColors(oldforeColor, oldbackColor);
            }
        }

        private void AddNewLine()
        {
            void AddNewLineHelper()
            {
                AddToParagraph(new Run(Environment.NewLine));
            }

            if (_console.Dispatcher.CheckAccess())
            {
                AddNewLineHelper();
            }
            else
            {
                _console.Dispatcher.Invoke(AddNewLineHelper);
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
                run.Foreground = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
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
                    _console.Dispatcher.Invoke(WriteHelper);
                }
            }
            finally
            {
                ResetColors(oldforeColor, oldbackColor);
            }
        }

        private void AddToParagraph(Run run)
        {
            void AddToParagraphHelper()
            {
                _paragraph.Inlines.Add(run);
            }

            if (_console.Dispatcher.CheckAccess())
            {
                AddToParagraphHelper();
            }
            else
            {
                _console.Dispatcher.Invoke(AddToParagraphHelper);
            }
        }

        public string ReadLine(string prompt = "")
        {
            string ReadLineHelper()
            {
                _prompt.Text = prompt;
                var text = _input.Text;
                _input.Text = string.Empty;
                return text;
            }

            try
            {
                _inputWaitEvent.Reset();

                _console.Dispatcher.Invoke(() =>_console.ScrollToEnd());

                _input.KeyDown += HandleKeyPress;

                Task.Run(() =>
                {
                    _inputWaitEvent.WaitOne();

                }).Wait();
            }
            finally
            {
                _input.KeyDown -= HandleKeyPress;
            }
            
            return _console.Dispatcher.CheckAccess() ? ReadLineHelper(): _console.Dispatcher.Invoke(ReadLineHelper);
        }

        public void Beep()
        {
            throw new NotImplementedException();
        }

        public void ClearScreen()
        {
            void ClearScreenHelper()
            {
                _console.Document.Blocks.Clear();
                _paragraph = CreateNewParagraph();
                _console.Document.Blocks.Add(_paragraph);
            }

            if (_console.Dispatcher.CheckAccess())
            {
                ClearScreenHelper();
            }
            else
            {
                _console.Dispatcher.Invoke(ClearScreenHelper);
            }
        }

        private Paragraph CreateNewParagraph(TextAlignment textAlignment = TextAlignment.Justify)
        {
            var paragraph = new Paragraph
            {
                TextAlignment = textAlignment,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
            };
          
            return paragraph;
        }

        public void HideCursor()
        {
           throw new NotImplementedException();
        }

        public void ShowCursor()
        {
            throw new NotImplementedException();
        }
    }
}