namespace Darkness
{
    public class PlayerTurnCommand
    {
        public PlayerActionType Action { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public ItemStack Item { get; private set; }
        public bool ConsumesTurn { get; private set; }

        public PlayerTurnCommand(
            PlayerActionType action,
            RoomSlot targetSlot,
            ItemStack item,
            bool consumesTurn)
        {
            Action = action;
            TargetSlot = targetSlot;
            Item = item;
            ConsumesTurn = consumesTurn;
        }
    }
}
