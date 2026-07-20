using System;
using System.Collections.Generic;

namespace Darkness
{
    public class TreasureChest : IDamageable, ISlotContent
    {
        public string Name { get { return "보물 상자"; } }
        public int CurrentHealth { get; private set; }
        public int Defense { get; private set; }
        public int Evasion { get { return 0; } }
        public List<ActiveEffect> Effects { get; private set; }
        public Inventory Inventory { get; private set; }
        public bool IsOpened { get; private set; }
        public bool IsDestroyed { get { return CurrentHealth <= 0; } }

        public TreasureChest(
            int maxHealth,
            int defense,
            Inventory inventory)
        {
            CurrentHealth = Math.Max(1, maxHealth);
            Defense = Math.Max(0, defense);
            Inventory = inventory;
            Effects = new List<ActiveEffect>();
        }

        public void ReceiveDamage(int damage)
        {
            CurrentHealth = Math.Max(
                0,
                CurrentHealth - Math.Max(0, damage));
            if (IsDestroyed)
            {
                Inventory.ItemStacks.Clear();
            }
        }

        public SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (context.Action == PlayerActionType.Search)
            {
                Search(context.Actor, result);
            }
            else if (context.Action == PlayerActionType.Attack)
            {
                AddAttack(context.Actor, result);
            }
            else
            {
                result.Messages.Add(
                    ExplorationMessages.NoResponse());
            }

            return result;
        }

        private void Search(
            Hero hero,
            SlotInteractionResult result)
        {
            IsOpened = true;
            if (Inventory.ItemStacks.Count == 0)
            {
                result.Messages.Add(
                    ExplorationMessages.TreasureChestEmpty());
                return;
            }

            bool inventoryFull = false;
            foreach (ItemStack itemStack in
                     Inventory.ItemStacks.ToArray())
            {
                int itemCount = itemStack.Count;
                int transferred = Inventory.TransferTo(
                    itemStack,
                    itemCount,
                    hero.Inventory);
                if (transferred > 0)
                {
                    result.Messages.Add(
                        InventoryMessages.ItemObtained(
                            itemStack.Item.Type.Name));
                }

                if (transferred < itemCount)
                {
                    inventoryFull = true;
                }
            }

            if (inventoryFull)
            {
                result.Messages.Add(
                    InventoryMessages.InventoryFull());
            }
        }

        private void AddAttack(
            Hero hero,
            SlotInteractionResult result)
        {
            Item weapon = hero.GetEquippedItem(
                EquipmentSlot.Weapon);
            result.Attacks.Add(new AttackContext(
                hero,
                this,
                hero.Attack,
                hero.Accuracy,
                Evasion,
                weapon == null
                    ? AttackDeliveryType.Natural
                    : AttackDeliveryType.EquippedWeapon,
                weapon,
                weapon == null ? 0 : 1));
        }
    }
}
