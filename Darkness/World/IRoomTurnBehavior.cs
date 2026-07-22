namespace Darkness
{
    public interface IRoomTurnBehavior
    {
        SlotInteractionResult Act(
            Room room,
            Hero hero,
            PlayerActionContext playerAction);
    }
}
