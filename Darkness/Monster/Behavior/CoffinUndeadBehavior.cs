namespace Darkness
{
    public class CoffinUndeadBehavior : IMonsterBehavior
    {
        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            string message = monster.State == MonsterState.Indifferent
                ? "관 속의 망자가 몸을 일으켜 달려든다."
                : null;
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(),
                message);
        }
    }
}
