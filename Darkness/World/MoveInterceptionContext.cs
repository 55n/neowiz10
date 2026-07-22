namespace Darkness
{
    public class MoveInterceptionContext
    {
        public Hero Hero { get; private set; }
        public Room CurrentRoom { get; private set; }
        public RoomDirection Direction { get; private set; }
        public RoomEdge Edge { get; private set; }

        public MoveInterceptionContext(
            Hero hero,
            Room currentRoom,
            RoomDirection direction,
            RoomEdge edge)
        {
            Hero = hero;
            CurrentRoom = currentRoom;
            Direction = direction;
            Edge = edge;
        }
    }
}
