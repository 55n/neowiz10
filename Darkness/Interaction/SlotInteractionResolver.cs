namespace Darkness
{
    public static class SlotInteractionResolver
    {
        public static SlotInteractionResult Resolve(
            PlayerActionContext context)
        {
            if (context == null || context.TargetSlot == null)
            {
                return new SlotInteractionResult();
            }

            SlotInteractionResult result = ResolveSlot(context);
            if (context.TargetSlot.Content != null)
            {
                result.Merge(context.TargetSlot.Content.React(context));
            }

            return result;
        }

        private static SlotInteractionResult ResolveSlot(
            PlayerActionContext context)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            if (context.Action != PlayerActionType.Search)
            {
                if (context.TargetSlot != null &&
                    context.TargetSlot.IsEmpty &&
                    IsEmptySlotAction(context.Action))
                {
                    result.Messages.Add(
                        ExplorationMessages.NothingHappened());
                }

                return result;
            }

            RoomSlot slot = context.TargetSlot;
            result.RevealSlot = true;
            bool foundSomething = false;
            bool inventoryFull = false;

            if (slot.IsEmpty && slot.Type.HasDoor)
            {
                result.Messages.Add(ExplorationMessages.DoorFound());
                foundSomething = true;
            }

            foreach (ItemStack itemStack in
                     slot.GroundInventory.ItemStacks.ToArray())
            {
                foundSomething = true;
                int itemCount = itemStack.Count;
                int transferred = slot.GroundInventory.TransferTo(
                    itemStack,
                    itemCount,
                    context.Actor.Inventory);
                if (transferred > 0)
                {
                    result.Messages.Add(
                        InventoryMessages.ItemObtained(itemStack.Item.Type.Name));
                }

                if (transferred < itemCount)
                {
                    inventoryFull = true;
                }
            }

            if (inventoryFull)
            {
                result.Messages.Add(InventoryMessages.InventoryFull());
            }

            if (slot.IsEmpty && !foundSomething)
            {
                result.Messages.Add(ExplorationMessages.EmptySlotFound());
            }

            return result;
        }

        private static bool IsEmptySlotAction(PlayerActionType action)
        {
            return action == PlayerActionType.Wait ||
                   action == PlayerActionType.Talk ||
                   action == PlayerActionType.Attack ||
                   action == PlayerActionType.Defend ||
                   action == PlayerActionType.UseSkill;
        }
    }
}
