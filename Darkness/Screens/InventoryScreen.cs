using System.Collections.Generic;

namespace Darkness
{
    public static class InventoryScreen
    {
        private static readonly string[] InventoryActions =
        {
            "선택 취소",
            "사용하기",
            "던지기"
        };
        private static readonly string[] ThrowItemActions =
        {
            "던진다",
            "도로 집어넣는다"
        };

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
                "뒤로가기",
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
                "뒤로가기",
                "장비 선택을 취소한다.",
                true,
                parent,
                new ScreenSelection(ScreenAction.Cancel, null, equipmentSlot)));
            return node;
        }

        public static SelectionNode BuildThrowNode(
            Hero hero,
            SelectionNode parent)
        {
            SelectionNode node = new SelectionNode(
                "throw_inventory",
                "던질 아이템을 선택한다.",
                new List<SelectionOption>(),
                parent);

            foreach (ItemStack itemStack in hero.Inventory.ItemStacks)
            {
                node.Options.Add(new SelectionOption(
                    GetItemText(itemStack),
                    itemStack.Item.Type.Description,
                    true,
                    null,
                    new ScreenSelection(
                        ScreenAction.ThrowItem,
                        itemStack,
                        null)));
            }

            node.Options.Add(new SelectionOption(
                "뒤로가기",
                "아이템 던지기를 취소한다.",
                true,
                parent,
                new ScreenSelection(ScreenAction.Cancel, null, null)));
            return node;
        }

        private static SelectionOption BuildItemOption(
            ItemStack itemStack,
            SelectionNode parent)
        {
            string[] actions = InventoryActions;
            string[] throwActions = ThrowItemActions;
            SelectionNode actionNode = new SelectionNode(
                "inventory_item_" + itemStack.Item.Type.Id,
                itemStack.Item.Type.Description,
                new List<SelectionOption>(),
                parent);
            SelectionNode throwNode = new SelectionNode(
                "inventory_throw_" + itemStack.Item.Type.Id,
                InventoryMessages.ThrowItemPrompt(itemStack.Item.Type.Name),
                new List<SelectionOption>(),
                actionNode);

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
                throwNode));

            throwNode.Options.Add(new SelectionOption(
                throwActions[0],
                "아이템을 던진다.",
                true,
                null,
                new ScreenSelection(ScreenAction.ThrowItem, itemStack, null)));
            throwNode.Options.Add(new SelectionOption(
                throwActions[1],
                "아이템을 다시 소지품에 넣는다.",
                true,
                actionNode));

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
    }
}
