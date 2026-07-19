using System;
using System.Collections.Generic;

namespace Darkness
{
    public static class StatusScreen
    {
        private static readonly string[] EquipmentSlotNames =
        {
            "무기",
            "방어구",
            "장신구"
        };
        private static readonly string[] EquipmentActions =
        {
            "선택 취소",
            "장비 교체",
            "장비 해제",
            "던지기"
        };

        public static SelectionNode BuildNode(Hero hero, SelectionNode parent)
        {
            SelectionNode node = new SelectionNode(
                "status",
                "상태창",
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

            foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
            {
                AddEquipmentOption(hero, node, slot);
            }

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

            node.Options.Add(new SelectionOption(
                "뒤로가기",
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
            bool equipped = hero.Equipment.TryGetValue(slot, out equipment) && equipment != null;
            SelectionNode actionNode = BuildEquipmentActionNode(
                hero,
                statusNode,
                slot,
                equipment);
            string description = equipped ? equipment.Item.Type.Description : "장비가 장착되어 있지 않다.";
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
            string[] actions = EquipmentActions;
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
                "장비를 해제해 소지품에 넣는다.",
                equipment != null,
                null,
                new ScreenSelection(
                    ScreenAction.UnequipItem,
                    equipment,
                    slot)));
            actionNode.Options.Add(new SelectionOption(
                actions[3],
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
    }
}
