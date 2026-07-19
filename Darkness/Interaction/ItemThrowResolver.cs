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
                context.Item == null || context.Item.Item == null)
            {
                return result;
            }

            ItemType itemType = context.Item.Item.Type;
            RoomSlot targetSlot = context.TargetSlot;
            IDamageable target = targetSlot.Content as IDamageable;

            targetSlot.GroundInventory.Store(context.Item);
            if (target == null)
            {
                result.Messages.Add(
                    "[" + itemType.Name + "]이(가) 바닥에 부딪혀 소리를 낸다.");
                return result;
            }

            result.Messages.Add(
                "[" + itemType.Name + "]이(가) [" +
                targetSlot.Content.Name + "]에게 부딪힌다.");
            if (itemType.ThrowFunction == ItemFunction.Damage)
            {
                result.Attacks.Add(new AttackContext(
                    context.Actor,
                    target,
                    itemType.Attack,
                    context.Actor.Type.Accuracy,
                    target.Evasion));
            }

            return result;
        }
    }
}
