namespace Darkness
{
    public interface ISlotContent
    {
        string Name { get; }

        SlotInteractionResult React(PlayerActionContext context);
    }
}
