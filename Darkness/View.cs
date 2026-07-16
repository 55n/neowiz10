using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Darkness
{
    public static class View
    {
        public const int Width = 50;
        public const int Height = 25;

        public static void Initialize()
        {
            if (Console.WindowWidth < Width || Console.WindowHeight < Height)
            {
                throw new InvalidOperationException("콘솔 창은 최소 50칸 × 25줄이어야 합니다.");
            }

            int left = (Console.WindowWidth - Width) / 2;
            int top = (Console.WindowHeight - Height) / 2;

            Display = new Viewport(
                left: left,
                top: top,
                width: Width,
                height: 12);

            Message = new Viewport(
                left: left,
                top: top + 13,
                width: Width,
                height: 12);

            Console.Clear();
            Console.CursorVisible = false;
        }

        public static Viewport Display { get; private set; }
        public static Viewport Message { get; private set; }
    }

    public class Viewport
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Viewport(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
            {
                Console.SetCursorPosition(Left, Top + y);
                Console.Write(new string(' ', Width + 3)); // +3 없으면 끝에 남음. 이유는 몰?루
            }
        }

        public void Draw(string text)
        {
            Draw(new string[] { text });
        }

        public void Draw(string[] lines)
        {
            Clear();

            for (int i = 0; i < lines.Length && i < Height; i++)
            {
                DrawLine(i, lines[i]);
            }
        }

        public void DrawCentered(string text)
        {
            DrawCentered(new[] { text });
        }

        public void DrawCentered(string[] lines)
        {
            Clear();

            for (int i = 0; i < lines.Length && i < Height; i++)
            {
                DrawLineCentered(i, lines[i]);
            }
        }

        public void DrawCenteredLineByLine(string[] lines)
        {
            Clear();

            for (int i = 0; i < lines.Length && i < Height; i++)
            {
                DrawLineCentered(i, lines[i]);
                Thread.Sleep(100);
            }
        }

        public void ClearLine(int row)
        {
            DrawLine(row, "");
        }

        public void DrawAt(int row, int column, string text)
        {
            if (row < 0 || row >= Height)
            {
                throw new ArgumentOutOfRangeException("row");
            }

            if (column < 0 || column >= Width)
            {
                throw new ArgumentOutOfRangeException("column");
            }

            Console.SetCursorPosition(Left + column, Top + row);
            Console.Write(text);
        }

        public void DrawLine(int row, string text)
        {
            if (row < 0 || row >= Height)
            {
                throw new ArgumentOutOfRangeException("row");
            }

            Console.SetCursorPosition(Left, Top + row);
            Console.Write(new string(' ', Width));
            Console.SetCursorPosition(Left, Top + row);
            Console.Write(text.Length > Width ? text.Substring(0, Width) : text);
        }

        public void DrawLineCentered(int row, string text)
        {
            if (row < 0 || row >= Height)
            {
                throw new ArgumentOutOfRangeException("row");
            }

            Console.SetCursorPosition(Left, Top + row);
            Console.Write(new string(' ', Width));

            int textWidth = GetDisplayWidth(text);
            int offset = Math.Max(0, (Width - textWidth) / 2);
            Console.SetCursorPosition(Left + offset, Top + row);
            Console.Write(text);
        }

        private static int GetDisplayWidth(string text)
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
    }
}
