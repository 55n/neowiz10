namespace Darkness
{
    public class MonsterDecision
    {
        public MonsterState NextState { get; private set; }
        public MonsterActionPlan Action { get; private set; }
        public string Message { get; private set; }
        public bool RevealSlot { get; private set; }

        public MonsterDecision(
            MonsterState nextState,
            MonsterActionPlan action,
            string message,
            bool revealSlot)
        {
            NextState = nextState;
            Action = action ?? MonsterActionPlan.None();
            Message = message;
            RevealSlot = revealSlot;
        }

        public static MonsterDecision None(MonsterState state)
        {
            return new MonsterDecision(
                state,
                MonsterActionPlan.None(),
                null,
                false);
        }
    }
}
