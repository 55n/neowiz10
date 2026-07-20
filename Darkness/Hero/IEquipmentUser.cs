namespace Darkness
{
    public interface IEquipmentUser
    {
        Item GetEquippedItem(EquipmentSlot slot);
        bool RemoveEquippedItem(Item item);
    }
}
