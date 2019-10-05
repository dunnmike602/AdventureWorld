using System;

namespace AdventureLandCore.Services.CoreApi.Interfaces
{
    /// <summary>
    /// This class is used for all interaction with the current Console. Access to this object is via the variable ConsoleApi. This will choose the correct
    /// class depending on which Console is being used, for example Command Console or Windows Store application. Some methods may not be appropriate for some
    /// Consoles, this will be described in the comments and in the Api documentation.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Draws a line across the console screen.
        /// </summary>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        /// <param name="backgroundColor">Background color of the text string (optional).</param>
        void DrawLine(ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null);

        /// <summary>
        /// Hide the cursor. Applies to: Command Console.
        /// </summary>
        void HideCursor();

        /// <summary>
        /// Switches logging of the console to the specified file. If logging is already on this has no effect
        /// </summary>
        /// <param name="pathName">String containing the full file name.</param>
        void TurnLoggingOn(string pathName);

        /// <summary>
        /// Switches off logging  of the console. If logging is already on this has no effect.
        /// </summary>
        void TurnLoggingOff();

        /// <summary>
        /// Waits for the specified amount of time, or optionally until a key is pressed.
        /// </summary>
        /// <param name="milliSeconds">An integer containing the number of milliseconds to wait.</param>
        /// <param name="cancelOnKeyPress">Boolean value if True, pressing any key will cancel the wait. Default value is false</param>
        /// <returns>Null if cancelOnKeyPress is false or no key is pressed in the time period. Otherwise the Key code of the 
        /// key that was pressed. This is converted to a integer as keycodes are different in different consoles. Any script that processes this 
        /// value will have to allow for this.</returns>
        int? Wait(int milliSeconds, bool cancelOnKeyPress = false);

        /// <summary>
        /// Show the cursor. Applies to: Command Console.
        /// </summary>
        void ShowCursor();

        /// <summary>
        /// Specifies the width in characters of the screen. The FormattedWrite methods use this to layout text. Applies to: Command Console.
        /// </summary>
        int ScreenWidth { get; set; }

        /// <summary>
        /// Sets the left value (horizontal) of the cursor. Applies to: Command Console.
        /// </summary>
        int CursorLeft { get; set; }

        /// <summary>
        /// Sets the top value (vertical) of the cursor. Applies to: Command Console.
        /// </summary>
        int CursorTop { get; set; }

        /// <summary>
        /// Sets the console background color. Applies to: Command Console.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Sets the console foreground color. Applies to: Command Console.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Records the last string written to the screen. Applies to: Command Console.
        /// </summary>
        string ConsoleString { get; set; }

        /// <summary>
        /// Specified the title of the screen. Applies to: Command Console.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Writes a text string the screen, wrapped to the value of ScreenWidth. Applies to: Command Console.
        /// </summary>
        /// <param name="value">String contained the text to be written.</param>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        /// <param name="backgroundColor">Background color of the text string (optional).</param>
        void FormattedWrite(object value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null);

        /// <summary>
        /// Writes an object the screen, wrapped to the value of ScreenWidth. ToString() will be called on the object first. Applies to: Command Console.
        /// </summary>
        /// <param name="value">String contained the text to be written.</param>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        /// <param name="backgroundColor">Background color of the text string (optional).</param>
        void FormattedWrite(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null);

        /// <summary>
        /// Writes a text string the screen, followed by a newline. Applies to: Command Console.
        /// </summary>
        /// <param name="value">String contained the text to be written.</param>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        /// <param name="backgroundColor">Background color of the text string (optional).</param>
        void WriteLine(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null);

        /// <summary>
        /// Writes a text string the screen, without a newline. Applies to: Command Console.
        /// </summary>
        /// <param name="value">String contained the text to be written.</param>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        /// <param name="backgroundColor">Background color of the text string (optional).</param>
        void Write(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null);

        /// <summary>
        /// Reads a line of text from the keyboard. Applies to: Command Console.
        /// </summary>
        /// <param name="prompt">Text to be displayed before the input line. May be omitted.</param>
        /// <returns>A string containing the text read.</returns>
        string ReadLine(string prompt = "");

        /// <summary>
        /// Clears the screen and resets the cursor to top left. Applies to: Command Console.
        /// </summary>
        void ClearScreen();

        /// <summary>
        /// Writes a text string to the screen, centered, with a newline. Applies to: Command Console.
        /// </summary>
        /// <param name="value">String contained the text to be written.</param>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        /// <param name="backgroundColor">Background color of the text string (optional).</param>
        void CenteredWrite(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null);

        /// <summary>
        /// Writes an object the screen (ToString() is called first), centered, with a newline. Applies to: Command Console.
        /// </summary>
        /// <param name="value">String contained the text to be written.</param>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        /// <param name="backgroundColor">Background color of the text string (optional).</param>
        void CenteredWrite(object value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null);

        /// <summary>
        /// Make a beeping sound, exact implementation is determined by the current console.
        /// </summary>
        void Beep();

        /// <summary>
        /// Writes a line of text in large banner format.
        /// </summary>
        /// <param name="text">String contained the text to be written.</param>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        void WriteBanner(string text, ConsoleColor? foregroundColor);

        /// <summary>
        /// Writes an object to the screen in large banner format.
        /// </summary>
        /// <param name="value">Object contained the text to be written.</param>
        /// <param name="foregroundColor">Foreground color of the text string (optional).</param>
        void WriteBanner(object value, ConsoleColor? foregroundColor = null);


        /// <summary>
        /// Implements an environment specific count-down timer, counts from amount to 0.
        /// </summary>
        /// <param name="amount">Start time in milliseconds for the count-down timer.</param>
        /// <param name="largeInterval">Interval in milliseconds that progress will be reported.</param>
        /// <param name="switchTime">Time when the granularity of reporting will be switched, may be null. Ignored if null.</param>
        /// <param name="smallInterval">Small Interval in milliseconds that progress will be reported. Allows timer to speed up when reaching 0. Ignored if null.</param>
        /// <param name="callback">Callback function executed every large interval (or smaill interval). Returns a Timespan object representing the remaining time. </param>
        void CountDownTimer(double amount, double largeInterval, double? switchTime, double? smallInterval, Action<TimeSpan> callback);

        /// <summary>
        /// Simulates the key press gesture for the specified key.
        /// </summary>
        /// <param name="key">The <see cref="T:WindowsInput.Native.VirtualKeyCode" /> for the key.</param>
        void SimulateKeyPress(int key);

        /// <summary>
        /// Simulates the key down gesture for the specified key.
        /// </summary>
        /// <param name="key">The <see cref="T:WindowsInput.Native.VirtualKeyCode" /> for the key.</param>
        void SimulateKeyDown(int key);


        /// <summary>Simulates uninterrupted text entry via the keyboard.</summary>
        /// <param name="text">The text to be simulated.</param>
        void SimulateTextEntry(string text);

        /// <summary>
        /// Reads a string of alphanumeric characters from the terminal and returns them as a string. Enter does not need to be pressed to end input.
        /// Control characters and punctuation are ignored
        /// </summary>
        /// <param name="count">Number of characters to read.</param>
        /// <param name="placeHolder">If supplied a line of this characters will appear and each one will change to the character as it is pressed. May be null.</param>
        /// <returns>The string that has been input.</returns>
        string ReadAlphaNumericString(int count, char? placeHolder = null);
    }
}