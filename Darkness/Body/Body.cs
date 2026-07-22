using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Body : ISlotContent, IDamageable
    {
        public string Name { get { return "해골"; } }
        public int CurrentHealth { get; private set; }
        public int Defense { get; private set; }
        public int Evasion { get { return 0; } }
        public List<ActiveEffect> Effects { get; private set; }
        public Inventory Inventory { get; private set; }

        private readonly PoisonFogTrap hiddenTrap;

        public Body(
            int health,
            int defense,
            Inventory inventory,
            PoisonFogTrap hiddenTrap = null)
        {
            CurrentHealth = Math.Max(1, health);
            Defense = Math.Max(0, defense);
            Inventory = inventory ?? new Inventory(0);
            this.hiddenTrap = hiddenTrap;
            Effects = new List<ActiveEffect>();
        }

        public void ReceiveDamage(int damage)
        {
            CurrentHealth = Math.Max(
                0,
                CurrentHealth - Math.Max(0, damage));
        }

        public SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            if (context == null || context.TargetSlot == null)
            {
                return result;
            }

            if (context.Action == PlayerActionType.Search)
            {
                Search(context, result);
            }
            else if (context.Action == PlayerActionType.Attack)
            {
                Attack(context, result);
            }
            else if (context.Action == PlayerActionType.Talk)
            {
                result.Messages.Add(
                    ExplorationMessages.NoResponse());
            }

            return result;
        }

        private void Search(
            PlayerActionContext context,
            SlotInteractionResult result)
        {
            if (context.TargetSlot.State == SlotState.UNREVEALED)
            {
                result.Messages.Add(BodyMessages.Discovered());
                return;
            }

            TakeItems(context.Actor, result);
            if (hiddenTrap == null)
            {
                return;
            }

            Inventory.TransferAllTo(context.TargetSlot.GroundInventory);
            ActivateTrap(context, result);
        }

        private void Attack(
            PlayerActionContext context,
            SlotInteractionResult result)
        {
            Item weapon = context.Actor.GetEquippedItem(
                EquipmentSlot.Weapon);
            result.Attacks.Add(new AttackContext(
                context.Actor,
                this,
                context.Actor.Attack,
                context.Actor.Accuracy,
                Evasion,
                weapon == null
                    ? AttackDeliveryType.Natural
                    : AttackDeliveryType.EquippedWeapon,
                weapon,
                weapon == null ? 0 : 1));

            if (hiddenTrap != null)
            {
                ActivateTrap(context, result);
            }
        }

        private void TakeItems(
            Hero hero,
            SlotInteractionResult result)
        {
            if (Inventory.ItemStacks.Count == 0)
            {
                result.Messages.Add(BodyMessages.NothingFound());
                return;
            }

            bool inventoryFull = false;
            foreach (ItemStack itemStack in Inventory.ItemStacks.ToArray())
            {
                int count = itemStack.Count;
                int transferred = Inventory.TransferTo(
                    itemStack,
                    count,
                    hero.Inventory);
                if (transferred > 0)
                {
                    result.Messages.Add(InventoryMessages.ItemObtained(
                        itemStack.Item.Type.Name));
                }

                if (transferred < count)
                {
                    inventoryFull = true;
                }
            }

            if (inventoryFull)
            {
                result.Messages.Add(InventoryMessages.InventoryFull());
            }
        }

        private void ActivateTrap(
            PlayerActionContext context,
            SlotInteractionResult result)
        {
            result.LoudEventOccurred = true;
            result.Messages.Add(BodyMessages.PoisonTrapActivated());
            result.SlotContentChanges.Add(
                SlotContentChangeRequest.Replace(
                    context.TargetSlot,
                    this,
                    hiddenTrap));
            result.SlotStateChanges.Add(
                SlotStateChangeRequest.Reveal(context.TargetSlot));
            result.EffectRequests.Add(new EffectRequest(
                hiddenTrap.RoomEffects,
                EffectContext.FromRoomEffect(
                    hiddenTrap,
                    context.Room,
                    context.Actor)));
        }
    }
}
