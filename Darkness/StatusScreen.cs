using System;
using System.Collections.Generic;

namespace Darkness
{
    public static class StatusScreen
    {
        private const int InnerWidth = 48;

        public static void Show(
            Viewport view,
            int health,
            int maxHealth,
            int focus,
            int maxFocus,
            string[] statusEffects,
            string[] equipment)
        {
            List<string> contents = new List<string>();
            contents.Add($"[생명력]: {health}/{maxHealth}");
            contents.Add($"[집중력]: {focus}/{maxFocus}");
            contents.Add("[상태이상]:");
            contents.AddRange(statusEffects);

            int equipmentStartRow = contents.Count + 1;
            int backRow = contents.Count + equipment.Length + 2;
            string[] lines = new string[contents.Count + equipment.Length + 3];
            lines[0] = "┌" + new string('─', InnerWidth) + "┐";
            for (int i = 0; i < contents.Count; i++)
            {
                lines[i + 1] = BuildContentLine(contents[i]);
            }

            for (int i = 0; i < equipment.Length; i++)
            {
                lines[equipmentStartRow + i] = BuildEquipmentLine(
                    equipment[i],
                    i == 0);
            }

            lines[backRow - 1] = "└" + new string('─', InnerWidth) + "┘";
            lines[backRow] = BuildBackLine(false);

            view.Draw(lines);

            int selected = 0;
            while (true)
            {
                ConsoleKey input = Utility.ReadInput();
                int previous = selected;
                int selectionCount = equipment.Length + 1;

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
                    if (selected == equipment.Length)
                    {
                        return;
                    }

                    int selectedAction = Selection.ChooseLeft(
                        view,
                        Narrative.EquipmentActions());

                    if (selectedAction == 0)
                    {
                        for (int i = 0; i < equipment.Length; i++)
                        {
                            lines[equipmentStartRow + i] = BuildEquipmentLine(
                                equipment[i],
                                i == selected);
                        }
                        lines[backRow] = BuildBackLine(selected == equipment.Length);

                        view.Draw(lines);
                        continue;
                    }

                    return;
                }

                if (previous != selected)
                {
                    DrawSelectionLine(
                        view,
                        equipment,
                        equipmentStartRow,
                        backRow,
                        previous,
                        false);
                    DrawSelectionLine(
                        view,
                        equipment,
                        equipmentStartRow,
                        backRow,
                        selected,
                        true);
                }
            }
        }

        private static void DrawSelectionLine(
            Viewport view,
            string[] equipment,
            int equipmentStartRow,
            int backRow,
            int selection,
            bool selected)
        {
            if (selection == equipment.Length)
            {
                view.DrawLine(backRow, BuildBackLine(selected));
                return;
            }

            view.DrawLine(
                equipmentStartRow + selection,
                BuildEquipmentLine(equipment[selection], selected));
        }

        private static string BuildBackLine(bool selected)
        {
            return (selected ? "▶ " : "  ") + Narrative.BackAction();
        }

        private static string BuildEquipmentLine(string equipment, bool selected)
        {
            return BuildContentLine((selected ? "▶ " : "  ") + equipment);
        }

        private static string BuildContentLine(string content)
        {
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
