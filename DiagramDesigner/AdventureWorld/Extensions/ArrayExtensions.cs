using System;
using System.Linq;

namespace DiagramDesigner.AdventureWorld.Extensions
{
    public static class ArrayExtensions
    {
        public static int FindFirstMissingNumber(this int[] array)
        {
            if(array.Length == 0)
            {
                return 1;
            }

            Array.Sort(array);

            var lastElement = array[array.Length - 1];

            var result = Enumerable.Range(1, lastElement).Except(array).ToArray(); 

            if(result.Length == 0)
            {
                return lastElement + 1;
            }
            else
            {
                return result[0];
            }
        }
    }
}
