namespace Darkness
{
    public class HungryTrollBehavior : IMonsterBehavior
    {
        private readonly DefaultMonsterBehavior defaultBehavior;

        public HungryTrollBehavior()
        {
            defaultBehavior = new DefaultMonsterBehavior();
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionContext action = perception.PlayerAction;
            if (action.Action == PlayerActionType.ThrowItem &&
                action.Item != null &&
                action.Item.Item.Type.ThrowFunction == ItemFunction.Lure &&
                action.TargetSlot.IsEmpty &&
                action.TargetSlot != perception.CurrentSlot)
            {
                return new MonsterDecision(
                    MonsterState.Alert,
                    MonsterActionPlan.MoveTo(action.TargetSlot),
                    monster.Name + "이(가) 미끼 냄새를 맡고 움직인다.");
            }

            return defaultBehavior.Decide(monster, perception);
        }
    }
}
