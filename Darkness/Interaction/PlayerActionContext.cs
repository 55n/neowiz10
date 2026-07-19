namespace Darkness
{
    public class PlayerActionContext
    {
        public Hero Actor { get; private set; }
        public PlayerActionType Action { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public ItemStack Item { get; private set; }

        public PlayerActionContext(
            Hero actor,
            PlayerActionType action,
            RoomSlot targetSlot,
            ItemStack item)
        {
            Actor = actor;
            Action = action;
            TargetSlot = targetSlot;
            Item = item;
        }
    }
}
