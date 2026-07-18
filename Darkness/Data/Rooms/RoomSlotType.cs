namespace Darkness
{
    public class RoomSlotType
    {
        public RoomObjectType ObjectType { get; private set; }
        public string ObjectTypeId { get; private set; }
        public bool HasDoor {  get; private set; }

        public RoomSlotType(
            RoomObjectType objectType,
            string objectTypeId,
            bool hasDoor)
        {
            ObjectType = objectType;
            ObjectTypeId = objectTypeId;
            HasDoor = hasDoor;
        }
    }
}
