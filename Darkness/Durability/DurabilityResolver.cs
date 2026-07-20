using System;

namespace Darkness
{
    public class DurabilityResolver
    {
        private const int DefenseDurabilityCost = 1;

        public DurabilityResolveResult Resolve(
            EquipmentDurabilityRequest request)
        {
            DurabilityResolveResult result =
                new DurabilityResolveResult();
            if (request == null)
            {
                return result;
            }

            Item item = request.Owner.GetEquippedItem(request.Slot);
            if (item != null)
            {
                item.SetDurability(request.Durability);
            }

            return result;
        }

        public DurabilityResolveResult ResolveAttack(
            AttackContext attack,
            AttackResult attackResult,
            Room room)
        {
            DurabilityResolveResult result =
                new DurabilityResolveResult();
            if (attack == null || attackResult == null ||
                !attackResult.IsHit)
            {
                return result;
            }

            Consume(
                attack.UsedItem,
                attack.UsedItemDurabilityCost,
                attack,
                room,
                result);

            IEquipmentUser defender =
                attack.Target as IEquipmentUser;
            IEffectTarget effectTarget =
                attack.Target as IEffectTarget;
            if (defender == null)
            {
                return result;
            }

            bool isDefending = effectTarget != null &&
                effectTarget.Effects.Exists(
                    effect => effect.Type != null &&
                              effect.Type.Id == "defending");
            EquipmentSlot defendedSlot = isDefending
                ? EquipmentSlot.Weapon
                : EquipmentSlot.Armor;
            Consume(
                defender.GetEquippedItem(defendedSlot),
                DefenseDurabilityCost,
                attack,
                room,
                result);
            return result;
        }

        private static void Consume(
            Item item,
            int amount,
            AttackContext attack,
            Room room,
            DurabilityResolveResult result)
        {
            if (item == null || !item.ConsumeDurability(amount))
            {
                return;
            }

            RemoveItem(item, attack, room);
            result.Messages.Add(
                EquipmentMessages.Broken(item.Type.Name));
        }

        private static bool RemoveItem(
            Item item,
            AttackContext attack,
            Room room)
        {
            if (RemoveFromDamageable(attack.Source, item) ||
                RemoveFromDamageable(attack.Target, item))
            {
                return true;
            }

            if (room == null)
            {
                return false;
            }

            foreach (RoomSlot slot in room.Slots)
            {
                if (slot.GroundInventory.RemoveItem(item))
                {
                    return true;
                }

                Monster monster = slot.Content as Monster;
                if (monster != null &&
                    monster.Inventory.RemoveItem(item))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool RemoveFromDamageable(
            IDamageable target,
            Item item)
        {
            IEquipmentUser equipmentUser =
                target as IEquipmentUser;
            if (equipmentUser != null &&
                equipmentUser.RemoveEquippedItem(item))
            {
                return true;
            }

            ISkillUser skillUser = target as ISkillUser;
            return skillUser != null &&
                   skillUser.Inventory != null &&
                   skillUser.Inventory.RemoveItem(item);
        }
    }
}
