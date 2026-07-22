namespace Darkness
{
    public class VibrationWormBehavior :
        IMonsterBehavior,
        ISlotMovementBehavior,
        IPlayerTargetability,
        ISlotAppearance
    {
        private int lureTurnsRemaining;

        public bool CanBeTargetedByPlayer
        {
            get { return false; }
        }

        public string SlotDisplayName
        {
            get { return "모래"; }
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionContext action = perception == null
                ? null
                : perception.PlayerAction;
            if (action == null)
            {
                return Ignore(monster);
            }

            int lureTurns = GetLureTurns(action);
            if (lureTurns >= 1)
            {
                lureTurnsRemaining = lureTurns;
                if (action.TargetSlot == perception.CurrentSlot)
                {
                    return new MonsterDecision(
                        monster.State,
                        MonsterActionPlan.Wait(),
                        "무거운 물건이 파묻힌 자리 아래에서 거대한 몸이 맴돈다.");
                }

                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.MoveTo(
                        action.TargetSlot,
                        monster.State),
                    "모래 아래의 거대한 무언가가 충돌 지점을 향해 파고든다.");
            }

            if (lureTurnsRemaining > 0)
            {
                lureTurnsRemaining--;
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "무거운 물건이 파묻힌 곳에서 모래가 계속 들썩인다.");
            }

            if (action.Action == PlayerActionType.Search ||
                action.Action == PlayerActionType.Move)
            {
                return Attack();
            }

            return Ignore(monster);
        }

        public bool CanEnter(ISlotContent content)
        {
            return content is Sand;
        }

        public ISlotContent CreateContentForVacatedSlot(
            RoomSlot slot)
        {
            return new Sand(slot.Type.ObjectTypeId);
        }

        private static int GetLureTurns(
            PlayerActionContext action)
        {
            if (action.Action != PlayerActionType.ThrowItem ||
                action.Item == null || action.Item.Item == null ||
                action.TargetSlot == null)
            {
                return 0;
            }

            return action.Item.Item.Type.Weight / 2;
        }

        private static MonsterDecision Attack()
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(MonsterState.Indifferent),
                "발밑의 모래가 폭발하듯 솟구치며 거대한 턱이 당신을 덮친다. " +
                "괴물은 공격 직후 다시 모래 아래로 사라졌다.");
        }

        private static MonsterDecision Ignore(Monster monster)
        {
            return new MonsterDecision(
                monster.State,
                MonsterActionPlan.Wait(),
                "아무 반응도 없다.");
        }
    }
}
