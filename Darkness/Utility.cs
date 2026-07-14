using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    public static class Utility
    {
        public static void WriteLine(string output = "")
        {
            Console.WriteLine(output);
            ConsoleKeyInfo input = Console.ReadKey(true);

            while(input.Key != ConsoleKey.Enter)
            {
                input = Console.ReadKey(true);
            }
        }
    }
}
