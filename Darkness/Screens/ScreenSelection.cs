using System;

namespace Darkness
{
    public class ScreenSelection
    {
        public ScreenAction Action { get; private set; }
        public ItemStack ItemStack { get; private set; }
        public EquipmentSlot? EquipmentSlot { get; private set; }

        public ScreenSelection(
            ScreenAction action,
            ItemStack itemStack,
            EquipmentSlot? equipmentSlot)
        {
            Action = action;
            ItemStack = itemStack;
            EquipmentSlot = equipmentSlot;
        }
    }
}
