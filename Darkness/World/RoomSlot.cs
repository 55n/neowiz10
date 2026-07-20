namespace Darkness
{
    public class RoomSlot
    {
        private const int GroundInventoryCapacity = 100;

        public RoomSlotType Type { get; private set; }
        public SlotState State { get; private set; }
        public ISlotContent Content { get; private set; }
        public Inventory GroundInventory { get; private set; }
        public bool IsDoorDiscovered { get; private set; }
        public bool IsEmpty
        {
            get
            {
                return Content == null;
            }
        }

        public RoomSlot(RoomSlotType type, ISlotContent content)
        {
            Type = type;
            Content = content;
            State = SlotState.UNREVEALED;
            IsDoorDiscovered = false;
            GroundInventory = new Inventory(GroundInventoryCapacity);
        }

        public bool TryDiscoverDoor()
        {
            if (IsDoorDiscovered || !Type.HasDoor || !IsEmpty ||
                State != SlotState.REVEALED)
            {
                return false;
            }

            IsDoorDiscovered = true;
            return true;
        }

        public void SetState(SlotState state)
        {
            State = state;
        }

        public void Reveal()
        {
            State = SlotState.REVEALED;
        }

        public void ClearContent()
        {
            Content = null;
        }

        public void SetContent(ISlotContent content)
        {
            Content = content;
        }

        public ISlotContent TakeContent()
        {
            ISlotContent content = Content;
            Content = null;
            return content;
        }
    }
}
