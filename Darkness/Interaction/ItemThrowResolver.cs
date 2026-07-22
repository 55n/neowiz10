namespace Darkness
{
    public static class ItemThrowResolver
    {
        public static SlotInteractionResult Resolve(
            PlayerActionContext context)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            if (context == null ||
                context.Action != PlayerActionType.ThrowItem ||
                context.Actor == null || context.TargetSlot == null ||
                context.Item == null || context.Item.Item == null ||
                context.Item.Count <= 0)
            {
                return result;
            }

            Item thrownItem;
            if (!TryTakeItem(context, out thrownItem))
            {
                return result;
            }

            bool isLostInTerrain =
                context.TargetSlot.Content is Water ||
                context.TargetSlot.Content is Sand ||
                context.TargetSlot.Type.ObjectType ==
                    RoomObjectType.Sand;
            ItemStack landedStack = new ItemStack(thrownItem, 1);
            if (!isLostInTerrain &&
                context.TargetSlot.GroundInventory.Store(landedStack) != 0)
            {
                RestoreItem(context, thrownItem);
                return result;
            }

            object content = context.TargetSlot.Content;
            IPlayerTargetability targetability =
                content as IPlayerTargetability;
            bool canBeTargeted = targetability == null ||
                targetability.CanBeTargetedByPlayer;
            IDamageable target = canBeTargeted
                ? content as IDamageable
                : null;
            IEffectTarget effectTarget = canBeTargeted
                ? content as IEffectTarget
                : null;
            AttackContext impactAttack = target == null
                ? null
                : CreateImpactAttack(context.Actor, thrownItem, target);
            result.ItemThrows.Add(new ItemThrowPlan(
                context.Actor,
                thrownItem,
                context.TargetSlot,
                target,
                effectTarget,
                impactAttack,
                thrownItem.Type.ThrowEffects));
            result.Messages.Add(
                InventoryMessages.ItemThrown(thrownItem.Type.Name));
            if (isLostInTerrain)
            {
                string terrain = context.TargetSlot.Content is Water
                    ? "물속"
                    : "모래 속";
                result.Messages.Add(
                    InventoryMessages.ItemLostInTerrain(
                        thrownItem.Type.Name,
                        terrain));
            }

            return result;
        }

        private static AttackContext CreateImpactAttack(
            Hero thrower,
            Item item,
            IDamageable target)
        {
            int damage = item.Type.Category == ItemCategory.Weapon
                ? item.Attack
                : item.Type.Weight;
            return new AttackContext(
                thrower,
                target,
                damage,
                thrower.Accuracy * 50 / 100,
                target.Evasion,
                AttackDeliveryType.ThrownItem,
                item,
                2);
        }

        private static bool TryTakeItem(
            PlayerActionContext context,
            out Item item)
        {
            item = context.Item.Item;
            if (context.ItemSourceEquipmentSlot.HasValue)
            {
                EquipmentSlot slot =
                    context.ItemSourceEquipmentSlot.Value;
                ItemStack equipped;
                if (!context.Actor.Equipment.TryGetValue(
                        slot,
                        out equipped) ||
                    !ReferenceEquals(equipped, context.Item))
                {
                    return false;
                }

                context.Actor.Equipment[slot] = null;
                return true;
            }

            return context.Actor.Inventory.Discard(
                context.Item,
                1) == 1;
        }

        private static void RestoreItem(
            PlayerActionContext context,
            Item item)
        {
            if (context.ItemSourceEquipmentSlot.HasValue)
            {
                context.Actor.Equipment[
                    context.ItemSourceEquipmentSlot.Value] =
                    new ItemStack(item, 1);
                return;
            }

            context.Actor.Inventory.Store(
                new ItemStack(item, 1));
        }
    }
}
