namespace Darkness
{
    public class Room11MoveInterceptor : IMoveInterceptor
    {
        private readonly RoomSlotContentFactory contentFactory;
        private bool hasTriggered;

        public Room11MoveInterceptor(
            RoomSlotContentFactory contentFactory)
        {
            this.contentFactory = contentFactory;
        }

        public MoveInterceptionResult TryIntercept(
            MoveInterceptionContext context)
        {
            if (hasTriggered || context == null ||
                context.CurrentRoom == null ||
                context.CurrentRoom.Slots.Count != 5)
            {
                return MoveInterceptionResult.Continue();
            }

            hasTriggered = true;
            SlotInteractionResult interaction =
                new SlotInteractionResult();
            interaction.Messages.Add(
                "문틀을 점액질이 뒤덮는다.");
            interaction.Messages.Add(
                "당신은 당황해서 물러섰다.");
            if (HasMagicStone(context.Hero))
            {
                interaction.Messages.Add(
                    "주머니 속 마석들이 거칠게 떨리기 시작한다.");
            }

            for (int slotIndex = 0; slotIndex < 5; slotIndex++)
            {
                RoomSlot slot =
                    context.CurrentRoom.Slots[slotIndex];
                ISlotContent part =
                    contentFactory.CreateRoom11DoorClingerPart(
                        slotIndex);

                interaction.SlotStateChanges.Add(
                    SlotStateChangeRequest.Unreveal(slot));
                interaction.SlotContentChanges.Add(
                    slot.IsEmpty
                        ? SlotContentChangeRequest.Place(
                            slot,
                            part)
                        : SlotContentChangeRequest.Replace(
                            slot,
                            slot.Content,
                            part));
            }

            return MoveInterceptionResult.Cancel(interaction);
        }

        private static bool HasMagicStone(Hero hero)
        {
            return hero != null && hero.Inventory != null &&
                   hero.Inventory.ItemStacks.Exists(stack =>
                       stack != null && stack.Item != null &&
                       stack.Count > 0 &&
                       stack.Item.Type.Id == "magic_stone");
        }
    }
}
