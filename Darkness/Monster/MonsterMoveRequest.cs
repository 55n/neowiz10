namespace Darkness
{
    public class MonsterMoveRequest
    {
        public Monster Monster { get; private set; }
        public RoomSlot TargetSlot { get; private set; }

        public MonsterMoveRequest(
            Monster monster,
            RoomSlot targetSlot)
        {
            Monster = monster;
            TargetSlot = targetSlot;
        }
    }
}
