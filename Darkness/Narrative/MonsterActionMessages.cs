namespace Darkness
{
    public static class MonsterActionMessages
    {
        public static string DecisionFallback(
            string monsterName,
            MonsterState previousState,
            MonsterDecision decision)
        {
            if (decision == null)
            {
                return null;
            }

            string stateMessage = previousState == decision.NextState
                ? null
                : StateChanged(monsterName, decision.NextState);
            string actionMessage = ActionPlanned(
                monsterName,
                decision.Action);

            if (string.IsNullOrEmpty(stateMessage))
            {
                return actionMessage;
            }

            if (string.IsNullOrEmpty(actionMessage))
            {
                return stateMessage;
            }

            return stateMessage + " " + actionMessage;
        }

        private static string StateChanged(
            string monsterName,
            MonsterState state)
        {
            switch (state)
            {
                case MonsterState.Indifferent:
                    return monsterName +
                        "은(는) 당신에게서 관심을 거둔다.";
                case MonsterState.Alert:
                    return monsterName +
                        "은(는) 인기척을 알아차리고 주변을 경계한다.";
                case MonsterState.Detected:
                    return monsterName +
                        "은(는) 당신의 위치를 정확히 알아낸다.";
                case MonsterState.Combat:
                    return monsterName +
                        "은(는) 당신을 적으로 판단한다.";
                default:
                    return null;
            }
        }

        private static string ActionPlanned(
            string monsterName,
            MonsterActionPlan action)
        {
            if (action == null)
            {
                return null;
            }

            switch (action.Type)
            {
                case MonsterActionType.Wait:
                    return monsterName +
                        "은(는) 움직임을 멈추고 주변의 변화를 살핀다.";
                case MonsterActionType.Attack:
                    return monsterName +
                        "은(는) 당신을 노리고 공격 태세를 갖춘다.";
                case MonsterActionType.Move:
                    return monsterName +
                        "은(는) 다른 위치로 자리를 옮기려 한다.";
                case MonsterActionType.Defend:
                    return monsterName +
                        "은(는) 몸을 웅크려 당신의 공격에 대비한다.";
                case MonsterActionType.UseSkill:
                    return monsterName +
                        "은(는) 당신을 향해 특별한 행동을 준비한다.";
                default:
                    return null;
            }
        }
    }
}
