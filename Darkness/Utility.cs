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

        public static string TruncateToDisplayWidth(string text, int maxWidth)
        {
            if (string.IsNullOrEmpty(text) || maxWidth <= 0)
            {
                return "";
            }

            StringBuilder result = new StringBuilder();
            int width = 0;
            foreach (char character in text)
            {
                int characterWidth = GetDisplayWidth(character.ToString());
                if (width + characterWidth > maxWidth)
                {
                    break;
                }

                result.Append(character);
                width += characterWidth;
            }

            return result.ToString();
        }

        public static IEnumerable<string> WrapText(string text, int maxWidth)
        {
            if (maxWidth <= 0)
            {
                throw new ArgumentOutOfRangeException("maxWidth");
            }

            string normalized = (text ?? "").Replace("\r\n", "\n").Replace('\r', '\n');
            string[] paragraphs = normalized.Split('\n');

            foreach (string paragraph in paragraphs)
            {
                string remaining = paragraph;
                if (remaining.Length == 0)
                {
                    yield return "";
                    continue;
                }

                while (GetDisplayWidth(remaining) > maxWidth)
                {
                    int splitIndex = FindWrapIndex(remaining, maxWidth);
                    string line = remaining.Substring(0, splitIndex).TrimEnd();
                    yield return line;
                    remaining = remaining.Substring(splitIndex).TrimStart();
                }

                yield return remaining;
            }
        }

        private static int FindWrapIndex(string text, int maxWidth)
        {
            int width = 0;
            int lastWhitespace = -1;

            for (int i = 0; i < text.Length; i++)
            {
                int characterWidth = GetDisplayWidth(text[i].ToString());
                if (width + characterWidth > maxWidth)
                {
                    return lastWhitespace > 0 ? lastWhitespace : Math.Max(1, i);
                }

                width += characterWidth;
                if (char.IsWhiteSpace(text[i]))
                {
                    lastWhitespace = i;
                }
            }

            return text.Length;
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
            int row = 0;

            foreach (string message in messages)
            {
                foreach (string line in WrapText(message, View.Message.Width))
                {
                    if (row >= View.Message.Height)
                    {
                        View.Message.Clear();
                        row = 0;
                    }

                    View.Message.DrawLine(row, line);
                    row++;
                    Utility.WaitForEnter();
                }
            }
        }

        public static void PlayMessage(string message)
        {
            PlayMessages(new string[] { message });
        }

    }
}
