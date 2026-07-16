using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    public static class Utility
    {
        public static ConsoleKey ReadInput()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow ||
                    key == ConsoleKey.DownArrow ||
                    key == ConsoleKey.LeftArrow ||
                    key == ConsoleKey.RightArrow ||
                    key == ConsoleKey.Enter)
                {
                    return key;
                }
            }
        }

        public static void WaitForEnter()
        {
            while (ReadInput() != ConsoleKey.Enter)
            {
            }
        }

        public static void PlayMessages(string[] messages)
        {
            View.Message.Clear();

            for (int i = 0; i < messages.Length; i++)
            {
                View.Message.DrawLine(i, messages[i]);
                Utility.WaitForEnter();
            }
        }

        public static void PlayMessage(string message)
        {
            PlayMessages(new string[] { message });
        }

    }
}
