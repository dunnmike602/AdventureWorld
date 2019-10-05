using System.Collections.Generic;
using System.Reflection;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;

namespace AdventureLandCore.Services
{
    public static class LanguageHelper
    {
        private static readonly string StopWords;
        private static readonly Dictionary<int, Direction> Directions;

        private const int NorthIndex = 3;
        private const int SouthIndex = 7;
        private const int EastIndex = 1;
        private const int WestIndex = 5;
        private const int SouthEastIndex = 8;
        private const int SouthWestIndex = 6;
        private const int NorthEastIndex = 2;
        private const int NorthWestIndex = 4;
        private const int UpIndex = 9;
        private const int DownIndex = 10;

        static LanguageHelper()
        {
            StopWords = Assembly.GetExecutingAssembly()
                .GetResourceAsString("AdventureLandCore.LanguageResources.StopWords.txt");

            Directions = new Dictionary<int, Direction>
            {
                {NorthIndex, new Direction {Abbreviation = "N", Text = "NORTH"}},
                {SouthIndex, new Direction {Abbreviation = "S", Text = "SOUTH"}},
                {EastIndex, new Direction {Abbreviation = "E", Text = "EAST"}},
                {WestIndex, new Direction {Abbreviation = "W", Text = "WEST"}},
                {SouthEastIndex, new Direction {Abbreviation = "SE", Text = "SOUTHEAST"}},
                {SouthWestIndex, new Direction {Abbreviation = "SW", Text = "SOUTHWEST"}},
                {NorthEastIndex, new Direction {Abbreviation = "NE", Text = "NORTHEAST"}},
                {NorthWestIndex, new Direction {Abbreviation = "NW", Text = "NORTHWEST"}},
                {UpIndex, new Direction {Abbreviation = "U", Text = "UP"}},
                {DownIndex, new Direction {Abbreviation = "D", Text = "DOWN"}},
            };
        }

        public static Dictionary<int, Direction> GetDirections()
        {
            return Directions;
        }

        public static string GetStopWords()
        {
            return StopWords.Trim();
        }
    }
}