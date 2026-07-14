using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    public static class View
    {
        static View()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Canvas = new Viewport(
                left: 0,
                top: 0,
                width: width,
                height: height - 6);

            Message = new Viewport(
                left: 0,
                top: height - 6,
                width: width,
                height: 6);
        }

        public static readonly Viewport Canvas;
        public static readonly Viewport Message;
    }

    public class Viewport
    {
        public int Left { get; }
        public int Top { get; }
        public int Width { get; }
        public int Height { get; }

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
                Console.Write(new string(' ', Width));
            }
        }

        public void Draw(string text)
        {
            Draw(new[] { text });
        }

        public void Draw(string[] lines)
        {
            Clear();

            for (int i = 0; i < lines.Length && i < Height; i++)
            {
                Console.SetCursorPosition(Left, Top + i);

                string line = lines[i];

                //if (line.Length > Width);
            }
        }
    }
}
