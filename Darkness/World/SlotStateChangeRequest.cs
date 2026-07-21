namespace Darkness
{
    public class SlotStateChangeRequest
    {
        public RoomSlot Slot { get; private set; }
        public SlotState State { get; private set; }

        public SlotStateChangeRequest(
            RoomSlot slot,
            SlotState state)
        {
            Slot = slot;
            State = state;
        }

        public static SlotStateChangeRequest Reveal(RoomSlot slot)
        {
            return new SlotStateChangeRequest(
                slot,
                SlotState.REVEALED);
        }

        public static SlotStateChangeRequest Unreveal(RoomSlot slot)
        {
            return new SlotStateChangeRequest(
                slot,
                SlotState.UNREVEALED);
        }
    }
}
