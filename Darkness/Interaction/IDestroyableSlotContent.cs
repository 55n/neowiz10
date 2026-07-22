namespace Darkness
{
    public interface IDestroyableSlotContent :
        ISlotContent,
        IDamageable
    {
        bool IsDestroyed { get; }

        SlotDestructionResult ResolveDestruction(
            Room room,
            RoomSlot slot);
    }
}
