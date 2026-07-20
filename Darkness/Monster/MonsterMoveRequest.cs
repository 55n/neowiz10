namespace Darkness
{
    public class MonsterMoveRequest
    {
        public Monster Monster { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public MonsterState? StateAfterMove { get; private set; }

        public MonsterMoveRequest(
            Monster monster,
            RoomSlot targetSlot,
            MonsterState? stateAfterMove = null)
        {
            Monster = monster;
            TargetSlot = targetSlot;
            StateAfterMove = stateAfterMove;
        }
    }
}
