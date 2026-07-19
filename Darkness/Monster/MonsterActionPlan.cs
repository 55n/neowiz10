namespace Darkness
{
    public class MonsterActionPlan
    {
        public MonsterActionType Type { get; private set; }
        public RoomSlot TargetSlot { get; private set; }

        private MonsterActionPlan(
            MonsterActionType type,
            RoomSlot targetSlot)
        {
            Type = type;
            TargetSlot = targetSlot;
        }

        public static MonsterActionPlan None()
        {
            return new MonsterActionPlan(MonsterActionType.None, null);
        }

        public static MonsterActionPlan Wait()
        {
            return new MonsterActionPlan(MonsterActionType.Wait, null);
        }

        public static MonsterActionPlan Attack()
        {
            return new MonsterActionPlan(MonsterActionType.Attack, null);
        }

        public static MonsterActionPlan MoveTo(RoomSlot targetSlot)
        {
            return new MonsterActionPlan(MonsterActionType.Move, targetSlot);
        }
    }
}
