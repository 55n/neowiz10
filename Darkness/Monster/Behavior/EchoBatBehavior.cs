namespace Darkness
{
    public class EchoBatBehavior : IMonsterBehavior, IDefeatBehavior
    {
        private int remainingBatCount;

        public EchoBatBehavior(int batCount)
        {
            remainingBatCount = batCount < 1 ? 1 : batCount;
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            return new MonsterDecision(
                MonsterState.Detected,
                MonsterActionPlan.Attack(),
                null);
        }

        public DefeatBehaviorResult ResolveDefeat(Monster monster)
        {
            remainingBatCount--;
            if (remainingBatCount <= 0)
            {
                return DefeatBehaviorResult.Complete();
            }

            monster.ResetAfterDefeat(MonsterState.Detected);
            return DefeatBehaviorResult.Continue(
                EchoBatMessages.AnotherBatAppears());
        }
    }
}
