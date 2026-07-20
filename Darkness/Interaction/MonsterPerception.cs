namespace Darkness
{
    public class MonsterPerception
    {
        public PlayerActionContext PlayerAction { get; private set; }
        public Room CurrentRoom { get; private set; }
        public RoomSlot CurrentSlot { get; private set; }
        public bool LoudEventOccurred { get; private set; }

        public MonsterPerception(
            PlayerActionContext playerAction,
            Room currentRoom,
            RoomSlot currentSlot,
            bool loudEventOccurred)
        {
            PlayerAction = playerAction;
            CurrentRoom = currentRoom;
            CurrentSlot = currentSlot;
            LoudEventOccurred = loudEventOccurred;
        }
    }
}
