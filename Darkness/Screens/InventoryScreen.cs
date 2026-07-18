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

        public static SelectionNode BuildNode(Hero hero, SelectionNode parent)
        {
            SelectionNode node = new SelectionNode(
                "inventory",
                "소지품을 확인한다.",
                new List<SelectionOption>(),
                parent);

            for (int i = 0; i < hero.Inventory.Capacity; i++)
            {
                if (i < hero.Inventory.ItemStacks.Count)
                {
                    ItemStack itemStack = hero.Inventory.ItemStacks[i];
                    node.Options.Add(BuildItemOption(itemStack, node));
                }
                else
                {
                    node.Options.Add(new SelectionOption(
                        "[빈 슬롯]",
                        "아무것도 들어 있지 않다.",
                        false,
                        null));
                }
            }

            node.Options.Add(new SelectionOption(
                Narrative.BackAction(),
                "이전 화면으로 돌아간다.",
                true,
                parent,
                new ScreenSelection(ScreenAction.Back, null, null)));
            return node;
        }

        public static SelectionNode BuildEquipmentNode(
            Hero hero,
            EquipmentSlot equipmentSlot,
            SelectionNode parent)
        {
            SelectionNode node = new SelectionNode(
                "equipment_inventory_" + equipmentSlot,
                "교체할 장비를 선택한다.",
                new List<SelectionOption>(),
                parent);
            ItemCategory category = GetCategory(equipmentSlot);

            foreach (ItemStack itemStack in hero.Inventory.ItemStacks)
            {
                if (itemStack.Item.Type.Category != category)
                {
                    continue;
                }

                node.Options.Add(new SelectionOption(
                    GetItemText(itemStack),
                    itemStack.Item.Type.Description,
                    true,
                    null,
                    new ScreenSelection(
                        ScreenAction.EquipItem,
                        itemStack,
                        equipmentSlot)));
            }

            node.Options.Add(new SelectionOption(
                Narrative.BackAction(),
                "장비 선택을 취소한다.",
                true,
                parent,
                new ScreenSelection(ScreenAction.Cancel, null, equipmentSlot)));
            return node;
        }

        private static SelectionOption BuildItemOption(
            ItemStack itemStack,
            SelectionNode parent)
        {
            string[] actions = Narrative.InventoryActions();
            SelectionNode actionNode = new SelectionNode(
                "inventory_item_" + itemStack.Item.Type.Id,
                itemStack.Item.Type.Description,
                new List<SelectionOption>(),
                parent);

            actionNode.Options.Add(new SelectionOption(
                actions[0],
                "아이템 선택을 취소한다.",
                true,
                parent,
                new ScreenSelection(ScreenAction.Cancel, itemStack, null)));
            actionNode.Options.Add(new SelectionOption(
                actions[1],
                itemStack.Item.Type.Description,
                itemStack.Item.Type.IsUsable,
                null,
                new ScreenSelection(ScreenAction.UseItem, itemStack, null)));
            actionNode.Options.Add(new SelectionOption(
                actions[2],
                "아이템을 던진다.",
                true,
                null,
                new ScreenSelection(ScreenAction.ThrowItem, itemStack, null)));

            return new SelectionOption(
                GetItemText(itemStack),
                itemStack.Item.Type.Description,
                true,
                actionNode);
        }

        private static ItemCategory GetCategory(EquipmentSlot equipmentSlot)
        {
            switch (equipmentSlot)
            {
                case EquipmentSlot.Weapon:
                    return ItemCategory.Weapon;
                case EquipmentSlot.Armor:
                    return ItemCategory.Armor;
                default:
                    return ItemCategory.Accessory;
            }
        }

        private static string GetItemText(ItemStack itemStack)
        {
            return itemStack.Count > 1
                ? itemStack.Item.Type.Name + " x" + itemStack.Count
                : itemStack.Item.Type.Name;
        }

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
            int padding = Math.Max(0, InnerWidth - Utility.GetDisplayWidth(content));
            return "│" + content + new string(' ', padding) + "│";
        }

    }
}
