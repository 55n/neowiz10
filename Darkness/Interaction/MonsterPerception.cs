namespace Darkness
{
    public class MonsterPerception
    {
        public PlayerActionContext PlayerAction { get; private set; }
        public Room CurrentRoom { get; private set; }
        public RoomSlot CurrentSlot { get; private set; }

        public MonsterPerception(
            PlayerActionContext playerAction,
            Room currentRoom,
            RoomSlot currentSlot)
        {
            PlayerAction = playerAction;
            CurrentRoom = currentRoom;
            CurrentSlot = currentSlot;
        }
    }
}
