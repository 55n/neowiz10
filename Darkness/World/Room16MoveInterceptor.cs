namespace Darkness
{
    public class Room16MoveInterceptor : IMoveInterceptor
    {
        private const int AbdomenSlotIndex = 2;

        public MoveInterceptionResult TryIntercept(
            MoveInterceptionContext context)
        {
            if (context == null || context.CurrentRoom == null ||
                context.Direction != RoomDirection.FORWARD ||
                context.CurrentRoom.Slots.Count <= AbdomenSlotIndex)
            {
                return MoveInterceptionResult.Continue();
            }

            RoomSlot abdomenSlot =
                context.CurrentRoom.Slots[AbdomenSlotIndex];
            if (abdomenSlot.IsEmpty)
            {
                return MoveInterceptionResult.Continue();
            }

            SlotInteractionResult interaction =
                new SlotInteractionResult();
            interaction.Messages.Add(
                "발견한 통로로 향하려 했지만, 숨결사냥꾼의 복부가 출구를 틀어막고 있어 이동할 수 없다.");
            return MoveInterceptionResult.Cancel(interaction, true);
        }
    }
}
