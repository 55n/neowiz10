namespace Darkness
{
    public class SpiderlingBehavior : IMonsterBehavior
    {
        private bool hasAttacked;

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            string message = hasAttacked
                ? NarrativeTokens.Actor +
                  "이(가) 가느다란 다리로 달려들어 물어뜯으려 한다."
                : NarrativeTokens.Actor +
                  "이(가) 당신의 기척을 알아차리고 알껍질을 박차며 달려든다.";
            hasAttacked = true;
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(MonsterState.Combat),
                message);
        }
    }
}
