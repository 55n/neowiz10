namespace Darkness
{
    public class CeilingWebBehavior : IMonsterBehavior
    {
        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.UseSkill("web_bind"),
                "천장의 거미줄이 떨어져 당신의 몸을 휘감는다.");
        }
    }
}
