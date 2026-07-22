using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Game game = new Game();
                game.Start();
            }
            catch (ConsoleWindowTooSmallException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
