using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Timers;
using WindowsInput;
using WindowsInput.Native;
using AdventureLandCore.Extensions;
using MLDComputing.ObjectBrowser.Attributes;
#pragma warning disable 1591

namespace AdventureLandCore.Services.CoreApi
{
    [IgnoreInObjectBrowser]
    public abstract class CommandConsoleBase
    {
        private double _amountInMilliseconds;
        private DateTime _startTime;
        private Timer _timer;

        protected string LogPathName { get; set; }

        protected string TurnLoggingOnHelper(string pathName)
        {
            var pathToCreate = Path.GetDirectoryName(pathName);

            if (!string.IsNullOrWhiteSpace(pathToCreate)  && !Directory.Exists(pathToCreate) && Path.GetPathRoot(pathToCreate) != pathToCreate)
            {
                Directory.CreateDirectory(pathToCreate);
            }

            File.AppendAllText(pathName, $"Logging started at {DateTime.Now.ToLocalTime()}.".AddLineBreaks().PrefixLineBreaks());

            LogPathName = pathName;

            return Path.GetFullPath(pathName);
        }

        protected void LogText(string value)
        {
            if (!string.IsNullOrWhiteSpace(LogPathName))
            {
                File.AppendAllText(LogPathName, value);
            }
        }

        protected Color MapToDrawingColor(ConsoleColor foregroundColor)
        {
            var colorMap = new Dictionary<ConsoleColor, Color>
            {
                {ConsoleColor.Black, Color.Black},
                {ConsoleColor.DarkBlue, Color.DarkBlue},
                {ConsoleColor.DarkGreen, Color.DarkGreen},
                {ConsoleColor.DarkCyan, Color.DarkCyan},
                {ConsoleColor.DarkRed, Color.DarkRed},
                {ConsoleColor.DarkMagenta, Color.DarkMagenta},
                {ConsoleColor.DarkYellow, Color.Gold},
                {ConsoleColor.Gray, Color.Gray},
                {ConsoleColor.Green, Color.Green},
                {ConsoleColor.Cyan, Color.Cyan},
                {ConsoleColor.Red, Color.Red},
                {ConsoleColor.Magenta, Color.Magenta},
                {ConsoleColor.Yellow, Color.Yellow},
                {ConsoleColor.White, Color.White},
                {ConsoleColor.Blue, Color.Blue}
            };

            return colorMap[foregroundColor];
        }

        public void SimulateKeyPress(int key)
        {
            var simulator = new InputSimulator();
            simulator.Keyboard.KeyPress((VirtualKeyCode) key);
        }

        public void SimulateKeyDown(int key)
        {
            var simulator = new InputSimulator();
            simulator.Keyboard.KeyDown((VirtualKeyCode) key);
        }

        /// <summary>Simulates uninterrupted text entry via the keyboard.</summary>
        /// <param name="text">The text to be simulated.</param>
        public void SimulateTextEntry(string text)
        {
            var simulator = new InputSimulator();
            simulator.Keyboard.TextEntry(text);
        }
        
        public void CountDownTimer(double amount, double largeInterval, double? switchTime, double? smallInterval, Action<TimeSpan> callback)
        {
            double GetRemainingTime()
            {
                var currentTime = DateTime.Now;

                var remainingTime = Math.Max(_amountInMilliseconds - (currentTime - _startTime).TotalMilliseconds, 0);

                callback(TimeSpan.FromMilliseconds(remainingTime));

                return remainingTime;
            }

            _amountInMilliseconds = amount;
            _startTime = DateTime.Now;

            _timer?.Dispose();

            _timer = new Timer { Interval = largeInterval };

            GetRemainingTime();

            _timer.Elapsed += (s, e) =>
            {
                var remainingTime = GetRemainingTime();

                if (remainingTime <= 0)
                {
                    _timer.Stop();
                }

                else if (smallInterval.HasValue && switchTime.HasValue && remainingTime < switchTime)
                {
                    _timer.Interval = smallInterval.Value;
                }
            };

            _timer.Start();
        }
    }
}