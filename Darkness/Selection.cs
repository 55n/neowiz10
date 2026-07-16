using System;

namespace Darkness
{
    public static class Selection
    {
        public static int Choose(Viewport view, string[] options)
        {
            return Choose(view, options, true, true, 0);
        }

        public static int Choose(Viewport view, string[] options, int startRow)
        {
            return Choose(view, options, true, false, startRow);
        }

        public static int ChooseLeft(Viewport view, string[] options)
        {
            return Choose(view, options, false, true, 0);
        }

        public static int ChooseLeft(Viewport view, string[] options, int startRow)
        {
            return Choose(view, options, false, false, startRow);
        }

        private static int Choose(
            Viewport view,
            string[] options,
            bool centered,
            bool clearView,
            int startRow)
        {
            int selected = 0;
            string[] lines = Format(options, selected);

            if (clearView)
            {
                if (centered)
                {
                    view.DrawCentered(lines);
                }
                else
                {
                    view.Draw(lines);
                }
            }
            else
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    DrawLine(view, startRow + i, lines[i], centered);
                }
            }

            while (true)
            {
                ConsoleKey input = Utility.ReadInput();
                int previous = selected;

                if (input == ConsoleKey.UpArrow)
                {
                    selected = (selected - 1 + options.Length) % options.Length;
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    selected = (selected + 1) % options.Length;
                }
                else if (input == ConsoleKey.Enter)
                {
                    return selected;
                }

                if (previous != selected)
                {
                    DrawLine(view, startRow + previous, FormatLine(options[previous], false), centered);
                    DrawLine(view, startRow + selected, FormatLine(options[selected], true), centered);
                }
            }
        }

        private static void DrawLine(Viewport view, int row, string text, bool centered)
        {
            if (centered)
            {
                view.DrawLineCentered(row, text);
            }
            else
            {
                view.DrawLine(row, text);
            }
        }

        private static string[] Format(string[] options, int selected)
        {
            // ▶▷▲△

            string[] lines = new string[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                lines[i] = FormatLine(options[i], i == selected);
            }

            return lines;
        }

        private static string FormatLine(string option, bool selected)
        {
            return (selected ? "▶ " : "  ") + option;
        }
    }
}
