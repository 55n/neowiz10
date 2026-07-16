using System;
using System.Text;

namespace Darkness
{
    public static class EncounterScreen
    {
        private const int SlotCount = 5;
        private const int SlotWidth = 8;
        private const int SlotGap = 2;
        private const int SlotHeight = 7;
        private const int Top = 2;

        public static int ChooseSlot(
            Viewport view,
            string[] slotContents,
            bool[] revealedSlots)
        {
            int selected = Array.FindIndex(revealedSlots, revealed => !revealed);

            DrawSlots(view, slotContents, revealedSlots);
            DrawSelector(view, selected);

            while (true)
            {
                ConsoleKey input = Utility.ReadInput();
                int previous = selected;

                if (input == ConsoleKey.LeftArrow)
                {
                    do
                    {
                        selected = (selected - 1 + SlotCount) % SlotCount;
                    }
                    while (revealedSlots[selected]);
                }
                else if (input == ConsoleKey.RightArrow)
                {
                    do
                    {
                        selected = (selected + 1) % SlotCount;
                    }
                    while (revealedSlots[selected]);
                }
                else if (input == ConsoleKey.Enter)
                {
                    return selected;
                }

                if (previous != selected)
                {
                    DrawSelector(view, selected);
                }
            }
        }

        public static void DrawSlots(
            Viewport view,
            string[] slotContents,
            bool[] revealedSlots)
        {
            int left = (view.Width - GetTotalWidth()) / 2;

            view.Clear();
            for (int row = 0; row < SlotHeight; row++)
            {
                view.DrawLine(
                    Top + row,
                    BuildSlotLine(slotContents, revealedSlots, row, left));
            }
        }

        private static void DrawSelector(Viewport view, int selected)
        {
            int selectorRow = Top + SlotHeight;
            int left = (view.Width - GetTotalWidth()) / 2;
            int markerColumn = left + selected * (SlotWidth + SlotGap) + 3;

            view.ClearLine(selectorRow);
            view.DrawAt(selectorRow, markerColumn, "▲");
        }

        private static string BuildSlotLine(
            string[] slotContents,
            bool[] revealedSlots,
            int row,
            int left)
        {
            StringBuilder line = new StringBuilder(new string(' ', left));
            for (int i = 0; i < SlotCount; i++)
            {
                if (i > 0)
                {
                    line.Append(new string(' ', SlotGap));
                }
                line.Append(BuildSlot(slotContents[i], revealedSlots[i], row));
            }

            return line.ToString();
        }

        private static string BuildSlot(string content, bool revealed, int row)
        {
            if (row == 0)
            {
                return revealed ? "┌──────┐" : "┌─  ─  ┐";
            }

            if (row == SlotHeight - 1)
            {
                return revealed ? "└──────┘" : "└─  ─  ┘";
            }

            string edge = "│";
            if (!string.IsNullOrEmpty(content) && row == SlotHeight / 2)
            {
                int contentWidth = GetDisplayWidth(content);
                int leftPadding = (SlotWidth - 2 - contentWidth) / 2;
                int rightPadding = SlotWidth - 2 - contentWidth - leftPadding;
                return edge + new string(' ', leftPadding) + content +
                       new string(' ', rightPadding) + edge;
            }

            if (!revealed)
            {
                return row % 2 == 1 ? "│      │" : "        ";
            }

            return edge + new string(' ', SlotWidth - 2) + edge;
        }

        private static int GetDisplayWidth(string text)
        {
            int width = 0;
            foreach (char character in text)
            {
                width += character <= '\u007e' ? 1 : 2;
            }

            return width;
        }

        private static int GetTotalWidth()
        {
            return SlotCount * SlotWidth + (SlotCount - 1) * SlotGap;
        }
    }
}
