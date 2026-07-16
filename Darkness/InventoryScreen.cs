using System;
using System.Collections.Generic;

namespace Darkness
{
    public enum InventoryOutcome
    {
        None,
        Used,
        Thrown
    }

    public static class InventoryScreen
    {
        private const int InnerWidth = 28;

        public static InventoryOutcome Show(
            Viewport view,
            List<string> itemStacks,
            int slotCount)
        {
            return Show(view, itemStacks, slotCount, null, null, null);
        }

        public static InventoryOutcome Show(
            Viewport view,
            List<string> itemStacks,
            int slotCount,
            Viewport targetView,
            string[] targetSlots,
            bool[] revealedTargetSlots)
        {
            int selected = 0;
            Draw(view, itemStacks, slotCount, selected);

            while (true)
            {
                ConsoleKey input = Utility.ReadInput();
                int previous = selected;
                int selectionCount = slotCount + 1;

                if (input == ConsoleKey.UpArrow)
                {
                    selected = (selected - 1 + selectionCount) % selectionCount;
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    selected = (selected + 1) % selectionCount;
                }
                else if (input == ConsoleKey.Enter)
                {
                    if (selected == slotCount)
                    {
                        return InventoryOutcome.None;
                    }

                    if (selected >= itemStacks.Count)
                    {
                        continue;
                    }

                    int selectedAction = Selection.ChooseLeft(
                        view,
                        Narrative.InventoryActions());

                    if (selectedAction == 0)
                    {
                        Draw(view, itemStacks, slotCount, selected);
                        continue;
                    }

                    if (selectedAction == 2)
                    {
                        string itemStack = itemStacks[selected];
                        view.Draw(Narrative.ThrowItemPrompt(itemStack));
                        int selectedThrowAction = Selection.ChooseLeft(
                            view,
                            Narrative.ThrowItemActions(),
                            1);

                        if (selectedThrowAction == 0)
                        {
                            if (targetView != null &&
                                targetSlots != null &&
                                revealedTargetSlots != null)
                            {
                                EncounterScreen.ChooseAnySlot(
                                    targetView,
                                    targetSlots,
                                    revealedTargetSlots);
                            }

                            Utility.PlayMessage(Narrative.ItemThrown(itemStack));
                            return InventoryOutcome.Thrown;
                        }

                        Draw(view, itemStacks, slotCount, selected);
                        continue;
                    }

                    return InventoryOutcome.Used;
                }

                if (previous != selected)
                {
                    DrawSelectionLine(
                        view,
                        itemStacks,
                        slotCount,
                        previous,
                        false);
                    DrawSelectionLine(
                        view,
                        itemStacks,
                        slotCount,
                        selected,
                        true);
                }
            }
        }

        private static void Draw(
            Viewport view,
            List<string> itemStacks,
            int slotCount,
            int selected)
        {
            string[] lines = new string[slotCount + 3];
            lines[0] = "┌" + new string('─', InnerWidth) + "┐";

            for (int i = 0; i < slotCount; i++)
            {
                string itemStack = i < itemStacks.Count ? itemStacks[i] : "";
                lines[i + 1] = BuildSlotLine(itemStack, i == selected);
            }

            lines[slotCount + 1] = "└" + new string('─', InnerWidth) + "┘";
            lines[slotCount + 2] = BuildBackLine(selected == slotCount);

            view.Draw(lines);
        }

        private static void DrawSelectionLine(
            Viewport view,
            List<string> itemStacks,
            int slotCount,
            int selection,
            bool selected)
        {
            if (selection == slotCount)
            {
                view.DrawLine(slotCount + 2, BuildBackLine(selected));
                return;
            }

            string itemStack = selection < itemStacks.Count
                ? itemStacks[selection]
                : "";
            view.DrawLine(selection + 1, BuildSlotLine(itemStack, selected));
        }

        private static string BuildBackLine(bool selected)
        {
            return (selected ? "▶ " : "  ") + Narrative.BackAction();
        }

        private static string BuildSlotLine(string itemStack, bool selected)
        {
            string content = (selected ? "▶ " : "  ") + itemStack;
            int padding = Math.Max(0, InnerWidth - GetDisplayWidth(content));
            return "│" + content + new string(' ', padding) + "│";
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
    }
}
