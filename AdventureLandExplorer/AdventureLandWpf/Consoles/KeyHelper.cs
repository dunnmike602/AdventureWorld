using System;
using System.Windows.Input;

namespace AdventureLandExplorer.AdventureLandWpf.Consoles
{
    public static class KeyHelper
    {
        public static char? GetCharacterFromKey(this Key key)
        {
            switch (key)
            {
                case Key.Space:
                    return ' ';
                case Key.A:
                    return 'A';
                case Key.B:
                    return 'B';
                case Key.C:
                    return 'C';
                case Key.D:
                    return 'D';
                case Key.E:
                    return 'E';
                case Key.F:
                    return 'F';
                case Key.G:
                    return 'G';
                case Key.H:
                    return 'H';
                case Key.I:
                    return 'I';
                case Key.J:
                    return 'J';
                case Key.K:
                    return 'K';
                case Key.L:
                    return 'L';
                case Key.M:
                    return 'M';
                case Key.N:
                    return 'N';
                case Key.O:
                    return 'O';
                case Key.P:
                    return 'P';
                case Key.Q:
                    return 'Q';
                case Key.R:
                    return 'R';
                case Key.S:
                    return 'S';
                case Key.T:
                    return 'T';
                case Key.U:
                    return 'U';
                case Key.V:
                    return 'V';
                case Key.W:
                    return 'W';
                case Key.X:
                    return 'X';
                case Key.Y:
                    return 'Y';
                case Key.Z:
                    return 'Z';
                case Key.D0:
                    return '0';
                case Key.NumPad1:
                case Key.D1:
                    return '1';
                case Key.NumPad2:
                case Key.D2:
                    return '2';
                case Key.NumPad3:
                case Key.D3:
                    return '3';
                case Key.NumPad4:
                case Key.D4:
                    return '4';
                case Key.NumPad5:
                case Key.D5:
                    return '5';
                case Key.NumPad6:
                case Key.D6:
                    return '6';
                case Key.NumPad7:
                case Key.D7:
                    return '7';
                case Key.NumPad8:
                case Key.D8:
                    return '8';
                case Key.NumPad9:
                case Key.D9:
                    return '9';
                default:
                    return null;
            }
            
        }

    }
}
