namespace Darkness
{
    public class MonsterActionPlan
    {
        public MonsterActionType Type { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public string SkillId { get; private set; }
        public MonsterState? StateAfterAction { get; private set; }

        private MonsterActionPlan(
            MonsterActionType type,
            RoomSlot targetSlot,
            string skillId,
            MonsterState? stateAfterAction)
        {
            Type = type;
            TargetSlot = targetSlot;
            SkillId = skillId;
            StateAfterAction = stateAfterAction;
        }

        public static MonsterActionPlan None()
        {
            return new MonsterActionPlan(
                MonsterActionType.None, null, null, null);
        }

        public static MonsterActionPlan Wait()
        {
            return new MonsterActionPlan(
                MonsterActionType.Wait, null, null, null);
        }

        public static MonsterActionPlan Attack(
            MonsterState? stateAfterAction = null)
        {
            return new MonsterActionPlan(
                MonsterActionType.Attack,
                null,
                null,
                stateAfterAction);
        }

        public static MonsterActionPlan MoveTo(
            RoomSlot targetSlot,
            MonsterState? stateAfterAction = null)
        {
            return new MonsterActionPlan(
                MonsterActionType.Move,
                targetSlot,
                null,
                stateAfterAction);
        }

        public static MonsterActionPlan Defend()
        {
            return new MonsterActionPlan(
                MonsterActionType.Defend, null, null, null);
        }

        public static MonsterActionPlan UseSkill(string skillId)
        {
            return new MonsterActionPlan(
                MonsterActionType.UseSkill, null, skillId, null);
        }
    }
}
