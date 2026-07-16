using System;

namespace Darkness
{
    public class SelectionOption
    {
        public SelectionOption(string text, bool enabled, string description)
        {
            Text = text;
            Enabled = enabled;
            Description = description;
        }

        public string Text { get; private set; }
        public bool Enabled { get; private set; }
        public string Description { get; private set; }
    }

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

        public static int ChooseLeft(
            Viewport view,
            SelectionOption[] options,
            int startRow,
            int descriptionRow)
        {
            return Choose(view, options, false, false, startRow, descriptionRow);
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

        private static int Choose(
            Viewport view,
            SelectionOption[] options,
            bool centered,
            bool clearView,
            int startRow,
            int descriptionRow)
        {
            int selected = FirstEnabledIndex(options);

            if (clearView)
            {
                view.Clear();
            }

            DrawDescription(view, descriptionRow, options[selected].Description);
            for (int i = 0; i < options.Length; i++)
            {
                DrawOptionLine(view, startRow + i, options[i], i == selected, centered);
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
                else if (input == ConsoleKey.Enter && options[selected].Enabled)
                {
                    return selected;
                }

                if (previous != selected)
                {
                    DrawDescription(view, descriptionRow, options[selected].Description);
                    DrawOptionLine(view, startRow + previous, options[previous], false, centered);
                    DrawOptionLine(view, startRow + selected, options[selected], true, centered);
                }
            }
        }

        private static int FirstEnabledIndex(SelectionOption[] options)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].Enabled)
                {
                    return i;
                }
            }

            return 0;
        }

        private static void DrawDescription(Viewport view, int row, string description)
        {
            view.DrawLine(row, description ?? "");
        }

        private static void DrawOptionLine(
            Viewport view,
            int row,
            SelectionOption option,
            bool selected,
            bool centered)
        {
            string line = FormatLine(option.Text, selected);
            if (option.Enabled)
            {
                DrawLine(view, row, line, centered);
                return;
            }

            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            DrawLine(view, row, line, centered);
            Console.ForegroundColor = previousColor;
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
