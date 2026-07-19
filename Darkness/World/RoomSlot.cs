namespace Darkness
{
    public class RoomSlot
    {
        private const int GroundInventoryCapacity = 100;

        public RoomSlotType Type { get; private set; }
        public SlotState State { get; private set; }
        public ISlotContent Content { get; private set; }
        public Inventory GroundInventory { get; private set; }
        public bool IsEmpty
        {
            get
            {
                return Content == null &&
                       Type.ObjectType != RoomObjectType.TreasureChest &&
                       Type.ObjectType != RoomObjectType.Pile;
            }
        }

        public RoomSlot(RoomSlotType type, ISlotContent content)
        {
            Type = type;
            Content = content;
            State = SlotState.UNREVEALED;
            GroundInventory = new Inventory(GroundInventoryCapacity);
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
