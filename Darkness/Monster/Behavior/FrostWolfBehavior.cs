namespace Darkness
{
    public class FrostWolfBehavior : IMonsterBehavior
    {
        private readonly WolfPack pack;

        public FrostWolfBehavior(WolfPack pack)
        {
            this.pack = pack;
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            string packMessage = GetPackMessage();
            if (monster.State == MonsterState.Combat ||
                monster.State == MonsterState.Detected)
            {
                return Attack(
                    packMessage + GetCombatMessage(monster));
            }

            if (IsDirectlyProvoked(perception))
            {
                return Attack(
                    packMessage + NarrativeTokens.Actor +
                    "이(가) 당신의 위치를 알아차리고 달려든다.");
            }

            if (monster.AdvanceDetection())
            {
                return Attack(
                    packMessage + NarrativeTokens.Actor +
                    "이(가) 냄새를 따라 당신의 위치를 찾아냈다.");
            }

            return new MonsterDecision(
                MonsterState.Alert,
                MonsterActionPlan.Wait(),
                packMessage + NarrativeTokens.Actor +
                "이(가) 코를 바닥에 대고 냄새를 더듬는다. " +
                monster.DetectionTurnsRemaining +
                "턴 안에 발각될 것 같다.");
        }

        private string GetPackMessage()
        {
            if (pack == null)
            {
                return "";
            }

            string message = "";
            if (pack.TryAnnounceTactics())
            {
                message += "늑대들의 발소리가 서로 호응한다. " +
                    "함께 있는 늑대가 많을수록 공격과 방어가 강해지는 것 같다. ";
            }

            int livingCount;
            if (pack.TryAnnounceLoss(out livingCount))
            {
                message += livingCount <= 1
                    ? "동료들의 울음이 모두 끊기자 무리의 기세가 완전히 무너졌다. "
                    : "한 늑대의 울음이 끊기자 무리의 기세가 약해졌다. ";
            }

            return message;
        }

        private string GetCombatMessage(Monster monster)
        {
            int allyCount = pack == null
                ? 0
                : pack.GetLivingAllyCount(monster);
            return allyCount > 0
                ? NarrativeTokens.Actor +
                  "이(가) 다른 늑대와 보조를 맞춰 달려든다."
                : NarrativeTokens.Actor +
                  "이(가) 홀로 이를 드러내며 달려든다.";
        }

        private static bool IsDirectlyProvoked(
            MonsterPerception perception)
        {
            if (perception == null ||
                perception.PlayerAction == null ||
                perception.PlayerAction.TargetSlot !=
                    perception.CurrentSlot)
            {
                return false;
            }

            PlayerActionType action =
                perception.PlayerAction.Action;
            return action == PlayerActionType.Attack ||
                   action == PlayerActionType.ThrowItem ||
                   action == PlayerActionType.Search ||
                   action == PlayerActionType.Talk ||
                   action == PlayerActionType.UseSkill;
        }

        private static MonsterDecision Attack(string message)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(
                    MonsterState.Combat),
                message);
        }
    }
}
