namespace Darkness
{
    public interface ISlotMovementBehavior
    {
        bool CanEnter(ISlotContent content);
        ISlotContent CreateContentForVacatedSlot(RoomSlot slot);
    }
}
