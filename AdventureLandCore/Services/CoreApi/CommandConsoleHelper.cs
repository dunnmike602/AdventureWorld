using System;
using MLDComputing.ObjectBrowser.Attributes;

namespace AdventureLandCore.Services.CoreApi
{
    [IgnoreInObjectBrowser]
    public static class CommandConsoleHelper
    {
        public static int? ReadKeyWithTimeOut(int timeOutMS)
        {
            var timeoutvalue = DateTime.Now.AddMilliseconds(timeOutMS);

            while (DateTime.Now < timeoutvalue)
            {

                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    return (int?) cki.Key;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                }

            }

            return null;
        }
    }
}