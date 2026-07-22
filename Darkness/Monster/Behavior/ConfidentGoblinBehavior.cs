using System;

namespace Darkness
{
    public class ConfidentGoblinBehavior :
        IMonsterBehavior,
        IBaitResponsiveBehavior
    {
        private readonly LostGoblinBehavior loneBehavior;
        private bool packLossAnnounced;

        public ConfidentGoblinBehavior()
        {
            loneBehavior = new LostGoblinBehavior();
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            if (monster == null || perception == null)
            {
                return MonsterDecision.None(
                    monster == null
                        ? MonsterState.Indifferent
                        : monster.State);
            }

            bool hasLivingPackMate = HasLivingPackMate(
                monster,
                perception.CurrentRoom);
            if (!hasLivingPackMate)
            {
                MonsterDecision loneDecision =
                    loneBehavior.Decide(monster, perception);
                if (packLossAnnounced)
                {
                    return loneDecision;
                }

                packLossAnnounced = true;
                string transitionMessage = NarrativeTokens.Actor +
                    "은(는) 쓰러진 동료를 보고 자신감을 잃는다.";
                string message = string.IsNullOrEmpty(
                    loneDecision.Message)
                    ? transitionMessage
                    : transitionMessage + " " +
                      loneDecision.Message;
                return new MonsterDecision(
                    loneDecision.NextState,
                    loneDecision.Action,
                    message);
            }

            if (monster.State != MonsterState.Combat)
            {
                return loneBehavior.Decide(monster, perception);
            }

            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(),
                NarrativeTokens.Actor +
                "은(는) 동료와 함께 당신을 포위하며 공격한다.");
        }

        private static bool HasLivingPackMate(
            Monster monster,
            Room room)
        {
            if (room == null)
            {
                return false;
            }

            foreach (RoomSlot slot in room.Slots)
            {
                Monster candidate = slot.Content as Monster;
                if (candidate != null &&
                    !ReferenceEquals(candidate, monster) &&
                    candidate.CanAct &&
                    candidate.Behavior is ConfidentGoblinBehavior)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
