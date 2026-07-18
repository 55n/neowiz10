using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    public static class Utility
    {
        public static int GetDisplayWidth(string text)
        {
            int width = 0;
            foreach (char character in text)
            {
                bool singleWidth = character <= '\u007e' ||
                                   (character >= '\u2500' && character <= '\u259f');
                width += singleWidth ? 1 : 2;
            }

            return width;
        }

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
