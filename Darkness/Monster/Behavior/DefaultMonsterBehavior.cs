namespace Darkness
{
    public class DefaultMonsterBehavior : IMonsterBehavior
    {
        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionContext action = perception.PlayerAction;
            if (action.Action == PlayerActionType.Move &&
                (monster.State == MonsterState.Alert ||
                 monster.State == MonsterState.Detected))
            {
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Attack(),
                    monster.Name + "이(가) 달아나는 플레이어를 공격한다.");
            }

            bool isTarget = action.TargetSlot == perception.CurrentSlot;
            if (!isTarget)
            {
                return MonsterDecision.None(monster.State);
            }

            if (action.Action == PlayerActionType.Attack)
            {
                return new MonsterDecision(
                    MonsterState.Detected,
                    MonsterActionPlan.Attack(),
                    monster.Name + "이(가) 반격을 준비한다.");
            }

            if (action.Action == PlayerActionType.ThrowItem &&
                action.Item != null)
            {
                bool damagingHit = isTarget &&
                    action.Item.Item.Type.ThrowFunction == ItemFunction.Damage;
                if (damagingHit)
                {
                    return new MonsterDecision(
                        MonsterState.Detected,
                        MonsterActionPlan.Attack(),
                        monster.Name + "이(가) 투척 공격에 반격을 준비한다.");
                }

                if (monster.State == MonsterState.Indifferent)
                {
                    return new MonsterDecision(
                        MonsterState.Alert,
                        MonsterActionPlan.Wait(),
                        monster.Name + "이(가) 물건이 부딪힌 소리를 알아차린다.");
                }
            }

            if (action.Action == PlayerActionType.Search &&
                monster.State == MonsterState.Indifferent)
            {
                return new MonsterDecision(
                    MonsterState.Alert,
                    MonsterActionPlan.Wait(),
                    monster.Name + "이(가) 인기척을 알아차린다.");
            }

            return MonsterDecision.None(monster.State);
        }
    }
}
