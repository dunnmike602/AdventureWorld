using System;
using System.Threading;
using AdventureLandCore.Extensions;
using AdventureLandCore.Services.CoreApi.Interfaces;
using Alba.CsConsoleFormat;
using MLDComputing.ObjectBrowser.Attributes;
using Console = System.Console;

#pragma warning disable 1591

namespace AdventureLandCore.Services.CoreApi
{
    [IgnoreInObjectBrowser]
    public class CommandConsole : CommandConsoleBase, IConsole
    {
        private int? _bufferWidth;

        public CommandConsole()
        {
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
        }

        public int ScreenWidth
        {
            get => _bufferWidth ?? Console.BufferWidth;
            set => _bufferWidth = value;
        }

        public int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        public int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public string ConsoleString { get; set; }

        public string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }

        public void CenteredWrite(string value, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null)
        {
            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            FormattedWrite(value, foregroundColor, backgroundColor);
        }

        public void TurnLoggingOff()
        {
            if (!string.IsNullOrWhiteSpace(LogPathName))
            {
                FormattedWrite("Logging is now switched off.");
            }

            LogPathName = string.Empty;
        }

        public int? Wait(int milliSeconds, bool cancelOnKeyPress = false)
        {
            if (!cancelOnKeyPress)
            {
                Thread.Sleep(milliSeconds);

                return null;
            }
            else
            {
                return CommandConsoleHelper.ReadKeyWithTimeOut(milliSeconds);
            }
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

        public void WriteBanner(string text, ConsoleColor? foregroundColor)
        {
            LogText(text.AddLineBreaks());

            Colorful.Console.WriteAscii(text, MapToDrawingColor(foregroundColor ?? ForegroundColor));
        }

        public void FormattedWrite(string value, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            LogText(value.AddLineBreaks());

            ConsoleString = value;

            var doc = new Document
            {
                Color = foregroundColor,
                Background = backgroundColor
            };

            doc.Children.Add(value);

            ConsoleRenderer.RenderDocument(doc);
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

        public void DrawLine(ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Write(new string('─', ScreenWidth), foregroundColor, backgroundColor);
        }

        public void WriteLine(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            LogText(value.AddLineBreaks());

            var oldforeColor = InitColors(foregroundColor, backgroundColor, out var oldbackColor);

            try
            {
                ConsoleString = value;
                Console.WriteLine(value);
            }
            finally
            {
                ResetColors(oldforeColor, oldbackColor);
            }
        }

        public void Write(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            LogText(value);

            var oldforeColor = InitColors(foregroundColor, backgroundColor, out var oldbackColor);

            try
            {
                ConsoleString = value;
                Console.Write(value);
            }
            finally
            {
                ResetColors(oldforeColor, oldbackColor);
            }
        }

        public string ReadLine(string prompt)
        {
            LogText(prompt);

            Console.Write(prompt);

            var text = Console.ReadLine();

            LogText(text.AddLineBreaks());

            return text;
        }

        public void Beep()
        {
            Console.Beep();
        }

        public void WriteBanner(object value, ConsoleColor? foregroundColor = null)
        {
            WriteBanner(value.ToString(), foregroundColor);
        }

        public void ClearScreen()
        {
            Console.Clear();
        }

        public void HideCursor()
        {
            Console.CursorVisible = false;
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
            Console.CursorVisible = true;
        }

        public string ReadAlphaNumericString(int count,  char? placeHolder = null)
        {
            if (placeHolder.HasValue)
            {
                var originalPosition = Console.CursorLeft;
                Console.Write(new string(placeHolder.Value, count));
                Console.CursorLeft = originalPosition;
            }

            var characters = string.Empty;

            for (var i = 0; i < count; i++)
            {
                char nextChar;

                do
                {
                    nextChar = Console.ReadKey().KeyChar;

                } while (!char.IsLetterOrDigit(nextChar));

                characters += nextChar;
            }

            return characters;
        }
    }
}