using System;
using System.Collections.Generic;

namespace Darkness
{
    public static class StatusScreen
    {
        private const int InnerWidth = 46;
        private static readonly string[] EquipmentSlotNames =
        {
            "무기",
            "방어구",
            "장신구"
        };

        public static SelectionNode BuildNode(Hero hero, SelectionNode parent)
        {
            SelectionNode node = new SelectionNode(
                "status",
                "현재 상태를 확인한다.",
                new List<SelectionOption>(),
                parent);

            node.Options.Add(new SelectionOption(
                "[생명력]: " + hero.CurrentHealth + "/" + hero.Type.MaxHealth,
                "현재 생명력과 최대 생명력이다.",
                true,
                null));
            node.Options.Add(new SelectionOption(
                "[집중력]: " + hero.CurrentFocus + "/" + hero.Type.MaxFocus,
                "현재 집중력과 최대 집중력이다.",
                true,
                null));

            foreach (ActiveEffect effect in hero.Effects)
            {
                string name = effect.Type == null ? "알 수 없는 Effect" : effect.Type.Name;
                string description = effect.Type == null
                    ? "Effect 정보가 없다."
                    : effect.Type.Description;
                node.Options.Add(new SelectionOption(
                    "[Effect]: " + name + " x" + effect.StackCount,
                    description,
                    true,
                    null));
            }

            foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
            {
                AddEquipmentOption(hero, node, slot);
            }

            node.Options.Add(new SelectionOption(
                Narrative.BackAction(),
                "이전 화면으로 돌아간다.",
                true,
                parent,
                new ScreenSelection(ScreenAction.Back, null, null)));
            return node;
        }

        private static void AddEquipmentOption(
            Hero hero,
            SelectionNode statusNode,
            EquipmentSlot slot)
        {
            ItemStack equipment;
            bool equipped = hero.Equipment.TryGetValue(slot, out equipment) &&
                            equipment != null;
            SelectionNode actionNode = BuildEquipmentActionNode(
                hero,
                statusNode,
                slot,
                equipment);
            string description = equipped
                ? equipment.Item.Type.Description
                : "장비가 장착되어 있지 않다.";
            string name = equipped ? equipment.Item.Type.Name : "비어 있음";

            statusNode.Options.Add(new SelectionOption(
                "[" + GetSlotName(slot) + "]: " + name,
                description,
                true,
                actionNode));
        }

        private static SelectionNode BuildEquipmentActionNode(
            Hero hero,
            SelectionNode statusNode,
            EquipmentSlot slot,
            ItemStack equipment)
        {
            string[] actions = Narrative.EquipmentActions();
            SelectionNode actionNode = new SelectionNode(
                "equipment_" + slot,
                equipment == null
                    ? "장착할 장비를 선택할 수 있다."
                    : equipment.Item.Type.Description,
                new List<SelectionOption>(),
                statusNode);

            actionNode.Options.Add(new SelectionOption(
                actions[0],
                "장비 선택을 취소한다.",
                true,
                statusNode,
                new ScreenSelection(ScreenAction.Cancel, equipment, slot)));
            actionNode.Options.Add(new SelectionOption(
                actions[1],
                "소지품에서 교체할 장비를 고른다.",
                true,
                InventoryScreen.BuildEquipmentNode(hero, slot, actionNode)));
            actionNode.Options.Add(new SelectionOption(
                actions[2],
                "장착 중인 아이템을 던진다.",
                equipment != null,
                null,
                new ScreenSelection(ScreenAction.ThrowItem, equipment, slot)));
            return actionNode;
        }

        private static string GetSlotName(EquipmentSlot slot)
        {
            return EquipmentSlotNames[(int)slot];
        }

        public static void Show(
            Viewport view,
            int health,
            int maxHealth,
            int focus,
            int maxFocus,
            string[] statusEffects,
            string[] equipment,
            List<string> itemStacks,
            Dictionary<string, int> equipmentSlots)
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
                    i,
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
                                i,
                                i == selected);
                        }
                        lines[backRow] = BuildBackLine(selected == equipment.Length);

                        view.Draw(lines);
                        continue;
                    }

                    if (selectedAction == 1)
                    {
                        List<int> compatibleItemIndexes = new List<int>();
                        List<string> compatibleEquipment = new List<string>();
                        for (int i = 0; i < itemStacks.Count; i++)
                        {
                            int equipmentSlot;
                            if (equipmentSlots.TryGetValue(itemStacks[i], out equipmentSlot) &&
                                equipmentSlot == selected)
                            {
                                compatibleItemIndexes.Add(i);
                                compatibleEquipment.Add(itemStacks[i]);
                            }
                        }

                        compatibleEquipment.Add(Narrative.BackAction());
                        int selectedEquipment = Selection.ChooseLeft(
                            view,
                            compatibleEquipment.ToArray());

                        if (selectedEquipment < compatibleItemIndexes.Count)
                        {
                            int itemIndex = compatibleItemIndexes[selectedEquipment];
                            string previousEquipment = equipment[selected];
                            equipment[selected] = itemStacks[itemIndex];
                            itemStacks[itemIndex] = previousEquipment;
                        }

                        for (int i = 0; i < equipment.Length; i++)
                        {
                            lines[equipmentStartRow + i] = BuildEquipmentLine(
                                equipment[i],
                                i,
                                i == selected);
                        }
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
                BuildEquipmentLine(
                    equipment[selection],
                    selection,
                    selected));
        }

        private static string BuildBackLine(bool selected)
        {
            return (selected ? "▶ " : "  ") + Narrative.BackAction();
        }

        private static string BuildEquipmentLine(
            string equipment,
            int equipmentSlot,
            bool selected)
        {
            string content = $"[{EquipmentSlotNames[equipmentSlot]}]: {equipment}";
            return BuildContentLine((selected ? "▶ " : "  ") + content);
        }

        private static string BuildContentLine(string content)
        {
            int padding = Math.Max(0, InnerWidth - Utility.GetDisplayWidth(content));
            return "│" + content + new string(' ', padding) + "│";
        }

    }
}
